using System;
using System.Linq;

using NFluent;

using Xunit;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable MemberCanBePrivate.Local
// ReSharper disable ClassNeverInstantiated.Local

namespace Reefact.FluentRequestBinder.UnitTests {

    public class Bind_should {

        [Fact]
        public void handle_correctly_when_a_complex_required_property_is_null() {
            Request_v1                   requestWithoutUser = new();
            RequestConverter<Request_v1> bind               = Bind.PropertiesOf(requestWithoutUser);
            RequiredProperty<User>       user               = bind.ComplexProperty(r => r.User).AsRequired(ConvertUser!);
            Check.That(user.IsValid).IsFalse();
            Check.That(bind.HasError).IsTrue();
            Check.That(bind.ErrorCount).IsEqualTo(1);
            ValidationError error = bind.GetErrors().First();
            Check.That(error.ArgumentName).IsEqualTo("User");
            Check.That(error.ErrorMessage).IsEqualTo("Argument is required.");
        }

        [Fact]
        public void handle_correctly_tree_naming_for_complex_properties() {
            Request_v1 requestWithoutUserName = new() {
                User = new User_v1 {
                    Id = Guid.NewGuid()
                }
            };
            RequestConverter<Request_v1> bind = Bind.PropertiesOf(requestWithoutUserName);
            RequiredProperty<User>       user = bind.ComplexProperty(r => r.User).AsRequired(ConvertUser!);
            Check.That(user.IsValid).IsFalse();
            Check.That(bind.HasError).IsTrue();
            Check.That(bind.ErrorCount).IsEqualTo(1);
            ValidationError error = bind.GetErrors().First();
            Check.That(error.ArgumentName).IsEqualTo("User.UserName");
            Check.That(error.ErrorMessage).IsEqualTo("Argument is required.");
        }

        private User ConvertUser(RequestConverter<User_v1> bind) {
            RequiredProperty<Guid>     id       = bind.SimpleProperty(u => u.Id).AsRequired();
            RequiredProperty<UserName> userName = bind.ComplexProperty(u => u.UserName).AsRequired(ConvertUserName!);
            bind.AssertHasNoError();

            return new User(id, userName);
        }

        private UserName ConvertUserName(RequestConverter<UserName_v1> bind) {
            RequiredProperty<string> firstName = bind.SimpleProperty(x => x.FirstName).AsRequired()!;
            RequiredProperty<string> lastName  = bind.SimpleProperty(x => x.LastName).AsRequired()!;
            bind.AssertHasNoError();

            return new UserName(firstName, lastName);
        }

        #region Nested types declarations

        private class Request_v1 {

            // ReSharper disable once MemberHidesStaticFromOuterClass
            public User_v1? User { get; set; }

        }

        private class User_v1 {

            public Guid         Id       { get; set; }
            // ReSharper disable once MemberHidesStaticFromOuterClass
            public UserName_v1? UserName { get; set; }

        }

        private class UserName_v1 {

            public string? FirstName { get; set; }
            public string? LastName  { get; set; }

        }

        public class UserName {

            #region Constructors declarations

            public UserName(string firstName, string lastName) {
                if (firstName is null) { throw new ArgumentNullException(nameof(firstName)); }
                if (lastName is null) { throw new ArgumentNullException(nameof(lastName)); }

                FirstName = firstName;
                LastName  = lastName;
            }

            #endregion

            public string FirstName { get; }
            public string LastName  { get; }

        }

        private class User {

            #region Constructors declarations

            public User(Guid id, UserName userName) {
                Id       = id;
                UserName = userName;
            }

            #endregion

            public Guid     Id       { get; }
            public UserName UserName { get; }

        }

        #endregion

    }

}