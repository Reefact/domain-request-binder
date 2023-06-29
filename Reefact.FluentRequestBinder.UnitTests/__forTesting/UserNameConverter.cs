#region Usings declarations

#endregion

namespace Reefact.FluentRequestBinder.UnitTests.__forTesting {

    internal static class UserNameConverter {

        #region Statics members declarations

        public static UserName Convert(RequestConverter<UserName_v1> bind) {
            RequiredProperty<string> firstName = bind.SimpleProperty(x => x.FirstName).AsRequired()!;
            RequiredProperty<string> lastName  = bind.SimpleProperty(x => x.LastName).AsRequired()!;
            bind.AssertHasNoError();

            return new UserName(firstName, lastName);
        }

        #endregion

    }

}