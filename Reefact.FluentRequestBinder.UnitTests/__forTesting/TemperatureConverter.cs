#region Usings declarations

using System;

#endregion

namespace Reefact.FluentRequestBinder.UnitTests.__forTesting {

    internal static class TemperatureConverter {

        #region Statics members declarations

        public static Temperature Convert(RequestConverter<Temperature_v1> convert) {
            if (convert is null) { throw new ArgumentNullException(nameof(convert)); }

            RequiredProperty<double>          temperatureValue = convert.SimpleProperty(e => e.Value).AsRequired(double.Parse!);
            RequiredProperty<TemperatureUnit> temperatureUnit  = convert.SimpleProperty(e => e.Unit).AsRequired(TemperatureUnitConverter.Convert!);
            convert.AssertHasNoError();

            return new Temperature(temperatureValue, temperatureUnit);
        }

        #endregion

    }

}