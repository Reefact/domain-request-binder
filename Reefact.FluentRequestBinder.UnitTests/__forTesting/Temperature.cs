namespace Reefact.FluentRequestBinder.UnitTests.__forTesting {

    public sealed class Temperature {

        #region Constructors declarations

        public Temperature(double value, TemperatureUnit unit) {
            Value = value;
            Unit  = unit;
        }

        #endregion

        public double          Value { get; }
        public TemperatureUnit Unit  { get; }

    }

}