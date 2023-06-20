#region Usings declarations

using System;

#endregion

namespace Reefact.FluentRequestBinder.Configuration {

    public sealed class ValidationOptions {

        #region Statics members declarations

        internal static ValidationOptions Default = new ValidationOptions(new DefaultPropertyNameProvider(), typeof(ApplicationException));

        public static ValidationOptions Instance = Default;

        public static void Configure(ValidationOptionsBuilder validationOptions) {
            if (validationOptions is null) { throw new ArgumentNullException(nameof(validationOptions)); }

            Instance = validationOptions.Build();
        }
        
        #endregion

        #region Constructors declarations

        internal ValidationOptions(PropertyNameProvider propertyNameProvider, Type handledExceptionType) {
            if (propertyNameProvider is null) { throw new ArgumentNullException(nameof(propertyNameProvider)); }
            if (handledExceptionType is null) { throw new ArgumentNullException(nameof(handledExceptionType)); }

            PropertyNameProvider = propertyNameProvider;
            HandledExceptionType = handledExceptionType;
        }

        #endregion

        public PropertyNameProvider PropertyNameProvider { get; }
        public Type                 HandledExceptionType { get; }

    }

}