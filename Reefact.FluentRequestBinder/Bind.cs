#region Usings declarations

using System;
using System.Diagnostics.CodeAnalysis;

using Reefact.FluentRequestBinder.Configuration;

#endregion

namespace Reefact.FluentRequestBinder {

    /// <summary>
    ///     Represents the request binder.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class Bind {

        #region Statics members declarations

        /// <summary>
        ///     Provides a dedicated <see cref="RequestConverter{TRequest}">converter</see> for a
        ///     request based on a contract object.
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <param name="request"></param>
        /// <returns>A <see cref="RequestConverter{TRequest}">request converter.</see></returns>
        public static RequestConverter<TRequest> PropertiesOf<TRequest>(TRequest request) {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }

            return new RequestConverter<TRequest>(request, ValidationOptions.Instance);
        }

        /// <summary>Provides a dedicated <see cref="ArgumentsConverter">converter</see> for a request based on simple fields.</summary>
        /// <returns>A <see cref="ArgumentsConverter">arguments converter.</see></returns>
        public static ArgumentsConverter Arguments() {
            return new ArgumentsConverter(ValidationOptions.Instance);
        }

        #endregion

    }

}