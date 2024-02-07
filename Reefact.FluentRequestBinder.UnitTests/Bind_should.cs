#region Usings declarations

using NFluent;

using Reefact.FluentRequestBinder.UnitTests.__forTesting;

#endregion

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
            // Setup
            Request_v1                   requestWithoutUser = new();
            RequestConverter<Request_v1> bind               = Bind.PropertiesOf(requestWithoutUser);

            // Exercise
            RequiredReferenceProperty<User> user = bind.ComplexProperty(r => r.User).AsRequired(UserConverter.Convert!);

            // Verify
            // - property
            Check.That(user.IsValid).IsFalse();

            // - binder
            Check.That(bind.HasError).IsTrue();
            Check.That(bind.ErrorCount).IsEqualTo(1);
            ValidationError error = bind.GetErrors().First();
            Check.That(error.ArgumentName).IsEqualTo("User");
            Check.That(error.ErrorMessage).IsEqualTo("Argument is required.");
        }

        [Fact]
        public void handle_correctly_tree_naming_for_complex_properties() {
            // Setup
            Request_v1 requestWithoutUserName = new() {
                User = new User_v1 {
                    Id = Guid.NewGuid()
                }
            };
            RequestConverter<Request_v1> bind = Bind.PropertiesOf(requestWithoutUserName);

            // Exercise
            RequiredReferenceProperty<User> user = bind.ComplexProperty(r => r.User).AsRequired(UserConverter.Convert!);

            // Verify
            // - property
            Check.That(user.IsValid).IsFalse();

            // - binder
            Check.That(bind.HasError).IsTrue();
            Check.That(bind.ErrorCount).IsEqualTo(1);
            ValidationError error = bind.GetErrors().First();
            Check.That(error.ArgumentName).IsEqualTo("User.UserName");
            Check.That(error.ErrorMessage).IsEqualTo("Argument is required.");
        }

        [Fact]
        public void handle_correctly_required_list_of_complex_properties() {
            // Setup
            Request_v1 requestWitRoles = new();
            requestWitRoles.Roles = new List<Role_v1> {
                new() { Id = "ADM", Name = "Administrator" },
                new() { Id = "DEV", Name = "Developer" }
            };
            RequestConverter<Request_v1> bind = Bind.PropertiesOf(requestWitRoles);

            // Exercise
            RequiredList<Role> roles = bind.ListOfComplexProperties(r => r.Roles!).AsRequired(RoleConverter.Convert);

            // Verify
            // - list
            Check.That(roles.IsValid).IsTrue();
            Check.That((List<Role>)roles).IsEquivalentTo(new Role("ADM", "Administrator"), new Role("DEV", "Developer"));

            // - binder
            Check.That(bind.HasError).IsFalse();
        }

        [Fact]
        public void handle_correctly_missing_required_list_of_complex_properties() {
            // Setup
            Request_v1 requestWithMissingRoles = new();
            requestWithMissingRoles.Roles = null;
            RequestConverter<Request_v1> bind = Bind.PropertiesOf(requestWithMissingRoles);

            // Exercise
            RequiredList<Role> roles = bind.ListOfComplexProperties(r => r.Roles!).AsRequired(RoleConverter.Convert);

            // Verify
            // - required property
            Check.That(roles.IsValid).IsFalse();
            Check.ThatCode(() => roles.Value)
                 .Throws<InvalidOperationException>()
                 .WithMessage("Property is not valid.");

            // - binder
            Check.That(bind.HasError).IsTrue();
            Check.That(bind.ErrorCount).IsEqualTo(1);

            ValidationError validationError = bind.GetErrors().First();
            Check.That(validationError.ArgumentName).IsEqualTo("Roles");
            Check.That(validationError.ErrorMessage).IsEqualTo("Argument is required.");
        }

        [Fact]
        public void handle_correctly_required_list_of_complex_properties_having_one_item_invalid() {
            // Setup
            Request_v1 requestWitRoles = new();
            requestWitRoles.Roles = new List<Role_v1> {
                new() { Id = "ADM", Name = "Administrator" },
                new() { Id = "USR" }, // Name is missing
                new() { Id = "DEV", Name = "Developer" }
            };
            RequestConverter<Request_v1> bind = Bind.PropertiesOf(requestWitRoles);

            // Exercise
            RequiredList<Role> roles = bind.ListOfComplexProperties(r => r.Roles!).AsRequired(RoleConverter.Convert);

            // Verify
            // - required property
            Check.That(roles.IsValid).IsFalse();

            // - binder
            Check.That(bind.HasError).IsTrue();
            Check.That(bind.ErrorCount).IsEqualTo(1);

            ValidationError validationError = bind.GetErrors().First();
            Check.That(validationError.ArgumentName).IsEqualTo("Roles[1].Name");
            Check.That(validationError.ErrorMessage).IsEqualTo("Argument is required.");
        }

        [Fact]
        public void handle_correctly_optional_list_of_complex_properties() {
            // Setup
            Request_v1 requestWitRoles = new();
            requestWitRoles.Roles = new List<Role_v1> {
                new() { Id = "ADM", Name = "Administrator" },
                new() { Id = "DEV", Name = "Developer" }
            };
            RequestConverter<Request_v1> bind = Bind.PropertiesOf(requestWitRoles);

            // Exercise
            OptionalList<Role> roles = bind.ListOfComplexProperties(r => r.Roles!).AsOptional(RoleConverter.Convert);

            // Verify
            // - property
            Check.That(roles.IsValid).IsTrue();
            Check.That(roles.IsMissing).IsFalse();
            Check.That(roles.ArgumentName).IsEqualTo("Roles");
            Check.That(roles.Value).IsEquivalentTo(new Role("ADM", "Administrator"), new Role("DEV", "Developer"));

            // - binder
            Check.That(bind.HasError).IsFalse();
        }

        [Fact]
        public void handle_correctly_missing_optional_list_of_complex_properties() {
            // Setup
            Request_v1                   requestWitRoles = new();
            RequestConverter<Request_v1> bind            = Bind.PropertiesOf(requestWitRoles);

            // Exercise
            OptionalList<Role> roles = bind.ListOfComplexProperties(r => r.Roles!).AsOptional(RoleConverter.Convert);

            // Verify
            // - property
            Check.That(roles.IsValid).IsTrue();
            Check.That(roles.IsMissing).IsTrue();
            Check.That(roles.Value).IsNotNull();
            Check.That(roles.Value).IsEmpty();

            // - binder
            Check.That(bind.HasError).IsFalse();
        }

        [Fact]
        public void handle_correctly_optional_list_of_complex_properties_having_one_item_invalid() {
            // Setup
            Request_v1 requestWitRoles = new();
            requestWitRoles.Roles = new List<Role_v1> {
                new() { Id = "ADM", Name = "Administrator" },
                new() { Id = "USR" }, // Name is missing
                new() { Id = "DEV", Name = "Developer" }
            };
            RequestConverter<Request_v1> bind = Bind.PropertiesOf(requestWitRoles);

            // Exercise
            OptionalList<Role> roles = bind.ListOfComplexProperties(r => r.Roles!).AsOptional(RoleConverter.Convert);

            // Verify
            // - required property
            Check.That(roles.IsValid).IsFalse();

            // - binder
            Check.That(bind.HasError).IsTrue();
            Check.That(bind.ErrorCount).IsEqualTo(1);

            ValidationError validationError = bind.GetErrors().First();
            Check.That(validationError.ArgumentName).IsEqualTo("Roles[1].Name");
            Check.That(validationError.ErrorMessage).IsEqualTo("Argument is required.");
        }

        [Fact]
        public void handle_correctly_required_list_of_simple_properties() {
            // Setup
            Request_v1 requestWitRoles = new();
            Guid       guid1           = Guid.NewGuid();
            Guid       guid2           = Guid.NewGuid();
            requestWitRoles.FriendIds = new List<Guid> {
                guid1,
                guid2
            };
            RequestConverter<Request_v1> bind = Bind.PropertiesOf(requestWitRoles);

            // Exercise
            RequiredList<AnyId> friendIds = bind.ListOfSimpleProperties(r => r.FriendIds!).AsRequired(AnyId.From);

            // Verify
            // - property
            Check.That(friendIds.IsValid).IsTrue();
            Check.That(friendIds.ArgumentName).IsEqualTo("FriendIds");
            Check.That(friendIds.Value).IsEquivalentTo(AnyId.From(guid1), AnyId.From(guid2));

            // - binder
            Check.That(bind.HasError).IsFalse();
        }

        [Fact]
        public void handle_correctly_missing_required_list_of_simple_properties() {
            // Setup
            Request_v1                   requestWithMissingRoles = new();
            RequestConverter<Request_v1> bind                    = Bind.PropertiesOf(requestWithMissingRoles);

            // Exercise
            RequiredList<AnyId> friendIds = bind.ListOfSimpleProperties(r => r.FriendIds!).AsRequired(AnyId.From);

            // Verify
            // - required property
            Check.That(friendIds.IsValid).IsFalse();
            Check.ThatCode(() => friendIds.Value)
                 .Throws<InvalidOperationException>()
                 .WithMessage("Property is not valid.");

            // - binder
            Check.That(bind.HasError).IsTrue();
            Check.That(bind.ErrorCount).IsEqualTo(1);

            ValidationError validationError = bind.GetErrors().First();
            Check.That(validationError.ArgumentName).IsEqualTo("FriendIds");
            Check.That(validationError.ErrorMessage).IsEqualTo("Argument is required.");
        }

        [Fact]
        public void handle_correctly_required_list_of_simple_properties_having_one_item_invalid() {
            // Setup
            Request_v1 requestWitRoles = new();
            Guid       guid1           = Guid.NewGuid();
            Guid       guid2           = Guid.NewGuid();
            requestWitRoles.FriendIds = new List<Guid> {
                guid1,
                Guid.Empty,
                guid2
            };
            RequestConverter<Request_v1> bind = Bind.PropertiesOf(requestWitRoles);

            // Exercise
            RequiredList<AnyId> friendIds = bind.ListOfSimpleProperties(r => r.FriendIds!).AsRequired(AnyId.From);

            // Verify
            // - required property
            Check.That(friendIds.IsValid).IsFalse();

            // - binder
            Check.That(bind.HasError).IsTrue();
            Check.That(bind.ErrorCount).IsEqualTo(1);

            ValidationError validationError = bind.GetErrors().First();
            Check.That(validationError.ArgumentName).IsEqualTo("FriendIds[1]");
            Check.That(validationError.ErrorMessage).IsEqualTo("GUID cannot be empty.");
        }

        [Fact]
        public void handle_correctly_optional_list_of_simple_properties() {
            // Setup
            Request_v1 requestWitRoles = new();
            Guid       guid1           = Guid.NewGuid();
            Guid       guid2           = Guid.NewGuid();
            requestWitRoles.FriendIds = new List<Guid> {
                guid1,
                guid2
            };
            RequestConverter<Request_v1> bind = Bind.PropertiesOf(requestWitRoles);

            // Exercise
            OptionalList<AnyId> friendIds = bind.ListOfSimpleProperties(r => r.FriendIds!).AsOptional(AnyId.From);

            // Verify
            // - property
            Check.That(friendIds.IsValid).IsTrue();
            Check.That(friendIds.IsMissing).IsFalse();
            Check.That(friendIds.ArgumentName).IsEqualTo("FriendIds");
            Check.That(friendIds.Value).IsEquivalentTo(AnyId.From(guid1), AnyId.From(guid2));

            // - binder
            Check.That(bind.HasError).IsFalse();
        }

        [Fact]
        public void handle_correctly_missing_optional_list_of_simple_properties() {
            // Setup
            Request_v1                   requestWitRoles = new();
            RequestConverter<Request_v1> bind            = Bind.PropertiesOf(requestWitRoles);

            // Exercise
            OptionalList<AnyId> friendIds = bind.ListOfSimpleProperties(r => r.FriendIds!).AsOptional(AnyId.From);

            // Verify
            // - property
            Check.That(friendIds.IsValid).IsTrue();
            Check.That(friendIds.IsMissing).IsTrue();
            Check.That(friendIds.Value).IsNotNull();
            Check.That(friendIds.Value).IsEmpty();

            // - binder
            Check.That(bind.HasError).IsFalse();
        }

        [Fact]
        public void handle_correctly_optional_list_of_simple_properties_having_one_item_invalid() {
            // Setup
            Request_v1 requestWitRoles = new();
            Guid       guid1           = Guid.NewGuid();
            Guid       guid2           = Guid.NewGuid();
            requestWitRoles.FriendIds = new List<Guid> {
                guid1,
                Guid.Empty,
                guid2
            };
            RequestConverter<Request_v1> bind = Bind.PropertiesOf(requestWitRoles);

            // Exercise
            OptionalList<AnyId> friendIds = bind.ListOfSimpleProperties(r => r.FriendIds!).AsOptional(AnyId.From);

            // Verify
            // - required property
            Check.That(friendIds.IsValid).IsFalse();

            // - binder
            Check.That(bind.HasError).IsTrue();
            Check.That(bind.ErrorCount).IsEqualTo(1);

            ValidationError validationError = bind.GetErrors().First();
            Check.That(validationError.ArgumentName).IsEqualTo("FriendIds[1]");
            Check.That(validationError.ErrorMessage).IsEqualTo("GUID cannot be empty.");
        }

        [Fact]
        public void handle_correctly_required_simple_property() {
            // Setup
            ArgumentsConverter bind = Bind.Arguments();

            // Exercise
            RequiredValueProperty<int> @int = bind.SimpleProperty("toto", "42").AsRequired(int.Parse);

            // Verify
            Check.That(@int.IsValid).IsTrue();
            Check.That(@int.Value).IsEqualTo(42);
        }

        [Fact]
        public void handle_correctly_list_of_simple_properties() {
            // Setup
            ArgumentsConverter bind = Bind.Arguments();

            // Exercise
            RequiredList<int> @int = bind.ListOfSimpleProperties("toto", new[] { "33", "42", "69" }).AsRequired(int.Parse);

            // Verify
            Check.That(@int.IsValid).IsTrue();
            Check.That(@int.Value).IsEquivalentTo(33, 42, 69);
        }

        [Fact]
        public void handle_correctly_complex_properties() {
            // Setup
            ArgumentsConverter bind = Bind.Arguments();

            // Exercise
            RequiredReferenceProperty<Temperature> temperature = bind.ComplexProperty(new Temperature_v1 { Value = "37", Unit = "celsius" }).AsRequired(TemperatureConverter.Convert);

            // Verify
            Check.That(temperature.IsValid).IsTrue();
            Check.That(temperature.Value).IsEqualTo(new Temperature(37, TemperatureUnit.Celsius));
        }

        [Fact]
        public void handle_correctly_invalid_complex_properties() {
            // Setup
            ArgumentsConverter bind = Bind.Arguments();

            // Exercise
            RequiredReferenceProperty<Temperature> temperature = bind.ComplexProperty(new Temperature_v1 { Value = "42", Unit = "GTI-TURBO" }).AsRequired(TemperatureConverter.Convert);

            // Verify
            // - property
            Check.That(temperature.IsValid).IsFalse();

            // binder
            Check.That(bind.HasError).IsTrue();
            Check.That(bind.ErrorCount).IsEqualTo(1);
            ValidationError validationError = bind.GetErrors().First();
            Check.That(validationError.ArgumentName).IsEqualTo("Unit");
            Check.That(validationError.ErrorMessage).IsEqualTo("Unknown temperature unit.");
        }

        [Fact]
        public void handle_correctly_invalid_list_of_simple_properties() {
            ArgumentsConverter  bind      = Bind.Arguments();
            List<Guid>          parameter = new() { Guid.NewGuid(), Guid.NewGuid(), Guid.Empty };
            RequiredList<AnyId> ids       = bind.ListOfSimpleProperties("toto", parameter).AsRequired(AnyId.From);
            // Verify
            // - property
            Check.That(ids.IsValid).IsFalse();

            // binder
            Check.That(bind.HasError).IsTrue();
            Check.That(bind.ErrorCount).IsEqualTo(1);
            ValidationError validationError = bind.GetErrors().First();
            Check.That(validationError.ArgumentName).IsEqualTo("toto[2]");
            Check.That(validationError.ErrorMessage).IsEqualTo("GUID cannot be empty.");
        }

    }

}