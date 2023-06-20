#region Usings declarations

using System;

#endregion

namespace Reefact.FluentRequestBinder.Configuration {

    public sealed class ValidationOptions {

        #region Statics members declarations

        public static readonly ValidationOptions Instance = new ValidationOptions();

        #endregion

        #region Fields declarations

        private PropertyNameProvider _argNameProvider;

        #endregion

        #region Constructors declarations

        private ValidationOptions() {
            _argNameProvider = new DefaultPropertyNameProvider();
        }

        #endregion

        public PropertyNameProvider PropertyNameProvider {
            get { return _argNameProvider; }
            set {
                if (value == null) { throw new ArgumentNullException(nameof(value)); }

                _argNameProvider = value;
            }
        }

    }

}