#region Usings declarations

#endregion

namespace Reefact.FluentRequestBinder.UnitTests.__forTesting {

    internal static class UserConverter {

        #region Statics members declarations

        public static User Convert(RequestConverter<User_v1> bind) {
            RequiredReferenceProperty<Guid>     id       = bind.SimpleProperty(u => u.Id).AsRequired();
            RequiredReferenceProperty<UserName> userName = bind.ComplexProperty(u => u.UserName).AsRequired(UserNameConverter.Convert!);
            bind.AssertHasNoError();

            return new User(id, userName);
        }

        #endregion

    }

}