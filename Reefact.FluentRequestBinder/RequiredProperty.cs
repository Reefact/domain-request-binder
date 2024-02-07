#region Usings declarations

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

#endregion

namespace Reefact.FluentRequestBinder {

    /// <summary>
    ///     The result of the binding of a required argument.
    /// </summary>
    /// <typeparam name="TProperty">The property type.</typeparam>
    [DebuggerDisplay("{ToString()}")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public sealed class RequiredProperty<TProperty> {

        #region Statics members declarations

        [Obsolete("Use override with `Argument` parameter instead.")]
        internal static RequiredProperty<TProperty> CreateValid(string argumentName, object argumentValue, TProperty propertyValue) {
            if (argumentName  == null) { throw new ArgumentNullException(nameof(argumentName)); }
            if (argumentValue == null) { throw RequiredPropertyException.CouldNotBeValidIfArgumentValueIsNull(argumentName); }
            if (propertyValue == null) { throw RequiredPropertyException.CouldNotBeValidIfPropertyValueIsNull(argumentName); }

            return new RequiredProperty<TProperty>(argumentName, argumentValue, propertyValue, true);
        }

        internal static RequiredProperty<TProperty> CreateValid(Argument argument, TProperty propertyValue) {
            if (argument       == null) { throw new ArgumentNullException(nameof(argument)); }
            if (argument.Value == null) { throw RequiredPropertyException.CouldNotBeValidIfArgumentValueIsNull(argument.Name); }
            if (propertyValue  == null) { throw RequiredPropertyException.CouldNotBeValidIfPropertyValueIsNull(argument.Name); }

            return new RequiredProperty<TProperty>(argument, propertyValue, true);
        }

        [Obsolete("Use override with `Argument` parameter instead.")]
        internal static RequiredProperty<TProperty> CreateInvalid(string argumentName, object argumentValue) {
            if (argumentName is null) { throw new ArgumentNullException(nameof(argumentName)); }
            if (argumentValue is null) { throw new ArgumentNullException(nameof(argumentValue)); }

            return new RequiredProperty<TProperty>(argumentName, argumentValue, default, false);
        }

        internal static RequiredProperty<TProperty> CreateInvalid(Argument argument) {
            if (argument is null) { throw new ArgumentNullException(nameof(argument)); }

            return new RequiredProperty<TProperty>(argument, default, false);
        }

        [Obsolete("Use override with `Argument` parameter instead.")]
        internal static RequiredProperty<TProperty> CreateMissing(string argumentName) {
            if (argumentName is null) { throw new ArgumentNullException(nameof(argumentName)); }

            return new RequiredProperty<TProperty>(argumentName, null, default, false);
        }

        internal static RequiredProperty<TProperty> CreateMissing(Argument argument) {
            if (argument is null) { throw new ArgumentNullException(nameof(argument)); }

            return new RequiredProperty<TProperty>(argument.Name, null, default, false);
        }

        #endregion

        /// <summary>
        ///     Implicit cast.
        /// </summary>
        /// <param name="requiredProperty">The required property to cast.</param>
        /// <returns>The value of the property.</returns>
        /// <exception cref="ArgumentNullException">Parameter <paramref name="requiredProperty" /> cannot be <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">
        ///     If <paramref name="requiredProperty">the required property</paramref> is
        ///     not valid.
        /// </exception>
        public static implicit operator TProperty(RequiredProperty<TProperty> requiredProperty) {
            if (requiredProperty == null) { throw new ArgumentNullException(nameof(requiredProperty)); }

            return requiredProperty.Value!;
        }

        #region Fields declarations

        private readonly TProperty? _value;

        #endregion

        #region Constructors declarations

        private RequiredProperty(Argument argument, TProperty? propertyValue, bool isValid) {
            if (argument is null) { throw new ArgumentNullException(nameof(argument)); }

            ArgumentName  = argument.Name;
            ArgumentValue = argument.Value;
            _value        = propertyValue;
            IsValid       = isValid;
        }

        private RequiredProperty(string argumentName, object? argumentValue, TProperty? propertyValue, bool isValid) {
            if (argumentName is null) { throw new ArgumentNullException(nameof(argumentName)); }

            ArgumentName  = argumentName;
            ArgumentValue = argumentValue;
            _value        = propertyValue;
            IsValid       = isValid;
        }

        #endregion

        /// <summary>
        ///     Indicates if the current instance of <see cref="RequiredProperty{TProperty}">required property</see> is valid (
        ///     <c>true</c>) or
        ///     not (<c>false</c>).
        /// </summary>
        public bool IsValid { get; }

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
        // [Obsolete("Use property `Argument` instead.")]
        public string ArgumentName { get; }

        /// <summary>
        ///     The value of the argument.
        /// </summary>
        // [Obsolete("Use property `Argument` instead.")]
        public object? ArgumentValue { get; }

        // TODO: Replace the two properties above [Obsolete] with an `Argument` property.
        // public Argument Argument { get; }

        /// <inheritdoc />
        public override string ToString() {
            return (IsValid ? _value?.ToString() : ArgumentValue?.ToString()) ?? string.Empty;
        }

    }

}