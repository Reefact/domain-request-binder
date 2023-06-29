#region Using declarations

using System;
using System.Diagnostics;

#endregion

namespace Reefact.FluentRequestBinder.UnitTests.__forTesting {

    [DebuggerDisplay("{ToString()}")]
    internal sealed class Temperature : IEquatable<Temperature> {

        public static bool operator ==(Temperature? left, Temperature? right) {
            return Equals(left, right);
        }

        public static bool operator !=(Temperature? left, Temperature? right) {
            return !Equals(left, right);
        }

        #region Constructors declarations

        public Temperature(double value, TemperatureUnit unit) {
            Value = value;
            Unit  = unit;
        }

        #endregion

        public double          Value { get; }
        public TemperatureUnit Unit  { get; }

        /// <inheritdoc />
        public bool Equals(Temperature? other) {
            if (ReferenceEquals(null, other)) {
                return false;
            }

            if (ReferenceEquals(this, other)) {
                return true;
            }

            return Value.Equals(other.Value) && Unit == other.Unit;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj) {
            return ReferenceEquals(this, obj) || (obj is Temperature other && Equals(other));
        }

        /// <inheritdoc />
        public override int GetHashCode() {
            return HashCode.Combine(Value, (int)Unit);
        }

        /// <inheritdoc />
        public override string ToString() {
            string unit = Unit == TemperatureUnit.Celsius ? "C" : "F";

            return $"{Value} °{unit}";
        }

    }

}