#region Usings declarations

using System;

#endregion

namespace Reefact.RequestValidation {

    public static class Validate {

        #region Statics members declarations

        public static RequestValidator<TRequest> Request<TRequest>(TRequest request) {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }

            return new RequestValidator<ApplicationException, TRequest>(request);
        }

        public static ArgumentsValidator Arguments() {
            return new ArgumentsValidator<ApplicationException>();
        }

        #endregion

    }

}