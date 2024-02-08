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
            Argument<string> argument       = new("HiddenInnerWisdom", "42");
            const int        convertedValue = 42;
            // Exercise
            RequiredProperty<int> requiredArgument = RequiredProperty<int>.CreateValid(argument, convertedValue);
            // Verify
            Check.That(requiredArgument.Argument.Name).IsEqualTo(argument.Name);
            Check.That(requiredArgument.IsValid).IsEqualTo(true);
            Check.That(requiredArgument.Argument.Value).IsEqualTo(argument.Value);
            Check.That(requiredArgument.Value).IsEqualTo(convertedValue);
        }

        [Fact]
        public void throw_an_exception_trying_to_get_the_value_of_an_invalid_argument() {
            // Setup
            Argument<string>       argument         = new("SarahConnor", "yes");
            RequiredProperty<bool> requiredArgument = RequiredProperty<bool>.CreateInvalid(argument);
            // Exercise & Verify
            Check.ThatCode(() => requiredArgument.Value)
                 .Throws<PropertyException>()
                 .WithMessage(ExceptionMessage.Property_ValueIsInvalid);
        }

        [Fact]
        public void return_the_string_representation_of_the_converted_value_if_is_valid() {
            // Setup
            Argument<int>            argument         = new("argh", 33);
            RequiredProperty<string> requiredArgument = RequiredProperty<string>.CreateValid(argument, "trente-trois");
            // Exercise
            string output = requiredArgument.ToString();
            // Verify
            Check.That(output).IsEqualTo("trente-trois");
        }

        [Fact]
        public void return_the_string_representation_of_the_original_value_if_is_not_valid() {
            // Setup
            Argument<int>         argument         = new("argh", 33);
            RequiredProperty<int> requiredArgument = RequiredProperty<int>.CreateInvalid(argument);
            // Exercise
            string output = requiredArgument.ToString();
            // Verify
            Check.That(output).IsEqualTo("33");
        }

        [Fact]
        public void never_accept_null_arg_name() {
            // Exercise & verify
            Check.ThatCode(() => RequiredProperty<int>.CreateInvalid(null!))
                 .Throws<ArgumentNullException>();
            Check.ThatCode(() => RequiredProperty<int>.CreateValid(null!, 33))
                 .Throws<ArgumentNullException>();
        }

        [Fact]
        public void not_accept_a_null_arg_value_when_is_valid() {
            // Setup
            Argument<int?> argument = new("anyName", null);
            // Exercise & verify
            Check.ThatCode(() => RequiredProperty<string>.CreateValid(argument, "trente-trois"))
                 .Throws<RequiredPropertyException>()
                 .WithMessage(ExceptionMessage.RequiredProperty_CouldNotBeValidIfArgumentValueIsNull);
        }

        [Fact]
        public void not_accept_a_null_converted_arg_value_when_is_valid() {
            // Setup
            Argument<string> argument = new("anyName", "33");
            // Exercise & verify
            Check.ThatCode(() => RequiredProperty<string>.CreateValid(argument, null!))
                 .Throws<RequiredPropertyException>()
                 .WithMessage(ExceptionMessage.RequiredProperty_CouldNotBeValidIfPropertyValueIsNull);
        }

        [Fact]
        public void return_its_value_when_implicitly_converted_while_valid() {
            // Setup
            Argument<int>            argument         = new("argh", 33);
            RequiredProperty<string> requiredArgument = RequiredProperty<string>.CreateValid(argument, "trente-trois");
            // Exercise
            string value = requiredArgument;
            // Verify
            Check.That(value).IsEqualTo("trente-trois");
        }

        [Fact]
        public void throw_an_exception_when_implicitly_converted_while_invalid() {
            // Setup
            Argument<int>            argument         = new("argh", 33);
            RequiredProperty<string> requiredArgument = RequiredProperty<string>.CreateInvalid(argument);
            // Exercise & verify
            Check.ThatCode(() => {
                      // ReSharper disable once UnusedVariable
                      string value = requiredArgument;
                  })
                 .Throws<PropertyException>()
                 .WithMessage(ExceptionMessage.Property_ValueIsInvalid);
        }

    }

}