#region Usings declarations

using System.Diagnostics.CodeAnalysis;

#endregion

namespace Reefact.FluentRequestBinder {

    /// <summary>Handle the conversion of an argument list to a property.</summary>
    /// <typeparam name="TArgument">The type of the input argument.</typeparam>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class ListOfSimplePropertiesConverter<TArgument> {

        #region Fields declarations

        private readonly Converter                        _argumentsValidator;
        private readonly Argument<IEnumerable<TArgument>> _argument;

        #endregion

        #region Constructors declarations

        internal ListOfSimplePropertiesConverter(Converter argumentsValidator, Argument<IEnumerable<TArgument>> argument) {
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
        /// <returns>The <see cref="RequiredList{TProperty}">required argument</see> conversion result.</returns>
        public RequiredList<TProperty> AsRequired<TProperty>(Func<TArgument, TProperty> convert) {
            if (convert is null) { throw new ArgumentNullException(nameof(convert)); }

            if (_argument.IsMissing) {
                _argumentsValidator.RecordError(ValidationError.ArgumentIsRequired(_argument));

                return RequiredList<TProperty>.CreateMissing(_argument);
            }

            List<TProperty> propertyValues = new();
            int             index          = 0;
            foreach (TArgument argumentValue in _argument.Value!) {
                string                      argumentName     = $"{_argument.Name}[{index}]";
                Argument<TArgument>         argument         = new(argumentName, argumentValue);
                RequiredProperty<TProperty> requiredProperty = _argumentsValidator.ConvertRequired(argument, convert);
                if (requiredProperty.IsValid) {
                    propertyValues.Add(requiredProperty);
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
        /// <returns>The <see cref="OptionalList{TProperty}">required argument</see> conversion result.</returns>
        public OptionalList<TProperty> AsOptional<TProperty>(Func<TArgument, TProperty> convert) {
            if (convert is null) { throw new ArgumentNullException(nameof(convert)); }

            if (_argument.IsMissing) { return OptionalList<TProperty>.CreateMissing(_argument); }

            List<TProperty> propertyValues = new();
            int             index          = 0;
            foreach (TArgument argumentValue in _argument.Value!) {
                string                      argumentName     = $"{_argument.Name}[{index}]";
                Argument<TArgument>         argument         = new(argumentName, argumentValue);
                OptionalProperty<TProperty> optionalProperty = _argumentsValidator.ConvertOptional(argument, convert);
                if (optionalProperty is { IsValid: true, IsMissing: false }) {
                    propertyValues.Add(optionalProperty.Value!);
                }
                index++;
            }

            if (_argumentsValidator.HasError) {
                return OptionalList<TProperty>.CreateInvalid(_argument);
            }

            return OptionalList<TProperty>.CreateValid(_argument, propertyValues);
        }

    }

}