#region Usings declarations

using System;

using Reefact.FluentRequestBinder.Configuration;

#endregion

namespace Reefact.FluentRequestBinder {

    public static class Validate {

        #region Statics members declarations

        public static RequestConverter<TRequest> Request<TRequest>(TRequest request) {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }

            return new RequestConverter<ApplicationException, TRequest>(request, ValidationOptions.Instance.PropertyNameProvider);
        }

        public static ArgumentsValidator Arguments() {
            return new ArgumentsValidator<ApplicationException>();
        }

        #endregion

    }

}