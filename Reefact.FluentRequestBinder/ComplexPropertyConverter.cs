#region Usings declarations

using System;
using System.Diagnostics.CodeAnalysis;

#endregion

namespace Reefact.FluentRequestBinder {

    /// <summary>Handle the conversion of an argument to a complex property.</summary>
    /// <typeparam name="TArgument">The type of the input argument.</typeparam>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class ComplexPropertyConverter<TArgument> {

        #region Fields declarations

        private readonly ArgumentsConverter _argumentsValidator;
        private readonly TArgument?         _argumentValue;
        private readonly string             _argumentName;

        #endregion

        #region Constructors declarations

        internal ComplexPropertyConverter(ArgumentsConverter argumentsValidator, string argumentName, TArgument? argumentValue) {
            if (argumentsValidator is null) { throw new ArgumentNullException(nameof(argumentsValidator)); }
            if (argumentName is null) { throw new ArgumentNullException(nameof(argumentName)); }

            _argumentsValidator = argumentsValidator;
            _argumentName       = GetArgFullName(argumentName);
            _argumentValue      = argumentValue;
        }

        #endregion

        /// <summary>
        ///     Converts an argument to a required complex property using a custom conversion method.
        /// </summary>
        /// <typeparam name="TProperty">The type of the output property.</typeparam>
        /// <param name="convert">The custom conversion method.</param>
        /// <returns>The <see cref="RequiredProperty{TProperty}">required argument</see> conversion result.</returns>
        public RequiredProperty<TProperty> AsRequired<TProperty>(Func<RequestConverter<TArgument>, TProperty> convert) {
            if (convert is null) { throw new ArgumentNullException(nameof(convert)); }

            if (_argumentValue is null) {
                _argumentsValidator.RecordError(new ValidationError(_argumentName, "Argument is required."));

                return RequiredProperty<TProperty>.CreateMissing(_argumentName);
            }

            try {
                RequestConverter<TArgument> requestConverter = new(_argumentValue, _argumentsValidator.Options, _argumentName);
                TProperty                   propertyValue    = convert(requestConverter);

                return RequiredProperty<TProperty>.CreateValid(_argumentName, _argumentValue, propertyValue);
            } catch (BadRequestException ex) {
                _argumentsValidator.RecordErrors(ex.ValidationErrors);

                return RequiredProperty<TProperty>.CreateInvalid(_argumentName, _argumentValue);
            }
        }

        /// <summary>
        ///     Converts an argument to an optional complex property using a custom conversion method.
        /// </summary>
        /// <typeparam name="TProperty">The type of the output property.</typeparam>
        /// <param name="convert">The custom conversion method.</param>
        /// <returns>The <see cref="OptionalProperty{TArgument}">required argument</see> conversion result.</returns>
        public OptionalProperty<TProperty> AsOptional<TProperty>(Func<RequestConverter<TArgument>, TProperty> convert) {
            if (convert is null) { throw new ArgumentNullException(nameof(convert)); }

            if (_argumentValue == null) { return OptionalProperty<TProperty>.CreateMissing(_argumentName); }

            try {
                RequestConverter<TArgument> requestConverter = new(_argumentValue, _argumentsValidator.Options, _argumentName);
                TProperty                   propertyValue    = convert(requestConverter);

                return OptionalProperty<TProperty>.CreateValid(_argumentName, _argumentValue, propertyValue);
            } catch (BadRequestException ex) {
                _argumentsValidator.RecordErrors(ex.ValidationErrors);

                return OptionalProperty<TProperty>.CreateInvalid(_argumentName, _argumentValue);
            }
        }

        private string GetArgFullName(string argName)
        {
            if (_argumentsValidator.ArgumentPrefix == null) { return argName; }

            return $"{_argumentsValidator.ArgumentPrefix}.{argName}";
        }
    }

}