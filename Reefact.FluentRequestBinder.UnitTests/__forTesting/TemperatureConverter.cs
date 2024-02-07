#region Usings declarations

#endregion

namespace Reefact.FluentRequestBinder.UnitTests.__forTesting {

    internal static class TemperatureConverter {

        #region Statics members declarations

        public static Temperature Convert(RequestConverter<Temperature_v1> bind) {
            if (bind is null) { throw new ArgumentNullException(nameof(bind)); }

            RequiredProperty<double>          temperatureValue = bind.SimpleProperty(e => e.Value).AsRequired(double.Parse!);
            RequiredProperty<TemperatureUnit> temperatureUnit  = bind.SimpleProperty(e => e.Unit).AsRequired(TemperatureUnitConverter.Convert!);
            bind.AssertHasNoError();

            return new Temperature(temperatureValue, temperatureUnit);
        }

        #endregion

    }

}