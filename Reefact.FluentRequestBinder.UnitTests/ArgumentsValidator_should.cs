// ReSharper disable RedundantUsingDirective

#region Usings declarations

using NFluent;

using System;
using System.Diagnostics.CodeAnalysis;

using Xunit;

using System.Linq;

using Reefact.FluentRequestBinder.Configuration;
using Reefact.FluentRequestBinder.UnitTests.__forTesting;

#endregion

namespace Reefact.FluentRequestBinder.UnitTests {

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public class ArgumentsValidator_should {

        #region Statics members declarations

        [Fact]
        public static void handle_correctly_the_conversion_of_one_valid_required_input() {
            // Setup
            ArgumentsConverter validator     = Bind.Arguments();
            Guid               anyGuid       = Guid.Parse("3EAFD5D9-CB0C-4D24-945A-9D9713D19B65");
            AnyId              expectedAnyId = AnyId.From(anyGuid);
            string             anyName       = "teamId";
            // Exercise
            RequiredReferenceProperty<AnyId> id = validator.SimpleProperty(anyName, anyGuid).AsRequired(AnyId.From);
            // Verify
            // - converted value
            Check.That(id.Argument.Name).IsEqualTo(anyName);
            Check.That(id.Argument.Value).IsEqualTo(anyGuid);
            Check.That(id.IsValid).IsTrue();
            Check.That(id.Value).IsEqualTo(expectedAnyId);
            // - validation
            Check.That(validator.HasError).IsFalse();
            Check.That(validator.ErrorCount).IsEqualTo(0);
            Check.That(validator.ToString()).IsEqualTo("No error recorded.");
            Check.That(validator.GetErrors()).CountIs(0);
        }

        [Fact]
        public static void handle_correctly_the_conversion_of_one_valid_optional_input() {
            // Setup
            ArgumentsConverter validator     = Bind.Arguments();
            Guid               anyGuid       = Guid.Parse("3EAFD5D9-CB0C-4D24-945A-9D9713D19B65");
            AnyId              expectedAnyId = AnyId.From(anyGuid);
            string             anyName       = "teamId";
            // Exercise
            OptionalProperty<AnyId> id = validator.SimpleProperty(anyName, anyGuid).AsOptional(AnyId.From);
            // Verify
            // - converted value
            Check.That(id.Argument.Name).IsEqualTo(anyName);
            Check.That(id.Argument.Value).IsEqualTo(anyGuid);
            Check.That(id.IsValid).IsTrue();
            Check.That(id.IsMissing).IsFalse();
            Check.That(id.Value).IsEqualTo(expectedAnyId);
            // - validation
            Check.That(validator.HasError).IsFalse();
            Check.That(validator.ErrorCount).IsEqualTo(0);
            Check.That(validator.ToString()).IsEqualTo("No error recorded.");
            Check.That(validator.GetErrors()).CountIs(0);
        }

        [Fact]
        public static void handle_correctly_the_conversion_of_one_missing_required_input() {
            // Setup
            ArgumentsConverter validator = Bind.Arguments();
            string             anyName   = "teamId";
            // Exercise
            RequiredReferenceProperty<AnyId> id = validator.SimpleProperty(anyName, (Guid?)null).AsRequired(AnyId.From);
            // Verify
            // - converted value
            Check.That(id.Argument.Name).IsEqualTo(anyName);
            Check.That(id.Argument.Value).IsNull();
            Check.That(id.IsValid).IsFalse();
            Check.ThatCode(() => id.Value)
                 .Throws<InvalidOperationException>()
                 .WithMessage("Property is not valid.");
            // - validation
            Check.That(validator.HasError).IsTrue();
            Check.That(validator.ErrorCount).IsEqualTo(1);
            Check.That(validator.ToString()).IsEqualTo("1 error has been recorded.");
            ValidationError[] errors = validator.GetErrors().ToArray();
            Check.That(errors).CountIs(1);
            ValidationError singleError = errors[0];
            Check.That(singleError.ArgumentName).IsEqualTo(anyName);
            Check.That(singleError.ErrorMessage).IsEqualTo("Argument is required.");
        }

        [Fact]
        public static void handle_correctly_the_conversion_of_one_missing_optional_input() {
            // Setup
            ArgumentsConverter validator = Bind.Arguments();
            string             anyName   = "teamId";
            // Exercise
            OptionalProperty<AnyId> id = validator.SimpleProperty(anyName, (Guid?)null).AsOptional(AnyId.From);
            // Verify
            // - converted value
            Check.That(id.Argument.Name).IsEqualTo(anyName);
            Check.That(id.Argument.Value).IsNull();
            Check.That(id.IsValid).IsTrue();
            Check.That(id.IsMissing).IsTrue();
            Check.That(id.Value).IsNull();
            // - validation
            Check.That(validator.HasError).IsFalse();
        }

        [Fact]
        public static void handle_correctly_the_failing_conversion_of_one_required_input() {
            // Setup
            ArgumentsConverter validator = Bind.Arguments();
            Guid               anyGuid   = Guid.Parse("3EAFD5D9-CB0C-4D24-945A-9D9713D19B65");
            string             anyName   = "teamId";
            // Exercise
            RequiredReferenceProperty<AnyId> id = validator.SimpleProperty(anyName, anyGuid).AsRequired<AnyId>(_ => throw new ApplicationException("Oulala"));
            // Verify
            Check.That(id.Argument.Name).IsEqualTo(anyName);
            Check.That(id.Argument.Value).IsEqualTo(anyGuid);
            Check.That(id.IsValid).IsFalse();
            Check.ThatCode(() => id.Value)
                 .Throws<InvalidOperationException>()
                 .WithMessage("Property is not valid.");
            // - validation
            Check.That(validator.HasError).IsTrue();
            Check.That(validator.ErrorCount).IsEqualTo(1);
            Check.That(validator.ToString()).IsEqualTo("1 error has been recorded.");
            ValidationError[] errors = validator.GetErrors().ToArray();
            Check.That(errors).CountIs(1);
            ValidationError singleError = errors[0];
            Check.That(singleError.ArgumentName).IsEqualTo(anyName);
            Check.That(singleError.ErrorMessage).IsEqualTo("Oulala");
        }

        [Fact]
        public static void handle_correctly_the_failing_conversion_of_one_optional_input() {
            // Setup
            ArgumentsConverter validator = Bind.Arguments();
            Guid               anyGuid   = Guid.Parse("3EAFD5D9-CB0C-4D24-945A-9D9713D19B65");
            string             anyName   = "teamId";
            // Exercise
            OptionalProperty<AnyId> id = validator.SimpleProperty(anyName, anyGuid).AsOptional<AnyId>(_ => throw new ApplicationException("Oulala"));
            // Verify
            Check.That(id.Argument.Name).IsEqualTo(anyName);
            Check.That(id.Argument.Value).IsEqualTo(anyGuid);
            Check.That(id.IsValid).IsFalse();
            Check.ThatCode(() => id.Value)
                 .Throws<InvalidOperationException>()
                 .WithMessage("Property is not valid.");
            // - validation
            Check.That(validator.HasError).IsTrue();
            Check.That(validator.ErrorCount).IsEqualTo(1);
            Check.That(validator.ToString()).IsEqualTo("1 error has been recorded.");
            ValidationError[] errors = validator.GetErrors().ToArray();
            Check.That(errors).CountIs(1);
            ValidationError singleError = errors[0];
            Check.That(singleError.ArgumentName).IsEqualTo(anyName);
            Check.That(singleError.ErrorMessage).IsEqualTo("Oulala");
        }

        [Fact]
        public static void handle_correctly_the_conversion_of_two_valid_input() {
            // Setup
            ArgumentsConverter validator = Bind.Arguments();
            Guid               anyGuid   = Guid.Parse("3EAFD5D9-CB0C-4D24-945A-9D9713D19B65");
            // Exercise
            validator.SimpleProperty("arg1", anyGuid).AsRequired(AnyId.From);
            validator.SimpleProperty("arg2", "oui").AsRequired(FrenchYesNoConvert);
            // Verify
            Check.That(validator.HasError).IsFalse();
            Check.That(validator.ToString()).IsEqualTo("No error recorded.");
        }

        [Fact]
        public static void handle_correctly_the_conversion_of_one_valid_input_and_one_missing_required_input() {
            // Setup
            ArgumentsConverter validator = Bind.Arguments();
            // Exercise
            validator.SimpleProperty("arg1", (Guid?)null).AsRequired(AnyId.From);
            validator.SimpleProperty("arg2", "oui").AsRequired(FrenchYesNoConvert);
            // Verify
            // - validation
            Check.That(validator.HasError).IsTrue();
            Check.That(validator.ErrorCount).IsEqualTo(1);
            Check.That(validator.ToString()).IsEqualTo("1 error has been recorded.");
            ValidationError[] errors = validator.GetErrors().ToArray();
            Check.That(errors).CountIs(1);
            ValidationError singleError = errors[0];
            Check.That(singleError.ArgumentName).IsEqualTo("arg1");
            Check.That(singleError.ErrorMessage).IsEqualTo("Argument is required.");
        }

        [Fact]
        public static void handle_correctly_the_conversion_of_one_valid_input_and_one_missing_required_input_and_one_failing_conversion() {
            // Setup
            ArgumentsConverter validator = Bind.Arguments();
            // Exercise
            validator.SimpleProperty("arg1", (Guid?)null).AsRequired(AnyId.From);
            validator.SimpleProperty("arg2", "oui").AsRequired(FrenchYesNoConvert);
            validator.SimpleProperty("arg3", "oui").AsRequired<bool>(_ => throw new ApplicationException("Oula!"));
            // Verify
            // - validation
            Check.That(validator.HasError).IsTrue();
            Check.That(validator.ErrorCount).IsEqualTo(2);
            Check.That(validator.ToString()).IsEqualTo("2 errors have been recorded.");
            ValidationError[] errors = validator.GetErrors().ToArray();
            Check.That(errors).CountIs(2);
            Check.That(errors).Contains(new ValidationError("arg1", "Argument is required."));
            Check.That(errors).Contains(new ValidationError("arg3", "Oula!"));
        }

        // TODO: test optional 

        private static bool FrenchYesNoConvert(string value) {
            switch (value) {
                case "oui": return true;
                case "non": return false;
                default:    throw new FormatException();
            }
        }

        #endregion

    }

}