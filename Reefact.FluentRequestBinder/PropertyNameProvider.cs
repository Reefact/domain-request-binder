#region Usings declarations

using System.Reflection;

#endregion

namespace Reefact.FluentRequestBinder {

    public interface PropertyNameProvider {

        string GetName(PropertyInfo propertyInfo);

    }

}