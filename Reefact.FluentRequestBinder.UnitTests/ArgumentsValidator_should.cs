// ReSharper disable RedundantUsingDirective

#region Usings declarations

using NFluent;

using System;

using Xunit;

using System.Linq;

using Reefact.FluentRequestBinder.UnitTests.__forTesting;

#endregion

namespace Reefact.FluentRequestBinder.UnitTests {

    public class ArgumentsValidator_should {

        #region Statics members declarations

        private static bool FrenchYesNoConvert(string value) {
            switch (value) {
                case "oui": return true;
                case "non": return false;
                default:    throw new FormatException();
            }
        }

        #endregion

        [Fact]
        public void handle_correctly_the_conversion_of_one_valid_input() {
            // Setup
            ArgumentsValidator validator = Validate.Arguments();
            Guid               anyGuid   = Guid.Parse("3EAFD5D9-CB0C-4D24-945A-9D9713D19B65");
            var                anyName   = "teamId";
            // Exercise
            RequiredArgument<AnyId> id = validator.ConvertRequired(anyName, anyGuid, AnyId.From);
            // Verify
            // - converted value
            Check.That(id.Name).IsEqualTo(anyName);
            Check.That(id.OriginalValue).IsEqualTo(anyGuid);
            Check.That(id.IsValid).IsTrue();
            Check.That(id.Value).IsEqualTo(AnyId.From(anyGuid));
            // - validation
            Check.That(validator.HasError).IsFalse();
            Check.That(validator.ErrorCount).IsEqualTo(0);
            Check.That(validator.ToString()).IsEqualTo("No error detected.");
            Check.That(validator.GetErrors()).CountIs(0);
        }

        [Fact]
        public void handle_correctly_the_conversion_of_one_missing_required_input() {
            // Setup
            ArgumentsValidator validator = Validate.Arguments();
            var                anyName   = "teamId";
            // Exercise
            RequiredArgument<AnyId> id = validator.ConvertRequired(anyName, (Guid?)null, AnyId.From);
            // Verify
            // - converted value
            Check.That(id.Name).IsEqualTo(anyName);
            Check.That(id.OriginalValue).IsNull();
            Check.That(id.IsValid).IsFalse();
            Check.ThatCode(() => id.Value)
                 .Throws<InvalidOperationException>()
                 .WithMessage("Argument is not valid.");
            // - validation
            Check.That(validator.HasError).IsTrue();
            Check.That(validator.ErrorCount).IsEqualTo(1);
            Check.That(validator.ToString()).IsEqualTo("1 error has been detected.");
            ValidationError[] errors = validator.GetErrors().ToArray();
            Check.That(errors).CountIs(1);
            ValidationError singleError = errors[0];
            Check.That(singleError.FieldName).IsEqualTo(anyName);
            Check.That(singleError.ErrorMessage).IsEqualTo("Argument is required.");
        }

        [Fact]
        public void handle_correctly_the_failing_conversion_of_one_required_input() {
            // Setup
            ArgumentsValidator validator = Validate.Arguments();
            Guid               anyGuid   = Guid.Parse("3EAFD5D9-CB0C-4D24-945A-9D9713D19B65");
            var                anyName   = "teamId";
            // Exercise
            RequiredArgument<AnyId> id = validator.ConvertRequired<Guid, AnyId>(anyName, anyGuid, x => throw new ApplicationException("Oulala"));
            // Verify
            Check.That(id.Name).IsEqualTo(anyName);
            Check.That(id.OriginalValue).IsEqualTo(anyGuid);
            Check.That(id.IsValid).IsFalse();
            Check.ThatCode(() => id.Value)
                 .Throws<InvalidOperationException>()
                 .WithMessage("Argument is not valid.");
            // - validation
            Check.That(validator.HasError).IsTrue();
            Check.That(validator.ErrorCount).IsEqualTo(1);
            Check.That(validator.ToString()).IsEqualTo("1 error has been detected.");
            ValidationError[] errors = validator.GetErrors().ToArray();
            Check.That(errors).CountIs(1);
            ValidationError singleError = errors[0];
            Check.That(singleError.FieldName).IsEqualTo(anyName);
            Check.That(singleError.ErrorMessage).IsEqualTo("Oulala");
        }

        [Fact]
        public void handle_correctly_the_conversion_of_two_valid_input() {
            // Setup
            ArgumentsValidator validator = Validate.Arguments();
            Guid               anyGuid   = Guid.Parse("3EAFD5D9-CB0C-4D24-945A-9D9713D19B65");
            // Exercise
            validator.ConvertRequired("arg1", anyGuid, AnyId.From);
            validator.ConvertRequired("arg2", "oui", FrenchYesNoConvert);
            // Verify
            Check.That(validator.HasError).IsFalse();
            Check.That(validator.ToString()).IsEqualTo("No error detected.");
        }

        [Fact]
        public void handle_correctly_the_conversion_of_one_valid_input_and_one_missing_required_input() {
            // Setup
            ArgumentsValidator validator = Validate.Arguments();
            // Exercise
            validator.ConvertRequired("arg1", (Guid?)null, AnyId.From);
            validator.ConvertRequired("arg2", "oui", FrenchYesNoConvert);
            // Verify
            // - validation
            Check.That(validator.HasError).IsTrue();
            Check.That(validator.ErrorCount).IsEqualTo(1);
            Check.That(validator.ToString()).IsEqualTo("1 error has been detected.");
            ValidationError[] errors = validator.GetErrors().ToArray();
            Check.That(errors).CountIs(1);
            ValidationError singleError = errors[0];
            Check.That(singleError.FieldName).IsEqualTo("arg1");
            Check.That(singleError.ErrorMessage).IsEqualTo("Argument is required.");
        }

        [Fact]
        public void handle_correctly_the_conversion_of_one_valid_input_and_one_missing_required_input_and_one_failing_conversion() {
            // Setup
            ArgumentsValidator validator = Validate.Arguments();
            // Exercise
            validator.ConvertRequired("arg1", (Guid?)null, AnyId.From);
            validator.ConvertRequired("arg2", "oui", FrenchYesNoConvert);
            validator.ConvertRequired<string, bool>("arg3", "oui", x => throw new ApplicationException("Oula!"));
            // Verify
            // - validation
            Check.That(validator.HasError).IsTrue();
            Check.That(validator.ErrorCount).IsEqualTo(2);
            Check.That(validator.ToString()).IsEqualTo("2 errors have been detected.");
            ValidationError[] errors = validator.GetErrors().ToArray();
            Check.That(errors).CountIs(2);
            Check.That(errors).Contains(new ValidationError("arg1", "Argument is required."));
            Check.That(errors).Contains(new ValidationError("arg3", "Oula!"));
        }

    }

}