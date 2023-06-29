#region Usings declarations

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Reefact.FluentRequestBinder.Configuration;

#endregion

namespace Reefact.FluentRequestBinder {

    /// <summary>
    ///     Converts simple arguments.
    /// </summary>
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

        /// <summary> Converts a required argument to a property. </summary>
        /// <typeparam name="TArgument">The type of the argument.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="argumentName">The name of the argument.</param>
        /// <param name="argumentValue">The value of the argument.</param>
        /// <param name="convert">The argument value to property value conversion method.</param>
        /// <returns>The <see cref="RequiredProperty{TProperty}">required property</see> conversion result.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Parameters <paramref name="argumentName" /> and <paramref name="convert" />
        ///     cannot be null.
        /// </exception>
        public RequiredProperty<TProperty> ConvertRequired<TArgument, TProperty>(string argumentName, TArgument? argumentValue, Func<TArgument, TProperty> convert) {
            if (argumentName == null) { throw new ArgumentNullException(nameof(argumentName)); }
            if (convert      == null) { throw new ArgumentNullException(nameof(convert)); }

            string argumentFullName = GetArgFullName(argumentName);

            if (argumentValue == null) {
                ValidationError error = new(argumentFullName, "Argument is required.");
                _errors.Add(error);

                return RequiredProperty<TProperty>.CreateMissing(argumentFullName);
            }

            try {
                TProperty                   propertyValue    = convert(argumentValue);
                RequiredProperty<TProperty> requiredProperty = RequiredProperty<TProperty>.CreateValid(argumentFullName, argumentValue, propertyValue);

                return requiredProperty;
            } catch (BadRequestException ex) {
                _errors.AddRange(ex.ValidationErrors);

                RequiredProperty<TProperty> convertRequired = RequiredProperty<TProperty>.CreateInvalid(argumentFullName, argumentValue);

                return convertRequired;
            } catch (ApplicationException ex) {
                if (!Options.HandledExceptionType.IsInstanceOfType(ex)) { throw; }

                ValidationError error = new(argumentFullName, ex.Message);
                _errors.Add(error);
                RequiredProperty<TProperty> convertRequired = RequiredProperty<TProperty>.CreateInvalid(argumentName, argumentValue);

                return convertRequired;
            }
        }

        /// <summary> Converts an optional argument to a property. </summary>
        /// <typeparam name="TArgument">The type of the argument.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="argumentName">The name of the argument.</param>
        /// <param name="argumentValue">The value of the argument.</param>
        /// <param name="convert">The argument value to property value conversion method.</param>
        /// <returns>The <see cref="OptionalProperty{TPropperty}">optional property</see> conversion result.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Parameters <paramref name="argumentName" /> and <paramref name="convert" />
        ///     cannot be null.
        /// </exception>
        public OptionalProperty<TProperty> ConvertOptional<TArgument, TProperty>(string argumentName, TArgument? argumentValue, Func<TArgument, TProperty> convert) {
            if (argumentName == null) { throw new ArgumentNullException(nameof(argumentName)); }
            if (convert      == null) { throw new ArgumentNullException(nameof(convert)); }

            string argFullName = GetArgFullName(argumentName);

            if (argumentValue == null) { return OptionalProperty<TProperty>.CreateMissing(argumentName); }

            try {
                TProperty                   propertyValue   = convert(argumentValue);
                OptionalProperty<TProperty> convertRequired = OptionalProperty<TProperty>.CreateValid(argFullName, argumentValue, propertyValue);

                return convertRequired;
            } catch (ApplicationException ex) {
                if (!Options.HandledExceptionType.IsInstanceOfType(ex)) { throw; }

                ValidationError error = new(argFullName, ex.Message);
                _errors.Add(error);
                OptionalProperty<TProperty> convertRequired = OptionalProperty<TProperty>.CreateInvalid(argFullName, argumentValue);

                return convertRequired;
            }
        }

        /// <summary>
        ///     Binds a argument to a required complex property using a custom conversion method.
        /// </summary>
        /// <typeparam name="TProperty">The type of the output property.</typeparam>
        /// <returns>The <see cref="RequiredProperty{TProperty}">required argument</see> conversions result.</returns>
        public RequiredProperty<TProperty> IsRequired<TProperty>(string argumentName, TProperty argumentValue) {
            if (argumentName == null) { throw new ArgumentNullException(nameof(argumentName)); }

            string argFullName = GetArgFullName(argumentName);

            if (argumentValue != null) { return RequiredProperty<TProperty>.CreateValid(argFullName, argumentValue, argumentValue); }

            ValidationError error = new(argFullName, "Argument is required.");
            _errors.Add(error);

            return RequiredProperty<TProperty>.CreateMissing(argFullName);
        }

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

        private string GetArgFullName(string argName) {
            if (ArgumentPrefix == null) { return argName; }

            return $"{ArgumentPrefix}.{argName}";
        }

    }

}