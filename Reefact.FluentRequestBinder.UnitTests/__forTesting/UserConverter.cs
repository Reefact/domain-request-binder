namespace Reefact.FluentRequestBinder.UnitTests.__forTesting {

    internal static class UserConverter {

        #region Statics members declarations

        public static User Convert(RequestConverter<User_v1> bind) {
            RequiredProperty<Guid>     id       = bind.SimpleProperty(u => u.Id).AsRequired();
            OptionalProperty<int?>     age      = bind.SimpleProperty(u => u.Age).AsOptional();
            RequiredProperty<UserName> userName = bind.ComplexProperty(u => u.UserName).AsRequired(UserNameConverter.Convert!);
            bind.AssertHasNoError();

            return new User(id, age, userName);
        }

        #endregion

    }

}