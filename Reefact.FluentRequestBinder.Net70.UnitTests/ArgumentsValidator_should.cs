// ReSharper disable RedundantUsingDirective

#region Usings declarations

using Xunit;

using TestThat = Reefact.FluentRequestBinder.UnitTests;

#endregion

namespace Reefact.FluentRequestBinder.Net70.UnitTests {

    public class ArgumentsValidator_should {

        [Fact]
        public void handle_correctly_the_conversion_of_one_valid_input() {
            TestThat.ArgumentsValidator_should.handle_correctly_the_conversion_of_one_valid_input();
        }

        [Fact]
        public void handle_correctly_the_conversion_of_one_missing_required_input() {
            TestThat.ArgumentsValidator_should.handle_correctly_the_conversion_of_one_missing_required_input();
        }

        [Fact]
        public void handle_correctly_the_failing_conversion_of_one_required_input() {
            TestThat.ArgumentsValidator_should.handle_correctly_the_failing_conversion_of_one_required_input();
        }

        [Fact]
        public void handle_correctly_the_conversion_of_two_valid_input() {
            TestThat.ArgumentsValidator_should.handle_correctly_the_conversion_of_two_valid_input();
        }

        [Fact]
        public void handle_correctly_the_conversion_of_one_valid_input_and_one_missing_required_input() {
            TestThat.ArgumentsValidator_should.handle_correctly_the_conversion_of_one_valid_input_and_one_missing_required_input();
        }

        [Fact]
        public void handle_correctly_the_conversion_of_one_valid_input_and_one_missing_required_input_and_one_failing_conversion() {
            TestThat.ArgumentsValidator_should.handle_correctly_the_conversion_of_one_valid_input_and_one_missing_required_input_and_one_failing_conversion();
        }

    }

}