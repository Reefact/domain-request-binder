namespace Reefact.FluentRequestBinder {

    public sealed class RequiredPropertyException : FluentRequestBinderException {

        #region Statics members declarations

        public static RequiredPropertyException CouldNotBeValidIfArgumentValueIsNull(string argumentName) {
            return new RequiredPropertyException("A required property could not be valid if argument value is null.");
        }

        public static RequiredPropertyException CouldNotBeValidIfPropertyValueIsNull(string argumentName) {
            return new RequiredPropertyException("A required property could not be valid if property value is null.");
        }

        #endregion

        #region Constructors declarations

        /// <inheritdoc />
        private RequiredPropertyException(string message) : base(message) { }

        #endregion

    }

}