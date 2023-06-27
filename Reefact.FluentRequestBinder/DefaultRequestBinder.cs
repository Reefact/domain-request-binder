#region Usings declarations

using System;
using System.Diagnostics.CodeAnalysis;

using Reefact.FluentRequestBinder.Configuration;

#endregion

namespace Reefact.FluentRequestBinder {

    /// <summary>Encapsulates the default behavior of <see cref="RequestBinder" />.</summary>
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public sealed class DefaultRequestBinder : RequestBinder {

        #region Fields declarations

        private readonly ValidationOptions _options;

        #endregion

        #region Constructors declarations

        /// <summary>
        ///     Instantiate a new instance of <see cref="DefaultRequestBinder" />.
        /// </summary>
        /// <param name="options">The validation options.</param>
        /// <exception cref="ArgumentNullException">Parameter <paramref name="options" /> cannot be null.</exception>
        public DefaultRequestBinder(ValidationOptions options) {
            if (options is null) { throw new ArgumentNullException(nameof(options)); }

            _options = options;
        }

        #endregion

        /// <inheritdoc />
        public RequestConverter<TRequest> PropertiesOf<TRequest>(TRequest request) {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }

            return new RequestConverter<TRequest>(request, _options);
        }

        /// <inheritdoc />
        public ArgumentsConverter Arguments() {
            return new ArgumentsConverter(_options);
        }

    }

}