#region Usings declarations

using System.Reflection;

#endregion

namespace Reefact.FluentRequestBinder {

    /// <summary>
    ///     Represents a service that provide the name of the argument as declared in the contract.
    /// </summary>
    public interface ArgumentNameProvider {

        /// <summary>
        ///     Retrieves the name of the argument.
        /// </summary>
        /// <param name="propertyInfo">The <see cref="PropertyInfo" /> representing the argument.</param>
        /// <returns>The name of the argument.</returns>
        string GetArgumentNameFrom(PropertyInfo propertyInfo);

    }

}