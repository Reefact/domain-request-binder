#region Usings declarations

using System.Diagnostics.CodeAnalysis;

#endregion

namespace Reefact.FluentRequestBinder {

    /// <summary>Handle the conversion of an argument to a simple property.</summary>
    /// <typeparam name="TArgument">The type of the input argument.</typeparam>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class SimplePropertyConverter<TArgument> {

        #region Fields declarations

        private readonly Converter           _argumentsValidator;
        private readonly Argument<TArgument> _argument;

        #endregion

        #region Constructors declarations

        internal SimplePropertyConverter(Converter argumentsValidator, Argument<TArgument> argument) {
            if (argumentsValidator is null) { throw new ArgumentNullException(nameof(argumentsValidator)); }
            if (argument is null) { throw new ArgumentNullException(nameof(argument)); }

            _argumentsValidator = argumentsValidator;
            _argument           = argument;
        }

        #endregion

        /// <summary>
        ///     Converts an argument to a required simple property using a custom conversion method.
        /// </summary>
        /// <typeparam name="TProperty">The type of the output property.</typeparam>
        /// <param name="convert">The custom conversion method.</param>
        /// <returns>The <see cref="RequiredProperty{TProperty}">required property</see> conversion result.</returns>
        public RequiredProperty<TProperty> AsRequired<TProperty>(Func<TArgument, TProperty> convert) {
            if (convert is null) { throw new ArgumentNullException(nameof(convert)); }

            return _argumentsValidator.ConvertRequired(_argument, convert);
        }

        /// <summary>
        ///     Converts an argument to a simple required property without conversion - only the argument requirement is checked.
        /// </summary>
        /// <returns>The <see cref="RequiredProperty{TProperty}">required property</see> conversion result.</returns>
        public RequiredProperty<TArgument> AsRequired() {
            return _argumentsValidator.IsRequired(_argument);
        }

        /// <summary>
        ///     Converts an argument to an optional simple property using a custom conversion method.
        /// </summary>
        /// <typeparam name="TProperty">The type of the output property.</typeparam>
        /// <param name="convert">The custom conversion method.</param>
        /// <returns>The <see cref="OptionalProperty{TArgument}">optional property</see> conversion result.</returns>
        public OptionalProperty<TProperty> AsOptional<TProperty>(Func<TArgument, TProperty> convert) {
            if (convert is null) { throw new ArgumentNullException(nameof(convert)); }

            return _argumentsValidator.ConvertOptional(_argument, convert);
        }

        public OptionalProperty<TArgument> AsOptional() {
            return _argumentsValidator.ConvertOptional(_argument, x => x);
        }

    }

}