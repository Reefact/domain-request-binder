#region Usings declarations

using System;
using System.Linq;

using NFluent;

using Reefact.FluentRequestBinder.UnitTests.__forTesting;

using Xunit;

#endregion

namespace Reefact.FluentRequestBinder.UnitTests {

    public class RequestValidator_should {

        #region Statics members declarations

        private static PromoteMember_v1 CreatePromoteMemberCommandWithTemperatureUnitError() {
            var request = new PromoteMember_v1 {
                TeamId       = Guid.NewGuid(),
                MemberUtCode = "UT3245",
                Temperature = new Temperature_v1 {
                    Value = "37",
                    Unit  = "cecius"
                }
            };

            return request;
        }

        private static PromoteMember_v1 CreateValidPromoteMemberCommand() {
            var request = new PromoteMember_v1 {
                TeamId       = Guid.NewGuid(),
                MemberUtCode = "UT3245",
                Temperature = new Temperature_v1 {
                    Value = "37",
                    Unit  = "celcius"
                }
            };

            return request;
        }

        private static PromoteMember_v1 CreatePromoteMemberCommandWithNullMember() {
            var request = new PromoteMember_v1 {
                TeamId = Guid.NewGuid(),
                Temperature = new Temperature_v1 {
                    Value = "37",
                    Unit  = "celcius"
                }
            };

            return request;
        }

        #endregion

        [Fact]
        public void test() {
            // Setup
            PromoteMember_v1                   requestWithError = CreatePromoteMemberCommandWithTemperatureUnitError();
            RequestConverter<PromoteMember_v1> bind          = Bind.PropertiesOf(requestWithError);
            // Exercise
            RequiredArgument<AnyId>       teamId      = bind.SimpleProperty(c => c.TeamId).AsRequired(AnyId.From);
            RequiredArgument<string>      utCode      = bind.SimpleProperty(c => c.MemberUtCode).AsRequired();
            RequiredArgument<Temperature> temperature = bind.ComplexProperty(c => c.Temperature).AsRequired(TemperatureConverter.Convert);
            // Verify
            Check.That(bind.ErrorCount).IsEqualTo(1);
            ValidationError error = bind.GetErrors().First();
            Check.That(error.FieldName).IsEqualTo("Temperature.Unit");
            Check.That(error.ErrorMessage).IsEqualTo("Unknown temperature unit.");
        }

        [Fact]
        public void test2() {
            // Setup
            PromoteMember_v1                   requestWithError = CreateValidPromoteMemberCommand();
            RequestConverter<PromoteMember_v1> bind          = Bind.PropertiesOf(requestWithError);
            // Exercise
            RequiredArgument<AnyId>       teamId      = bind.SimpleProperty(c => c.TeamId).AsRequired(AnyId.From);
            RequiredArgument<string>      utCode      = bind.SimpleProperty(c => c.MemberUtCode).AsRequired();
            RequiredArgument<Temperature> temperature = bind.ComplexProperty(c => c.Temperature).AsRequired(TemperatureConverter.Convert);
            // Verify
            Check.That(bind.HasError).IsFalse();
            Check.ThatCode(() => {
                var command = new PromoteMember(teamId, utCode, temperature);
            }).DoesNotThrow();
        }

        [Fact]
        public void test3() {
            // Setup
            PromoteMember_v1                   requestWithError = CreatePromoteMemberCommandWithNullMember();
            RequestConverter<PromoteMember_v1> bind          = Bind.PropertiesOf(requestWithError);
            // Exercise
            RequiredArgument<AnyId>       teamId      = bind.SimpleProperty(c => c.TeamId).AsRequired(AnyId.From);
            RequiredArgument<string>      utCode      = bind.SimpleProperty(c => c.MemberUtCode!).AsRequired();
            RequiredArgument<Temperature> temperature = bind.ComplexProperty(c => c.Temperature).AsRequired(TemperatureConverter.Convert);
            // Verify
            Check.That(bind.ErrorCount).IsEqualTo(1);
            Check.That(utCode.IsValid).IsFalse();
            ValidationError utCodeError = bind.GetErrors().First();
            Check.That(utCodeError.FieldName).IsEqualTo("MemberUtCode");
            Check.That(utCodeError.ErrorMessage).IsEqualTo("Argument is required.");
        }

        #region Nested types declarations

        #endregion

    }

}