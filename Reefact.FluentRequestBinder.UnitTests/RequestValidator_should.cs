#region Usings declarations

using NFluent;

using Reefact.FluentRequestBinder.UnitTests.__forTesting;

#endregion

namespace Reefact.FluentRequestBinder.UnitTests {

    public class RequestValidator_should {

        #region Statics members declarations

        private static PromoteMember_v1 CreatePromoteMemberCommandWithTemperatureUnitError() {
            PromoteMember_v1 request = new() {
                TeamId       = Guid.NewGuid(),
                MemberUtCode = "UT3245",
                Temperature = new Temperature_v1 {
                    Value = "37",
                    // ReSharper disable once StringLiteralTypo
                    Unit = "cecius"
                }
            };

            return request;
        }

        private static PromoteMember_v1 CreateValidPromoteMemberCommand() {
            PromoteMember_v1 request = new() {
                TeamId       = Guid.NewGuid(),
                MemberUtCode = "UT3245",
                Temperature = new Temperature_v1 {
                    Value = "37",
                    Unit  = "celsius"
                }
            };

            return request;
        }

        private static PromoteMember_v1 CreatePromoteMemberCommandWithNullMember() {
            PromoteMember_v1 request = new() {
                TeamId = Guid.NewGuid(),
                Temperature = new Temperature_v1 {
                    Value = "37",
                    Unit  = "celsius"
                }
            };

            return request;
        }

        #endregion

        [Fact]
        public void test() {
            // Setup
            PromoteMember_v1                   requestWithError = CreatePromoteMemberCommandWithTemperatureUnitError();
            RequestConverter<PromoteMember_v1> bind             = Bind.PropertiesOf(requestWithError);
            // Exercise
            bind.SimpleProperty(c => c.TeamId).AsRequired(AnyId.From);
            bind.SimpleProperty(c => c.MemberUtCode).AsRequired();
            bind.ComplexProperty(c => c.Temperature!).AsRequired(TemperatureConverter.Convert);
            // Verify
            Check.That(bind.ErrorCount).IsEqualTo(1);
            ValidationError error = bind.GetErrors().First();
            Check.That(error.ArgumentName).IsEqualTo("Temperature.Unit");
            Check.That(error.ErrorMessage).IsEqualTo("Unknown temperature unit.");
        }

        [Fact]
        public void test2() {
            // Setup
            PromoteMember_v1                   requestWithError = CreateValidPromoteMemberCommand();
            RequestConverter<PromoteMember_v1> bind             = Bind.PropertiesOf(requestWithError);
            // Exercise
            RequiredReferenceProperty<AnyId>       teamId      = bind.SimpleProperty(c => c.TeamId).AsRequired(AnyId.From);
            RequiredReferenceProperty<string>      utCode      = bind.SimpleProperty(c => c.MemberUtCode!).AsRequired();
            RequiredReferenceProperty<Temperature> temperature = bind.ComplexProperty(c => c.Temperature!).AsRequired(TemperatureConverter.Convert);
            // Verify
            Check.That(bind.HasError).IsFalse();
            Check.ThatCode(() => {
                // ReSharper disable once ObjectCreationAsStatement
                new PromoteMember(teamId, utCode, temperature);
            }).DoesNotThrow();
        }

        [Fact]
        public void test3() {
            // Setup
            PromoteMember_v1                   requestWithError = CreatePromoteMemberCommandWithNullMember();
            RequestConverter<PromoteMember_v1> bind             = Bind.PropertiesOf(requestWithError);
            // Exercise
            bind.SimpleProperty(c => c.TeamId).AsRequired(AnyId.From);
            RequiredReferenceProperty<string> utCode = bind.SimpleProperty(c => c.MemberUtCode!).AsRequired();
            bind.ComplexProperty(c => c.Temperature!).AsRequired(TemperatureConverter.Convert);
            // Verify
            Check.That(bind.ErrorCount).IsEqualTo(1);
            Check.That(utCode.IsValid).IsFalse();
            ValidationError utCodeError = bind.GetErrors().First();
            Check.That(utCodeError.ArgumentName).IsEqualTo("MemberUtCode");
            Check.That(utCodeError.ErrorMessage).IsEqualTo("Argument is required.");
        }

    }

}