using System;
using System.Collections.Generic;

namespace Reefact.FluentRequestBinder {

    /// <summary>Handle the conversion of an argument list to a property.</summary>
    /// <typeparam name="TArgument">The type of the input argument.</typeparam>
    public sealed class ListOfComplexPropertiesConverter<TArgument> {

        #region Fields declarations

        private readonly ArgumentsConverter      _argumentsValidator;
        private readonly string                  _argumentName;
        private readonly IEnumerable<TArgument>? _argumentValues;

        #endregion

        #region Constructors declarations

        internal ListOfComplexPropertiesConverter(ArgumentsConverter argumentsValidator, string argumentName, IEnumerable<TArgument>? argumentValues) {
            if (argumentsValidator is null) { throw new ArgumentNullException(nameof(argumentsValidator)); }
            if (argumentName is null) { throw new ArgumentNullException(nameof(argumentName)); }

            _argumentsValidator = argumentsValidator;
            _argumentName       = argumentName;
            _argumentValues     = argumentValues;
        }

        #endregion

        /// <summary>
        ///     Converts an argument to a required list using a custom conversion method.
        /// </summary>
        /// <typeparam name="TProperty">The type of the output property.</typeparam>
        /// <param name="convert">The custom conversion method.</param>
        /// <returns>The <see cref="RequiredProperty{TProperty}">required argument</see> conversion result.</returns>
        public RequiredList<TProperty> AsRequired<TProperty>(Func<RequestConverter<TArgument>, TProperty> convert) {
            if (convert is null) { throw new ArgumentNullException(nameof(convert)); }

            if (_argumentValues is null) {
                _argumentsValidator.RecordError(new ValidationError(_argumentName, "Argument is required."));

                return RequiredList<TProperty>.CreateMissing(_argumentName);
            }

            List<TProperty> propertyValues = new();
            int             index          = 0;
            foreach (TArgument argumentValue in _argumentValues) {
                try {
                    RequestConverter<TArgument> requestConverter = new(argumentValue, _argumentsValidator.Options, $"{_argumentName}[{index}]");
                    TProperty                   propertyValue    = convert(requestConverter);
                    propertyValues.Add(propertyValue);
                } catch (BadRequestException ex) {
                    _argumentsValidator.RecordErrors(ex.ValidationErrors);
                }
                index++;
            }

            if (_argumentsValidator.HasError) {
                return RequiredList<TProperty>.CreateInvalid(_argumentName, _argumentValues);
            }

            return RequiredList<TProperty>.CreateValid(_argumentName, _argumentValues, propertyValues);
        }

        /// <summary>
        ///     Converts an argument to an optional complex property using a custom conversion method.
        /// </summary>
        /// <typeparam name="TProperty">The type of the output property.</typeparam>
        /// <param name="convert">The custom conversion method.</param>
        /// <returns>The <see cref="OptionalProperty{TArgument}">required argument</see> conversion result.</returns>
        public OptionalList<TProperty> AsOptional<TProperty>(Func<RequestConverter<TArgument>, TProperty> convert) {
            if (convert is null) { throw new ArgumentNullException(nameof(convert)); }

            if (_argumentValues is null) { return OptionalList<TProperty>.CreateMissing(_argumentName); }

            List<TProperty> propertyValues = new();
            int             index          = 0;
            foreach (TArgument argumentValue in _argumentValues) {
                try {
                    RequestConverter<TArgument> requestConverter = new(argumentValue, _argumentsValidator.Options, $"{_argumentName}[{index}]");
                    TProperty                   propertyValue    = convert(requestConverter);
                    propertyValues.Add(propertyValue);
                } catch (BadRequestException ex) {
                    _argumentsValidator.RecordErrors(ex.ValidationErrors);
                }
                index++;
            }

            if (_argumentsValidator.HasError) {
                return OptionalList<TProperty>.CreateInvalid(_argumentName, _argumentValues);
            }

            return OptionalList<TProperty>.CreateValid(_argumentName, _argumentValues, propertyValues);
        }

    }

}