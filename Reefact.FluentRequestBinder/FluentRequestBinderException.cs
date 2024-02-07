namespace Reefact.FluentRequestBinder {

    /// <summary>
    ///     Represents errors that occur during Fluent Request Binder operations.
    /// </summary>
    /// <remarks>
    ///     This class is a specific type of ApplicationException that can be thrown when the Fluent Request Binder encounters
    ///     an error.
    /// </remarks>
    public class FluentRequestBinderException(string message) : ApplicationException(message) { }

}