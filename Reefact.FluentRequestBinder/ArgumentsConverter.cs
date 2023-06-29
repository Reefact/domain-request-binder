using System;
using System.Collections.Generic;

using Reefact.FluentRequestBinder.Configuration;

namespace Reefact.FluentRequestBinder {

    public sealed class ArgumentsConverter : Validator {

        #region Fields declarations

        private readonly Converter _argumentsValidator;

        #endregion

        #region Constructors declarations

        internal ArgumentsConverter(ValidationOptions validationOptions) {
            if (validationOptions is null) { throw new ArgumentNullException(nameof(validationOptions)); }

            _argumentsValidator = new Converter(validationOptions);
        }

        internal ArgumentsConverter(ValidationOptions validationOptions, string argNamePrefix) {
            if (validationOptions is null) { throw new ArgumentNullException(nameof(validationOptions)); }

            _argumentsValidator = new Converter(validationOptions, argNamePrefix);
        }

        #endregion

        /// <inheritdoc />
        public bool HasError => _argumentsValidator.HasError;
        /// <inheritdoc />
        public int ErrorCount => _argumentsValidator.ErrorCount;

        /// <summary>
        ///     Converts a field to a simple property.
        /// </summary>
        /// <typeparam name="TArgument">The type of the field.</typeparam>
        /// <param name="argumentName">The name of the argument.</param>
        /// <param name="argumentValue">The value of the argument.</param>
        /// <returns>An instance of <see cref="SimplePropertyConverter{TArgument}" />.</returns>
        public SimplePropertyConverter<TArgument> SimpleProperty<TArgument>(string argumentName, TArgument? argumentValue) {
            return new SimplePropertyConverter<TArgument>(_argumentsValidator, argumentName, argumentValue);
        }

        /// <summary>
        ///     Convert a field to a complex property.
        /// </summary>
        /// <typeparam name="TArgument">The type of the field.</typeparam>
        /// <param name="argumentValue">The value of the argument.</param>
        /// <returns>An instance of <see cref="ComplexPropertyConverter{TArgument}" />.</returns>
        public ComplexPropertyConverter<TArgument> ComplexProperty<TArgument>(TArgument? argumentValue) {
            return new ComplexPropertyConverter<TArgument>(_argumentsValidator, string.Empty, argumentValue);
        }

        /// <summary>
        ///     Convert a list of simple properties.
        /// </summary>
        /// <typeparam name="TArgument">The type of the field.</typeparam>
        /// <param name="argumentName">The name of the argument.</param>
        /// <param name="argumentValue">The value of the argument.</param>
        /// <returns>An instance of <see cref="ListOfComplexPropertiesConverter{TArgument}" />.</returns>
        public ListOfSimplePropertiesConverter<TArgument> ListOfSimpleProperties<TArgument>(string argumentName, IEnumerable<TArgument>? argumentValue) {
            return new ListOfSimplePropertiesConverter<TArgument>(_argumentsValidator, argumentName, argumentValue);
        }

        /// <inheritdoc />
        public void RecordError(ValidationError error) {
            _argumentsValidator.RecordError(error);
        }

        /// <inheritdoc />
        public void RecordErrors(IEnumerable<ValidationError> errors) {
            _argumentsValidator.RecordErrors(errors);
        }

        /// <inheritdoc />
        public void AssertHasNoError() {
            _argumentsValidator.AssertHasNoError();
        }

        /// <inheritdoc />
        public ValidationError[] GetErrors() {
            return _argumentsValidator.GetErrors();
        }

        /// <inheritdoc />
        public override string ToString() {
            return _argumentsValidator.ToString();
        }

    }

}