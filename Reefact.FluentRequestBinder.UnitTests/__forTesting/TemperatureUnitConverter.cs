#region Usings declarations

using System;

#endregion

namespace Reefact.FluentRequestBinder.UnitTests.__forTesting {

    internal static class TemperatureUnitConverter {

        #region Statics members declarations

        public static TemperatureUnit Convert(string input) {
            return input switch {
                "celsius"   => TemperatureUnit.Celcius,
                "fahrenheit" => TemperatureUnit.Farenheit,
                _           => throw new ApplicationException("Unknown temperature unit.")
            };
        }

        #endregion

    }

}