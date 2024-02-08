#region Usings declarations

using System.Diagnostics.CodeAnalysis;

#endregion

namespace Reefact.FluentRequestBinder {

    /// <summary>
    ///     The result of the binding of an optional argument.
    /// </summary>
    /// <typeparam name="TProperty">The property type.</typeparam>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public sealed class OptionalList<TProperty> {

        #region Statics members declarations

        internal static OptionalList<TProperty> CreateValid(Argument argument, List<TProperty> propertyValue) {
            if (argument == null) { throw new ArgumentNullException(nameof(argument)); }
            if (propertyValue is null) { throw new ArgumentNullException(nameof(propertyValue)); }

            return new OptionalList<TProperty>(argument, propertyValue, true, false);
        }

        internal static OptionalList<TProperty> CreateInvalid(Argument argument) {
            if (argument == null) { throw new ArgumentNullException(nameof(argument)); }

            return new OptionalList<TProperty>(argument, new List<TProperty>(), false, false);
        }

        internal static OptionalList<TProperty> CreateMissing(Argument argument) {
            if (argument == null) { throw new ArgumentNullException(nameof(argument)); }

            return new OptionalList<TProperty>(argument, new List<TProperty>(), true, true);
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

            return optionalProperty.Value;
        }

        #region Fields declarations

        private readonly List<TProperty> _value;

        #endregion

        #region Constructors declarations

        private OptionalList(Argument argument, List<TProperty> propertyValue, bool isValid, bool isMissing) {
            if (argument == null) { throw new ArgumentNullException(nameof(argument)); }

            Argument  = argument;
            _value    = propertyValue;
            IsValid   = isValid;
            IsMissing = isMissing;
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
        public List<TProperty> Value {
            get {
                if (!IsValid) { PropertyException.ValueIsInvalid(); }

                return _value;
            }
        }

        /// <summary>The name of the argument.</summary>
        public Argument Argument { get; }

        /// <inheritdoc />
        public override string ToString() {
            return (IsValid ? _value.ToString() : Argument.Value?.ToString()) ?? string.Empty;
        }

    }

}