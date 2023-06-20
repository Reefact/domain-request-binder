#region Usings declarations

using System;

using Reefact.FluentRequestBinder.Configuration;

#endregion

namespace Reefact.FluentRequestBinder {

    public static class Bind {

        #region Statics members declarations

        public static RequestConverter<TRequest> PropertiesOf<TRequest>(TRequest request) {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }

            return new RequestConverter<TRequest>(request, ValidationOptions.Instance);
        }
    
        public static ArgumentsConverter Arguments() {
            return new ArgumentsConverter(ValidationOptions.Instance);
        }
     

        #endregion

    }

}