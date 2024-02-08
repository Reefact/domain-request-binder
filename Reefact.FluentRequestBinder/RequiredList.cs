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
    public sealed class RequiredList<TProperty> {

        #region Statics members declarations

        internal static RequiredList<TProperty> CreateValid(Argument argument, List<TProperty> propertyValue) {
            if (argument       == null) { throw new ArgumentNullException(nameof(argument)); }
            if (argument.Value == null) { throw RequiredPropertyException.CouldNotBeValidIfArgumentValueIsNull(argument.Name); }
            if (propertyValue  == null) { throw RequiredPropertyException.CouldNotBeValidIfPropertyValueIsNull(argument.Name); }

            return new RequiredList<TProperty>(argument, propertyValue, true, false);
        }

        internal static RequiredList<TProperty> CreateInvalid(Argument argument) {
            if (argument == null) { throw new ArgumentNullException(nameof(argument)); }

            return new RequiredList<TProperty>(argument, null, false, false);
        }

        internal static RequiredList<TProperty> CreateMissing(Argument argument) {
            if (argument == null) { throw new ArgumentNullException(nameof(argument)); }

            return new RequiredList<TProperty>(argument, null, false, true);
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
        public static implicit operator List<TProperty>(RequiredList<TProperty> requiredProperty) {
            if (requiredProperty == null) { throw new ArgumentNullException(nameof(requiredProperty)); }

            return requiredProperty.Value.ToList();
        }

        #region Fields declarations

        private readonly List<TProperty>? _value;

        #endregion

        #region Constructors declarations

        private RequiredList(Argument argument, List<TProperty>? propertyValue, bool isValid, bool isMissing) {
            Argument  = argument;
            _value    = propertyValue;
            IsValid   = isValid;
            IsMissing = isMissing;
        }

        #endregion

        /// <summary>
        ///     Indicates if the current instance of <see cref="RequiredList{TProperty}">required property</see> is valid (
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
        ///     If the current instance of <see cref="RequiredList{TProperty}">required property</see> is not valid.
        /// </exception>
        public IEnumerable<TProperty> Value {
            get {
                if (!IsValid) { throw PropertyException.ValueIsInvalid(); }
                if (IsMissing) { throw PropertyException.ValueIsMissing(); }

                return _value!;
            }
        }

        /// <summary>The underlying argument.</summary>
        public Argument Argument { get; }

        /// <inheritdoc />
        public override string ToString() {
            return (IsValid ? _value!.ToString() : Argument.Value?.ToString()) ?? string.Empty;
        }

    }

}