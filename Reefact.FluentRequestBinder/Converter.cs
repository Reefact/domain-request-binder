#region Usings declarations

using System.Diagnostics;

using Reefact.FluentRequestBinder.Configuration;

#endregion

namespace Reefact.FluentRequestBinder {

    [DebuggerDisplay("{ToString()}")]
    internal sealed class Converter : Validator {

        #region Fields declarations

        private readonly List<ValidationError> _errors = new();

        #endregion

        #region Constructors declarations

        internal Converter(ValidationOptions validationOptions) {
            if (validationOptions is null) { throw new ArgumentNullException(nameof(validationOptions)); }

            Options        = validationOptions;
            ArgumentPrefix = null;
        }

        internal Converter(ValidationOptions validationOptions, string prefix) {
            if (validationOptions is null) { throw new ArgumentNullException(nameof(validationOptions)); }

            Options        = validationOptions;
            ArgumentPrefix = string.IsNullOrWhiteSpace(prefix) ? null : prefix.Trim();
        }

        #endregion

        /// <inheritdoc />
        public bool HasError => _errors.Count > 0;

        /// <inheritdoc />
        public int ErrorCount => _errors.Count;

        internal ValidationOptions Options        { get; }
        internal string?           ArgumentPrefix { get; }

        /// <inheritdoc />
        public void RecordError(ValidationError error) {
            if (error is null) { throw new ArgumentNullException(nameof(error)); }

            _errors.Add(error);
        }

        /// <inheritdoc />
        public void RecordErrors(IEnumerable<ValidationError> errors) {
            if (errors == null) { throw new ArgumentNullException(nameof(errors)); }

            _errors.AddRange(errors);
        }

        /// <inheritdoc />
        public void AssertHasNoError() {
            if (HasError) { throw BadRequestException.From(this); }
        }

        /// <inheritdoc />
        public ValidationError[] GetErrors() {
            return _errors.ToArray();
        }

        /// <inheritdoc />
        public override string ToString() {
            if (_errors.Count == 0) { return "No error recorded."; }
            if (_errors.Count == 1) { return "1 error has been recorded."; }

            return $"{_errors.Count} errors have been recorded.";
        }

        internal RequiredReferenceProperty<TProperty> ConvertRequired<TArgument, TProperty>(ReferenceArgument<TArgument> argument, Func<TArgument, TProperty> convert) {
            if (argument is null) { throw new ArgumentNullException(nameof(argument)); }
            if (convert is null) { throw new ArgumentNullException(nameof(convert)); }

            ReferenceArgument<TArgument> prefixedArgument = argument.AppendPrefix(ArgumentPrefix);

            if (prefixedArgument.IsMissing) {
                _errors.Add(ValidationError.ArgumentIsRequired(prefixedArgument));

                return RequiredReferenceProperty<TProperty>.CreateMissing(prefixedArgument);
            }

            try {
                TProperty                            propertyValue    = convert(argument.Value!);
                RequiredReferenceProperty<TProperty> requiredProperty = RequiredReferenceProperty<TProperty>.CreateValid(prefixedArgument, propertyValue);

                return requiredProperty;
            } catch (BadRequestException ex) {
                _errors.AddRange(ex.ValidationErrors);

                RequiredReferenceProperty<TProperty> convertRequired = RequiredReferenceProperty<TProperty>.CreateInvalid(prefixedArgument);

                return convertRequired;
            } catch (ApplicationException ex) {
                if (!Options.HandledExceptionType.IsInstanceOfType(ex)) { throw; }

                ValidationError error = new(prefixedArgument.Name, ex.Message);
                _errors.Add(error);
                RequiredReferenceProperty<TProperty> convertRequired = RequiredReferenceProperty<TProperty>.CreateInvalid(argument);

                return convertRequired;
            }
        }

        internal OptionalProperty<TProperty> ConvertOptional<TArgument, TProperty>(ReferenceArgument<TArgument> argument, Func<TArgument, TProperty> convert) {
            if (argument == null) { throw new ArgumentNullException(nameof(argument)); }
            if (convert  == null) { throw new ArgumentNullException(nameof(convert)); }

            ReferenceArgument<TArgument> prefixedArgument = argument.AppendPrefix(ArgumentPrefix);

            if (prefixedArgument.IsMissing) { return OptionalProperty<TProperty>.CreateMissing(prefixedArgument); }

            try {
                TProperty                   propertyValue   = convert(prefixedArgument.Value!);
                OptionalProperty<TProperty> convertRequired = OptionalProperty<TProperty>.CreateValid(prefixedArgument, propertyValue);

                return convertRequired;
            } catch (ApplicationException ex) {
                if (!Options.HandledExceptionType.IsInstanceOfType(ex)) { throw; }

                ValidationError error = new(prefixedArgument.Name, ex.Message);
                _errors.Add(error);
                OptionalProperty<TProperty> convertRequired = OptionalProperty<TProperty>.CreateInvalid(prefixedArgument);

                return convertRequired;
            }
        }

        internal RequiredReferenceProperty<TArgument> IsRequired<TArgument>(ReferenceArgument<TArgument> argument) {
            if (argument == null) { throw new ArgumentNullException(nameof(argument)); }

            ReferenceArgument<TArgument> prefixedArgument = argument.AppendPrefix(ArgumentPrefix);

            if (prefixedArgument.IsFulfilled) { return RequiredReferenceProperty<TArgument>.CreateValid(prefixedArgument, prefixedArgument.Value!); }

            ValidationError error = new(prefixedArgument.Name, "Argument is required.");
            _errors.Add(error);

            return RequiredReferenceProperty<TArgument>.CreateMissing(prefixedArgument);
        }

    }

}