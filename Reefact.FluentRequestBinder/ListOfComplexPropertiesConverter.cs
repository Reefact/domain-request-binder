#region Usings declarations

using System.Diagnostics.CodeAnalysis;

#endregion

namespace Reefact.FluentRequestBinder {

    /// <summary>Handle the conversion of an argument list to a property.</summary>
    /// <typeparam name="TArgument">The type of the input argument.</typeparam>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class ListOfComplexPropertiesConverter<TArgument> {

        #region Fields declarations

        private readonly Converter                        _argumentsValidator;
        private readonly Argument<IEnumerable<TArgument>> _argument;

        #endregion

        #region Constructors declarations

        internal ListOfComplexPropertiesConverter(Converter argumentsValidator, Argument<IEnumerable<TArgument>> argument) {
            if (argumentsValidator is null) { throw new ArgumentNullException(nameof(argumentsValidator)); }
            if (argument is null) { throw new ArgumentNullException(nameof(argument)); }

            _argumentsValidator = argumentsValidator;
            _argument           = argument;
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

            if (_argument.IsMissing) {
                _argumentsValidator.RecordError(ValidationError.ArgumentIsRequired(_argument));

                return RequiredList<TProperty>.CreateMissing(_argument);
            }

            List<TProperty> propertyValues = new();
            int             index          = 0;
            foreach (TArgument argumentValue in _argument.Value!) {
                try {
                    RequestConverter<TArgument> requestConverter = new(argumentValue, _argumentsValidator.Options, $"{_argument.Name}[{index}]");
                    TProperty                   propertyValue    = convert(requestConverter);
                    propertyValues.Add(propertyValue);
                } catch (BadRequestException ex) {
                    _argumentsValidator.RecordErrors(ex.ValidationErrors);
                }
                index++;
            }

            if (_argumentsValidator.HasError) { return RequiredList<TProperty>.CreateInvalid(_argument); }

            return RequiredList<TProperty>.CreateValid(_argument, propertyValues);
        }

        /// <summary>
        ///     Converts an argument to an optional complex property using a custom conversion method.
        /// </summary>
        /// <typeparam name="TProperty">The type of the output property.</typeparam>
        /// <param name="convert">The custom conversion method.</param>
        /// <returns>The <see cref="OptionalProperty{TArgument}">required argument</see> conversion result.</returns>
        public OptionalList<TProperty> AsOptional<TProperty>(Func<RequestConverter<TArgument>, TProperty> convert) {
            if (convert is null) { throw new ArgumentNullException(nameof(convert)); }

            if (_argument.IsMissing) { return OptionalList<TProperty>.CreateMissing(_argument); }

            List<TProperty> propertyValues = new();
            int             index          = 0;
            foreach (TArgument argumentValue in _argument.Value!) {
                try {
                    RequestConverter<TArgument> requestConverter = new(argumentValue, _argumentsValidator.Options, $"{_argument.Name}[{index}]");
                    TProperty                   propertyValue    = convert(requestConverter);
                    propertyValues.Add(propertyValue);
                } catch (BadRequestException ex) {
                    _argumentsValidator.RecordErrors(ex.ValidationErrors);
                }
                index++;
            }

            if (_argumentsValidator.HasError) { return OptionalList<TProperty>.CreateInvalid(_argument); }

            return OptionalList<TProperty>.CreateValid(_argument, propertyValues);
        }

    }

}