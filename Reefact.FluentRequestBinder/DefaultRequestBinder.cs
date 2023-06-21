#region Usings declarations

using System;

using Reefact.FluentRequestBinder.Configuration;

#endregion

namespace Reefact.FluentRequestBinder {

    public sealed class DefaultRequestBinder : RequestBinder {

        #region Fields declarations

        private readonly ValidationOptions _options;

        #endregion

        #region Constructors declarations

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