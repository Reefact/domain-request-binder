namespace Reefact.FluentRequestBinder;

public sealed class PropertyException : FluentRequestBinderException {

    #region Statics members declarations

    public static PropertyException ValueIsInvalid() {
        return new PropertyException(ExceptionMessage.Property_ValueIsInvalid);
    }

    public static PropertyException ValueIsMissing() {
        return new PropertyException(ExceptionMessage.Property_ValueIsMissing);
    }

    #endregion

    #region Constructors declarations

    private PropertyException(string message) : base(message) { }

    #endregion

}