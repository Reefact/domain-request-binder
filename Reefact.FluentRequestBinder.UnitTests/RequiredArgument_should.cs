using System;
using System.Diagnostics.CodeAnalysis;

using NFluent;

using Xunit;

namespace Reefact.FluentRequestBinder.UnitTests {

    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public class RequiredArgument_should {

        [Fact]
        public void Test() {
            // Setup
            const string argName        = "HiddenInnerWisdom";
            const string originalValue  = "42";
            const int    convertedValue = 42;
            // Exercise
            RequiredProperty<int> requiredArgument = RequiredProperty<int>.CreateValid(argName, originalValue, convertedValue);
            // Verify
            Check.That(requiredArgument.ArgumentName).IsEqualTo(argName);
            Check.That(requiredArgument.IsValid).IsEqualTo(true);
            Check.That(requiredArgument.ArgumentValue).IsEqualTo(originalValue);
            Check.That(requiredArgument.Value).IsEqualTo(convertedValue);
        }

        [Fact]
        public void throw_an_exception_trying_to_get_the_value_of_an_invalid_argument() {
            // Setup
            const string argName       = "SarahConnor";
            const string originalValue = "yes";
            // Exercise
            RequiredProperty<bool> requiredArgument = RequiredProperty<bool>.CreateInvalid(argName, originalValue);
            // Verify
            Check.That(requiredArgument.ArgumentName).IsEqualTo(argName);
            Check.That(requiredArgument.IsValid).IsEqualTo(false);
            Check.That(requiredArgument.ArgumentValue).IsEqualTo(originalValue);
            Check.ThatCode(() => requiredArgument.Value).Throws<InvalidOperationException>();
        }

        [Fact]
        public void return_the_string_representation_of_the_converted_value_if_is_valid() {
            // Setup
            var requiredArgument = RequiredProperty<string>.CreateValid("argh", 33, "trente-trois");
            // Exercise
            string output = requiredArgument.ToString();
            // Verify
            Check.That(output).IsEqualTo("trente-trois");
        }

        [Fact]
        public void return_the_string_representation_of_the_original_value_if_is_not_valid() {
            // Setup
            var requiredArgument = RequiredProperty<int>.CreateInvalid("argh", 33);
            // Exercise
            string output = requiredArgument.ToString();
            // Verify
            Check.That(output).IsEqualTo("33");
        }

        [Fact]
        public void never_accept_null_arg_name() {
            // Exercise & verify
            Check.ThatCode(() => RequiredProperty<int>.CreateInvalid(null!, "trente-trois"))
                 .Throws<ArgumentNullException>();
            Check.ThatCode(() => RequiredProperty<int>.CreateValid(null!, "trente-trois", 33))
                 .Throws<ArgumentNullException>();
        }

        [Fact]
        public void not_accept_a_null_arg_value_when_is_valid() {
            // Exercise & verify
            Check.ThatCode(() => RequiredProperty<string>.CreateValid("anyName", null!, "trente-trois"))
                 .Throws<ArgumentException>()
                 .WithMessage("A required property could not be valid if argument value is null. (Parameter 'argumentValue')");
        }

        [Fact]
        public void not_accept_a_null_converted_arg_value_when_is_valid() {
            // Exercise & verify
            Check.ThatCode(() => RequiredProperty<string>.CreateValid("anyName", "33", null!))
                 .Throws<ArgumentException>()
                 .WithMessage("A required property could not be valid if property value is null. (Parameter 'propertyValue')");
        }

        [Fact]
        public void return_its_value_when_implicitly_converted_while_valid() {
            // Setup
            var requiredArgument = RequiredProperty<string>.CreateValid("argh", 33, "trente-trois");
            // Exercise
            string value = requiredArgument;
            // Verify
            Check.That(value).IsEqualTo("trente-trois");
        }

        [Fact]
        public void throw_an_exception_when_implicitly_converted_while_invalid() {
            // Setup
            var requiredArgument = RequiredProperty<string>.CreateInvalid("argh", 33);
            // Exercise & verify
            Check.ThatCode(() => {
                      // ReSharper disable once UnusedVariable
                      string value = requiredArgument;
                  })
                 .Throws<InvalidOperationException>().WithMessage("Property is not valid.");
        }

    }

}