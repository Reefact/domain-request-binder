namespace Reefact.FluentRequestBinder {

    public sealed class RequiredPropertyException : FluentRequestBinderException {

        #region Statics members declarations

        public static RequiredPropertyException CouldNotBeValidIfArgumentValueIsNull(string argumentName) {
            return new RequiredPropertyException(ExceptionMessage.RequiredProperty_CouldNotBeValidIfArgumentValueIsNull);
        }

        public static RequiredPropertyException CouldNotBeValidIfPropertyValueIsNull(string argumentName) {
            return new RequiredPropertyException(ExceptionMessage.RequiredProperty_CouldNotBeValidIfPropertyValueIsNull);
        }

        #endregion

        #region Constructors declarations

        /// <inheritdoc />
        private RequiredPropertyException(string message) : base(message) { }

        #endregion

    }

}