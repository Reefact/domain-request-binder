#region Usings declarations

using System;

#endregion

namespace Reefact.FluentRequestBinder.UnitTests.__forTesting {

    internal static class UserConverter {

        #region Statics members declarations

        public static User Convert(RequestConverter<User_v1> bind) {
            RequiredProperty<Guid>     id       = bind.SimpleProperty(u => u.Id).AsRequired();
            RequiredProperty<UserName> userName = bind.ComplexProperty(u => u.UserName).AsRequired(UserNameConverter.Convert!);
            bind.AssertHasNoError();

            return new User(id, userName);
        }

        #endregion

    }

}