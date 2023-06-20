#region Usings declarations

using System;

#endregion

namespace Reefact.FluentRequestBinder.Configuration {

    public sealed class ValidationOptionsBuilder {

        #region Fields declarations

        private PropertyNameProvider _propertyNameProvider;
        private Type                 _handledExceptionType;

        #endregion

        #region Constructors declarations

        public ValidationOptionsBuilder() {
            _handledExceptionType = ValidationOptions.Default.HandledExceptionType;
            _propertyNameProvider = ValidationOptions.Default.PropertyNameProvider;
        }

        #endregion

        public PropertyNameProvider PropertyNameProvider {
            get => _propertyNameProvider;
            set {
                if (value == null) { throw new ArgumentNullException(nameof(value)); }

                _propertyNameProvider = value;
            }
        }

        public Type HandledExceptionType {
            get => _handledExceptionType;
            set {
                if (value == null) { throw new ArgumentNullException("value"); }

                _handledExceptionType = value;
            }
        }

        internal ValidationOptions Build() {
            return new ValidationOptions(PropertyNameProvider, HandledExceptionType);
        }

    }

}