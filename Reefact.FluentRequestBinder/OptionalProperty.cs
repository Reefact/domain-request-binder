#region Usings declarations

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

        internal static OptionalProperty<TProperty> CreateMissing<TArgument>(ReferenceArgument<TArgument> argument) {
            if (argument == null) { throw new ArgumentNullException(nameof(argument)); }

            return new OptionalProperty<TProperty>(argument, default, true, true);
        }

        internal static OptionalProperty<TProperty> CreateValid(Argument argument, TProperty propertyValue) {
            if (argument == null) { throw new ArgumentNullException(nameof(argument)); }
            if (propertyValue is null) { throw new ArgumentNullException(nameof(propertyValue)); }

            return new OptionalProperty<TProperty>(argument, propertyValue, true, false);
        }

        internal static OptionalProperty<TProperty> CreateInvalid(Argument argument) {
            if (argument == null) { throw new ArgumentNullException(nameof(argument)); }

            return new OptionalProperty<TProperty>(argument, default, false, false);
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

            if (!optionalProperty.IsValid) { throw new InvalidOperationException($"Value '{optionalProperty.Argument.Value}' of argument '{optionalProperty.Argument.Name}' is not valid."); }

            return optionalProperty.Value;
        }

        #region Fields declarations

        private readonly TProperty? _value;

        #endregion

        #region Constructors declarations

        private OptionalProperty(Argument argument, TProperty? propertyValue, bool isValid, bool isMissing) {
            if (argument == null) { throw new ArgumentNullException(nameof(argument)); }

            Argument  = argument;
            IsValid   = isValid;
            IsMissing = isMissing;
            _value    = propertyValue;
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
        ///     If the current instance of <see cref="RequiredReferenceProperty{TProperty}">required property</see> is not valid.
        /// </exception>
        public TProperty? Value {
            get {
                if (!IsValid) { throw new InvalidOperationException("Property is not valid."); }

                return _value;
            }
        }

        /// <summary>The underlying argument.</summary>
        public Argument Argument { get; }

        /// <inheritdoc />
        public override string ToString() {
            return (IsValid ? _value?.ToString() : Argument.Value?.ToString()) ?? string.Empty;
        }

    }

}