#region Usings declarations

using System;

#endregion

namespace Reefact.FluentRequestBinder.Configuration {

    /// <summary>
    ///     The binding validation options.
    /// </summary>
    public sealed class ValidationOptions {

        #region Statics members declarations

        internal static readonly ValidationOptions Default = new(new DefaultArgumentNameProvider(), typeof(ApplicationException));

        /// <summary>
        ///     The instance of <see cref="ValidationOptions">options</see> used for validation.
        /// </summary>
        public static ValidationOptions Instance = Default;

        /// <summary>
        ///     Configures the instance of <see cref="ValidationOptions">options</see> that will be used for validation.
        /// </summary>
        /// <param name="validationOptions">The validation options.</param>
        /// <exception cref="ArgumentNullException">Parameter <paramref name="validationOptions" /> cannot be null.</exception>
        public static void Configure(ValidationOptionsBuilder validationOptions) {
            if (validationOptions is null) { throw new ArgumentNullException(nameof(validationOptions)); }

            Instance = validationOptions.Build();
        }

        #endregion

        #region Constructors declarations

        internal ValidationOptions(ArgumentNameProvider propertyNameProvider, Type handledExceptionType) {
            if (propertyNameProvider is null) { throw new ArgumentNullException(nameof(propertyNameProvider)); }
            if (handledExceptionType is null) { throw new ArgumentNullException(nameof(handledExceptionType)); }

            PropertyNameProvider = propertyNameProvider;
            HandledExceptionType = handledExceptionType;
        }

        #endregion

        /// <summary>
        ///     The <see cref="PropertyNameProvider">property name provider</see> to use during validation.
        /// </summary>
        public ArgumentNameProvider PropertyNameProvider { get; }

        /// <summary>
        ///     The type of domain exceptions matching a bad request argument to handle.
        /// </summary>
        public Type HandledExceptionType { get; }

    }

}