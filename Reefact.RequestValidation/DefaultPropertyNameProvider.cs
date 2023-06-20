#region Usings declarations

using System;
using System.Reflection;

#endregion

namespace Reefact.RequestValidation {

    internal sealed class DefaultPropertyNameProvider : PropertyNameProvider {

        /// <inheritdoc />
        public string GetName(PropertyInfo propertyInfo) {
            if (propertyInfo is null) { throw new ArgumentNullException(nameof(propertyInfo)); }

            return propertyInfo.Name;
        }

    }

}