#region Usings declarations

using System.Diagnostics.CodeAnalysis;

using NFluent;

#endregion

namespace Reefact.FluentRequestBinder.UnitTests {

    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public class RequiredArgument_should {

        [Fact]
        public void Test() {
            // Setup
            ReferenceArgument<string> argument       = new("HiddenInnerWisdom", "42");
            const int                 convertedValue = 42;
            // Exercise
            RequiredValueProperty<int> requiredArgument = RequiredValueProperty<int>.CreateValid(argument, convertedValue);
            // Verify
            Check.That(requiredArgument.Argument.Name).IsEqualTo(argument.Name);
            Check.That(requiredArgument.IsValid).IsEqualTo(true);
            Check.That(requiredArgument.Argument.Value).IsEqualTo(argument.Value);
            Check.That(requiredArgument.Value).IsEqualTo(convertedValue);
        }

        [Fact]
        public void throw_an_exception_trying_to_get_the_value_of_an_invalid_argument() {
            // Setup
            ReferenceArgument<string> argument = new("SarahConnor", "yes");
            // Exercise
            RequiredValueProperty<bool> requiredArgument = RequiredValueProperty<bool>.CreateInvalid(argument);
            // Verify
            Check.That(requiredArgument.Argument.Name).IsEqualTo(argument.Name);
            Check.That(requiredArgument.IsValid).IsEqualTo(false);
            Check.That(requiredArgument.Argument.Value).IsEqualTo(argument.Value);
            Check.ThatCode(() => requiredArgument.Value).Throws<InvalidOperationException>();
        }

        [Fact]
        public void return_the_string_representation_of_the_converted_value_if_is_valid() {
            // Setup
            ValueArgument<int>                argument         = new("argh", 33);
            RequiredReferenceProperty<string> requiredArgument = RequiredReferenceProperty<string>.CreateValid(argument, "trente-trois");
            // Exercise
            string output = requiredArgument.ToString();
            // Verify
            Check.That(output).IsEqualTo("trente-trois");
        }

        [Fact]
        public void return_the_string_representation_of_the_original_value_if_is_not_valid() {
            // Setup
            ValueArgument<int>         argument         = new("argh", 33);
            RequiredValueProperty<int> requiredArgument = RequiredValueProperty<int>.CreateInvalid(argument);
            // Exercise
            string output = requiredArgument.ToString();
            // Verify
            Check.That(output).IsEqualTo("33");
        }

        [Fact]
        public void never_accept_null_arg_name() {
            // Exercise & verify
            Check.ThatCode(() => RequiredValueProperty<int>.CreateInvalid(null!))
                 .Throws<ArgumentNullException>();
            Check.ThatCode(() => RequiredValueProperty<int>.CreateValid(null!, 33))
                 .Throws<ArgumentNullException>();
        }

        [Fact]
        public void not_accept_a_null_arg_value_when_is_valid() {
            // Setup
            ValueArgument<int> argument = new("anyName", null);
            // Exercise & verify
            Check.ThatCode(() => RequiredReferenceProperty<string>.CreateValid(argument, "trente-trois"))
                 .Throws<RequiredPropertyException>()
                 .WithMessage("A required property could not be valid if argument value is null.");
        }

        [Fact]
        public void not_accept_a_null_converted_arg_value_when_is_valid() {
            // Setup
            ReferenceArgument<string> argument = new("anyName", "33");
            // Exercise & verify
            Check.ThatCode(() => RequiredReferenceProperty<string>.CreateValid(argument, null!))
                 .Throws<RequiredPropertyException>()
                 .WithMessage("A required property could not be valid if property value is null.");
        }

        [Fact]
        public void return_its_value_when_implicitly_converted_while_valid() {
            // Setup
            ValueArgument<int>                argument         = new("argh", 33);
            RequiredReferenceProperty<string> requiredArgument = RequiredReferenceProperty<string>.CreateValid(argument, "trente-trois");
            // Exercise
            string value = requiredArgument;
            // Verify
            Check.That(value).IsEqualTo("trente-trois");
        }

        [Fact]
        public void throw_an_exception_when_implicitly_converted_while_invalid() {
            // Setup
            ValueArgument<int>                argument         = new("argh", 33);
            RequiredReferenceProperty<string> requiredArgument = RequiredReferenceProperty<string>.CreateInvalid(argument);
            // Exercise & verify
            Check.ThatCode(() => {
                      // ReSharper disable once UnusedVariable
                      string value = requiredArgument;
                  })
                 .Throws<InvalidOperationException>().WithMessage("Property is not valid.");
        }

    }

}