#region Usings declarations

using System;
using System.Collections.Generic;
using System.Diagnostics;

#endregion

namespace Reefact.RequestValidation {

    public interface ArgumentsValidator : Validator {

        RequiredArgument<TOutput> ConvertRequired<TInput, TOutput>(string argName, TInput argValue, Func<TInput, TOutput> convertArgValue);

        OptionalArgument<TOutput> ConvertOptional<TInput, TOutput>(string argName, TInput argValue, Func<TInput, TOutput> convertArgValue);

        RequiredArgument<TValue> IsRequired<TValue>(string argName, TValue argValue);

    }

    [DebuggerDisplay("{ToString()}")]
    public sealed class ArgumentsValidator<THandledException> : ArgumentsValidator
        where THandledException : ApplicationException {

        #region Fields declarations

        private readonly string                _argPrefix;
        private readonly List<ValidationError> _errors = new List<ValidationError>();

        #endregion

        #region Constructors declarations

        internal ArgumentsValidator() {
            _argPrefix = null;
        }

        internal ArgumentsValidator(string prefix) {
            _argPrefix = string.IsNullOrWhiteSpace(prefix) ? null : prefix.Trim();
        }

        #endregion

        public bool HasError   => _errors.Count > 0;
        public int  ErrorCount => _errors.Count;

        public RequiredArgument<TOutput> ConvertRequired<TInput, TOutput>(string argName, TInput argValue, Func<TInput, TOutput> convertArgValue) {
            if (argName         == null) { throw new ArgumentNullException(nameof(argName)); }
            if (convertArgValue == null) { throw new ArgumentNullException(nameof(convertArgValue)); }

            string argFullName = GetArgFullName(argName);

            if (argValue == null) {
                var error = new ValidationError(argFullName, "Argument is required.");
                _errors.Add(error);

                return RequiredArgument.CreateInvalid<TOutput>(argFullName, null);
            }

            try {
                TOutput                   convertedArgValue = convertArgValue(argValue);
                RequiredArgument<TOutput> convertRequired   = RequiredArgument.CreateValid(argFullName, argValue, convertedArgValue);

                return convertRequired;
            } catch (BadRequestException ex) {
                _errors.AddRange(ex.ValidationErrors);

                RequiredArgument<TOutput> convertRequired = RequiredArgument.CreateInvalid<TOutput>(argFullName, argValue);

                return convertRequired;
            } catch (THandledException ex) {
                var error = new ValidationError(argFullName, ex.Message);
                _errors.Add(error);
                RequiredArgument<TOutput> convertRequired = RequiredArgument.CreateInvalid<TOutput>(argName, argValue);

                return convertRequired;
            }
        }

        public OptionalArgument<TOutput> ConvertOptional<TInput, TOutput>(string argName, TInput argValue, Func<TInput, TOutput> convertArgValue) {
            if (argName         == null) { throw new ArgumentNullException(nameof(argName)); }
            if (convertArgValue == null) { throw new ArgumentNullException(nameof(convertArgValue)); }

            string argFullName = GetArgFullName(argName);

            try {
                TOutput                   convertedArgValue = convertArgValue(argValue);
                OptionalArgument<TOutput> convertRequired   = OptionalArgument.CreateValid(argFullName, argValue, convertedArgValue);

                return convertRequired;
            } catch (THandledException ex) {
                var error = new ValidationError(argFullName, ex.Message);
                _errors.Add(error);
                OptionalArgument<TOutput> convertRequired = OptionalArgument.CreateInvalid<TOutput>(argFullName, argValue);

                return convertRequired;
            }
        }

        public RequiredArgument<TValue> IsRequired<TValue>(string argName, TValue argValue) {
            if (argName == null) { throw new ArgumentNullException(nameof(argName)); }

            string argFullName = GetArgFullName(argName);

            if (argValue != null) { return RequiredArgument.CreateValid(argFullName, argValue, argValue); }

            var error = new ValidationError(argFullName, "Argument is required.");
            _errors.Add(error);

            return RequiredArgument.CreateInvalid<TValue>(argFullName, argValue);
        }

        public void AddError(ValidationError error) {
            if (error == null) { throw new ArgumentNullException(nameof(error)); }

            _errors.Add(error);
        }

        public void AddErrors(IEnumerable<ValidationError> errors) {
            if (errors == null) { throw new ArgumentNullException(nameof(errors)); }

            _errors.AddRange(errors);
        }

        public void AssertHasNoError() {
            if (HasError) { throw BadRequestException.From(this); }
        }

        public ValidationError[] GetErrors() {
            return _errors.ToArray();
        }

        public override string ToString() {
            if (_errors.Count == 0) { return "No error detected."; }
            if (_errors.Count == 1) { return "1 error has been detected."; }

            return $"{_errors.Count} errors have been detected.";
        }

        private string GetArgFullName(string argName) {
            if (_argPrefix == null) { return argName; }

            return $"{_argPrefix}.{argName}";
        }

    }

}