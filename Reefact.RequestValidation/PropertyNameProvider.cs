#region Usings declarations

using System.Reflection;

#endregion

namespace Reefact.RequestValidation {

    public interface PropertyNameProvider {

        string GetName(PropertyInfo propertyInfo);

    }

}