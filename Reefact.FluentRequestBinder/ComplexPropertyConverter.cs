#region Usings declarations

using System.Diagnostics.CodeAnalysis;

#endregion

namespace Reefact.FluentRequestBinder {

    /// <summary>Handle the conversion of an argument to a complex property.</summary>
    /// <typeparam name="TArgument">The type of the input argument.</typeparam>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class ComplexPropertyConverter<TArgument> {

        #region Fields declarations

        private readonly Converter                    _argumentsValidator;
        private readonly ReferenceArgument<TArgument> _argument;

        #endregion

        #region Constructors declarations

        internal ComplexPropertyConverter(Converter argumentsValidator, ReferenceArgument<TArgument> argument) {
            if (argumentsValidator is null) { throw new ArgumentNullException(nameof(argumentsValidator)); }
            if (argument is null) { throw new ArgumentNullException(nameof(argument)); }

            _argumentsValidator = argumentsValidator;
            _argument           = argument.AppendPrefix(_argumentsValidator.ArgumentPrefix);
        }

        #endregion

        /// <summary>
        ///     Converts an argument to a required complex property using a custom conversion method.
        /// </summary>
        /// <typeparam name="TProperty">The type of the output property.</typeparam>
        /// <param name="convert">The custom conversion method.</param>
        /// <returns>The <see cref="RequiredReferenceProperty{TProperty}">required argument</see> conversion result.</returns>
        public RequiredReferenceProperty<TProperty> AsRequired<TProperty>(Func<RequestConverter<TArgument>, TProperty> convert) {
            if (convert is null) { throw new ArgumentNullException(nameof(convert)); }

            if (_argument.IsMissing) {
                _argumentsValidator.RecordError(new ValidationError(_argument.Name, "Argument is required."));

                return RequiredReferenceProperty<TProperty>.CreateMissing(_argument);
            }

            try {
                RequestConverter<TArgument> requestConverter = new(_argument.Value!, _argumentsValidator.Options, _argument.Name);
                TProperty                   propertyValue    = convert(requestConverter);

                return RequiredReferenceProperty<TProperty>.CreateValid(_argument, propertyValue);
            } catch (BadRequestException ex) {
                _argumentsValidator.RecordErrors(ex.ValidationErrors);

                return RequiredReferenceProperty<TProperty>.CreateInvalid(_argument);
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

            if (_argument.IsMissing) { return OptionalProperty<TProperty>.CreateMissing(_argument); }

            try {
                RequestConverter<TArgument> requestConverter = new(_argument.Value!, _argumentsValidator.Options, _argument.Name);
                TProperty                   propertyValue    = convert(requestConverter);

                return OptionalProperty<TProperty>.CreateValid(_argument, propertyValue);
            } catch (BadRequestException ex) {
                _argumentsValidator.RecordErrors(ex.ValidationErrors);

                return OptionalProperty<TProperty>.CreateInvalid(_argument);
            }
        }

    }

}