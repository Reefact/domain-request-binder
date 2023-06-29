#region Usings declarations

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

#endregion

namespace Reefact.FluentRequestBinder {

    /// <summary>
    ///     The result of the binding of an optional argument.
    /// </summary>
    /// <typeparam name="TProperty">The property type.</typeparam>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public sealed class OptionalList<TProperty> {

        #region Statics members declarations

        internal static OptionalList<TProperty> CreateMissing(string argumentName) {
            if (argumentName == null) { throw new ArgumentNullException(nameof(argumentName)); }

            return new OptionalList<TProperty>(argumentName, null, new List<TProperty>(), true, true);
        }

        internal static OptionalList<TProperty> CreateValid(string argumentName, object argumentValue, IEnumerable<TProperty> propertyValue) {
            if (argumentName == null) { throw new ArgumentNullException(nameof(argumentName)); }
            if (argumentValue is null) { throw new ArgumentNullException(nameof(argumentValue)); }
            if (propertyValue is null) { throw new ArgumentNullException(nameof(propertyValue)); }

            return new OptionalList<TProperty>(argumentName, argumentValue, propertyValue, true, false);
        }

        internal static OptionalList<TProperty> CreateInvalid(string argumentName, object argumentValue) {
            if (argumentName == null) { throw new ArgumentNullException(nameof(argumentName)); }
            if (argumentValue is null) { throw new ArgumentNullException(nameof(argumentValue)); }

            return new OptionalList<TProperty>(argumentName, argumentValue, new List<TProperty>(), false, false);
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
        public static implicit operator List<TProperty>(OptionalList<TProperty> optionalProperty) {
            if (optionalProperty == null) { throw new ArgumentNullException(nameof(optionalProperty)); }

            if (optionalProperty.IsMissing) { return new List<TProperty>(); }

            return optionalProperty.Value!.ToList();
        }

        #region Fields declarations

        private readonly IEnumerable<TProperty> _value;

        #endregion

        #region Constructors declarations

        private OptionalList(string argumentName, object? argumentValue, IEnumerable<TProperty> propertyValue, bool isValid, bool isMissing) {
            if (argumentName == null) { throw new ArgumentNullException(nameof(argumentName)); }

            ArgumentName  = argumentName;
            ArgumentValue = argumentValue;
            IsValid       = isValid;
            IsMissing     = isMissing;
            _value        = propertyValue;
        }

        #endregion

        /// <summary>
        ///     Indicates if the current instance of <see cref="OptionalList{TProperty}">optional property</see> is valid (
        ///     <c>true</c>) or
        ///     not (<c>false</c>).
        /// </summary>
        public bool IsValid { get; }

        /// <summary>
        ///     Indicates if the current instance of <see cref="OptionalList{TProperty}">optional property</see> represents a
        ///     missing argument (<c>true</c>) or not (<c>false</c>).
        /// </summary>
        public bool IsMissing { get; }

        /// <summary>
        ///     The value of the property.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     If the current instance of <see cref="RequiredProperty{TProperty}">required property</see> is not valid.
        /// </exception>
        public IEnumerable<TProperty> Value {
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