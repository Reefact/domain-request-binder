#region Usings declarations

using System;
using System.Diagnostics.CodeAnalysis;

#endregion

namespace Reefact.FluentRequestBinder {

    /// <summary>
    ///     The result of the binding of an optional argument.
    /// </summary>
    /// <typeparam name="TProperty">The property type.</typeparam>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public sealed class OptionalProperty<TProperty> {

        #region Statics members declarations

        internal static OptionalProperty<TProperty> CreateMissing(string argumentName, TProperty empty) {
            if (argumentName == null) { throw new ArgumentNullException(nameof(argumentName)); }

            return new OptionalProperty<TProperty>(argumentName, null, empty, true, true);
        }

        internal static OptionalProperty<TProperty> CreateMissing(string argumentName) {
            if (argumentName == null) { throw new ArgumentNullException(nameof(argumentName)); }

            return new OptionalProperty<TProperty>(argumentName, null, default, true, true);
        }

        internal static OptionalProperty<TProperty> CreateValid(string argumentName, object argumentValue, TProperty propertyValue) {
            if (argumentName == null) { throw new ArgumentNullException(nameof(argumentName)); }
            if (argumentValue is null) { throw new ArgumentNullException(nameof(argumentValue)); }
            if (propertyValue is null) { throw new ArgumentNullException(nameof(propertyValue)); }

            return new OptionalProperty<TProperty>(argumentName, argumentValue, propertyValue, true, false);
        }

        internal static OptionalProperty<TProperty> CreateInvalid(string argumentName, object argumentValue) {
            if (argumentName == null) { throw new ArgumentNullException(nameof(argumentName)); }
            if (argumentValue is null) { throw new ArgumentNullException(nameof(argumentValue)); }

            return new OptionalProperty<TProperty>(argumentName, argumentValue, default, false, false);
        }

        #endregion

        /// <summary>Implicit cast.</summary>
        /// <param name="optionalProperty">The optional property to cast.</param>
        /// <returns>The value of the property.</returns>
        /// <exception cref="ArgumentNullException">Parameter <paramref name="optionalProperty" /> cannot be <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">
        ///     If <paramref name="optionalProperty">the optional property</paramref> is
        ///     not valid.
        /// </exception>
        public static implicit operator TProperty?(OptionalProperty<TProperty> optionalProperty) {
            if (optionalProperty == null) { throw new ArgumentNullException(nameof(optionalProperty)); }

            if (!optionalProperty.IsValid) { throw new InvalidOperationException($"Value '{optionalProperty.ArgumentValue}' of argument '{optionalProperty.ArgumentName}' is not valid."); }

            return optionalProperty.Value;
        }

        #region Fields declarations

        private readonly TProperty? _value;

        #endregion

        #region Constructors declarations

        private OptionalProperty(string argumentName, object? argumentValue, TProperty? propertyValue, bool isValid, bool isMissing) {
            if (argumentName == null) { throw new ArgumentNullException(nameof(argumentName)); }

            ArgumentName  = argumentName;
            ArgumentValue = argumentValue;
            IsValid       = isValid;
            IsMissing     = isMissing;
            _value        = propertyValue;
        }

        #endregion

        /// <summary>
        ///     Indicates if the current instance of <see cref="OptionalProperty{TProperty}">optional property</see> is valid (
        ///     <c>true</c>) or
        ///     not (<c>false</c>).
        /// </summary>
        public bool IsValid { get; }

        /// <summary>
        ///     Indicates if the current instance of <see cref="OptionalProperty{TProperty}">optional property</see> represents a
        ///     missing argument (<c>true</c>) or not (<c>false</c>).
        /// </summary>
        public bool IsMissing { get; }

        /// <summary>
        ///     The value of the property.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     If the current instance of <see cref="RequiredProperty{TProperty}">required property</see> is not valid.
        /// </exception>
        public TProperty? Value {
            get {
                if (!IsValid) { throw new InvalidOperationException("Property is not valid."); }

                return _value;
            }
        }

        /// <summary>The name of the argument.</summary>
        public string ArgumentName { get; }

        /// <summary>The value of the argument.</summary>
        public object? ArgumentValue { get; }

        /// <inheritdoc />
        public override string ToString() {
            return (IsValid ? _value?.ToString() : ArgumentValue?.ToString()) ?? string.Empty;
        }

    }

}