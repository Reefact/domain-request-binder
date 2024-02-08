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

        internal static RequiredProperty<TProperty> CreateValid(Argument argument, TProperty propertyValue) {
            if (argument       == null) { throw new ArgumentNullException(nameof(argument)); }
            if (argument.Value == null) { throw RequiredPropertyException.CouldNotBeValidIfArgumentValueIsNull(argument.Name); }
            if (propertyValue  == null) { throw RequiredPropertyException.CouldNotBeValidIfPropertyValueIsNull(argument.Name); }

            return new RequiredProperty<TProperty>(argument, propertyValue, true, false);
        }

        internal static RequiredProperty<TProperty> CreateInvalid(Argument argument) {
            if (argument is null) { throw new ArgumentNullException(nameof(argument)); }

            return new RequiredProperty<TProperty>(argument, default, false, false);
        }

        internal static RequiredProperty<TProperty> CreateMissing(Argument argument) {
            if (argument is null) { throw new ArgumentNullException(nameof(argument)); }

            return new RequiredProperty<TProperty>(argument, default, false, true);
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

        private RequiredProperty(Argument argument, TProperty? propertyValue, bool isValid, bool isMissing) {
            if (argument is null) { throw new ArgumentNullException(nameof(argument)); }

            Argument  = argument;
            _value    = propertyValue;
            IsValid   = isValid;
            IsMissing = isMissing;
        }

        #endregion

        /// <summary>
        ///     Indicates if the current instance of <see cref="RequiredProperty{TProperty}">required property</see> is
        ///     valid (
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
        public TProperty Value {
            get {
                if (!IsValid) { throw PropertyException.ValueIsInvalid(); }
                if (IsMissing) { throw PropertyException.ValueIsMissing(); }

                return _value!;
            }
        }

        /// <summary>The argument that has been provided.</summary>
        public Argument Argument { get; }

        /// <inheritdoc />
        public override string ToString() {
            return (IsValid ? _value?.ToString() : Argument.Value?.ToString()) ?? string.Empty;
        }

    }

}