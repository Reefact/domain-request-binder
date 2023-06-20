#region Usings declarations

using System;

#endregion

namespace Reefact.RequestValidation.UnitTests.__forTesting {

    internal static class TemperatureConverter {

        #region Statics members declarations

        public static Temperature Convert(RequestConverter<Temperature_v1> convert) {
            if (convert is null) { throw new ArgumentNullException(nameof(convert)); }

            RequiredArgument<double>          temperatureValue = convert.SimpleProperty(e => e.Value).AsRequired(double.Parse);
            RequiredArgument<TemperatureUnit> temperatureUnit  = convert.SimpleProperty(e => e.Unit).AsRequired(TemperatureUnitConverter.Convert);
            convert.AssertHasNoError();

            return new Temperature(temperatureValue, temperatureUnit);
        }

        #endregion

    }

}