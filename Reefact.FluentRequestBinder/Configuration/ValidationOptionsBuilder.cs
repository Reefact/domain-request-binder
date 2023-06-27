#region Usings declarations

using System;
using System.Diagnostics.CodeAnalysis;

#endregion

namespace Reefact.FluentRequestBinder.Configuration {

    /// <summary>
    ///     Enables the construction of valid validation options.
    /// </summary>
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class ValidationOptionsBuilder {

        #region Fields declarations

        private ArgumentNameProvider _propertyNameProvider;
        private Type                 _handledExceptionType;

        #endregion

        #region Constructors declarations

        /// <summary>
        ///     Instantiate a new <see cref="ValidationOptionsBuilder">validation options builder</see>.
        /// </summary>
        public ValidationOptionsBuilder() {
            _handledExceptionType = ValidationOptions.Default.HandledExceptionType;
            _propertyNameProvider = ValidationOptions.Default.PropertyNameProvider;
        }

        #endregion

        /// <summary>
        ///     The <see cref="PropertyNameProvider">property name provider</see> to use during validation.
        /// </summary>
        public ArgumentNameProvider PropertyNameProvider {
            get => _propertyNameProvider;
            set {
                if (value == null) { throw new ArgumentNullException(nameof(value)); }

                _propertyNameProvider = value;
            }
        }

        /// <summary>
        ///     The type of domain exceptions matching a bad request argument to handle.
        /// </summary>
        public Type HandledExceptionType {
            get => _handledExceptionType;
            set {
                if (value == null) { throw new ArgumentNullException(nameof(value)); }

                _handledExceptionType = value;
            }
        }

        internal ValidationOptions Build() {
            return new ValidationOptions(PropertyNameProvider, HandledExceptionType);
        }

    }

}