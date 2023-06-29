using System.Diagnostics.CodeAnalysis;

namespace Reefact.FluentRequestBinder {

    /// <summary>
    ///     Represents the request binder.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public interface RequestBinder {

        /// <summary>
        ///     Provides a dedicated <see cref="RequestConverter{TRequest}">converter</see> for a
        ///     request based on a contract object.
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <param name="request"></param>
        /// <returns>A <see cref="RequestConverter{TRequest}">request converter.</see></returns>
        RequestConverter<TRequest> PropertiesOf<TRequest>(TRequest request);

        /// <summary>Provides a dedicated <see cref="Converter">converter</see> for a request based on simple fields.</summary>
        /// <returns>A <see cref="Converter">arguments converter.</see></returns>
        ArgumentsConverter Arguments();

    }

}