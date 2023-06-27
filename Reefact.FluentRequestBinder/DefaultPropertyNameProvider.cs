#region Usings declarations

using System;
using System.Reflection;

#endregion

namespace Reefact.FluentRequestBinder {

    /// <summary>
    ///     Represents the default behavior of <see cref="ArgumentNameProvider" />: the name of the argument correspond to the
    ///     name of the property in the DTO class.
    /// </summary>
    /// <remarks>The name does not match any serialization name provided as Newtonsoft `JsonPropertyNameAttribute`, or Microsoft `JsonPropertyAttribute`.</remarks>
    internal sealed class DefaultArgumentNameProvider : ArgumentNameProvider {

        /// <inheritdoc />
        public string GetArgumentNameFrom(PropertyInfo propertyInfo) {
            if (propertyInfo is null) { throw new ArgumentNullException(nameof(propertyInfo)); }

            return propertyInfo.Name;
        }

    }

}