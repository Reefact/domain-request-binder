#region Usings declarations

using System;
using System.Diagnostics.CodeAnalysis;

#endregion

namespace Reefact.FluentRequestBinder {

    /// <summary>Handle the conversion of an argument to a simple property.</summary>
    /// <typeparam name="TArgument">The type of the input argument.</typeparam>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class SimplePropertyConverter<TArgument> {

        #region Fields declarations

        private readonly ArgumentsConverter _argumentsValidator;
        private readonly string             _argumentName;
        private readonly TArgument?         _argumentValue;

        #endregion

        #region Constructors declarations

        internal SimplePropertyConverter(ArgumentsConverter argumentsValidator, string argumentName, TArgument? argumentValue) {
            if (argumentsValidator is null) { throw new ArgumentNullException(nameof(argumentsValidator)); }
            if (argumentName is null) { throw new ArgumentNullException(nameof(argumentName)); }

            _argumentsValidator = argumentsValidator;
            _argumentName       = argumentName;
            _argumentValue      = argumentValue;
        }

        #endregion

        /// <summary>
        ///     Converts an argument to a required simple property using a custom conversion method.
        /// </summary>
        /// <typeparam name="TProperty">The type of the output property.</typeparam>
        /// <param name="convert">The custom conversion method.</param>
        /// <returns>The <see cref="RequiredProperty{TArgument}">required property</see> conversion result.</returns>
        public RequiredProperty<TProperty> AsRequired<TProperty>(Func<TArgument, TProperty> convert) {
            return _argumentsValidator.ConvertRequired(_argumentName, _argumentValue, convert);
        }

        /// <summary>
        ///     Converts an argument to a simple required property without conversion - only the argument requirement is checked.
        /// </summary>
        /// <returns>The <see cref="RequiredProperty{TArgument}">required property</see> conversion result.</returns>
        public RequiredProperty<TArgument> AsRequired() {
            return _argumentsValidator.IsRequired(_argumentName, _argumentValue)!;
        }

        /// <summary>
        ///     Converts an argument to an optional simple property using a custom conversion method.
        /// </summary>
        /// <typeparam name="TProperty">The type of the output property.</typeparam>
        /// <param name="convert">The custom conversion method.</param>
        /// <returns>The <see cref="OptionalProperty{TArgument}">optional property</see> conversion result.</returns>
        public OptionalProperty<TProperty> AsOptional<TProperty>(Func<TArgument, TProperty> convert) {
            return _argumentsValidator.ConvertOptional(_argumentName, _argumentValue, convert);
        }

    }

}