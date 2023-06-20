#region Usings declarations

using System;
using System.Diagnostics;

#endregion

namespace Reefact.RequestValidation.UnitTests.__forTesting {

    [DebuggerDisplay("{ToString()}")]
    public sealed class AnyId : IEquatable<AnyId> {

        #region Statics members declarations

        public static AnyId From(Guid? guid) {
            if (guid == null) { return new AnyId(Guid.Empty); }

            return new AnyId(guid.Value);
        }

        public static AnyId From(Guid guid) {
            return new AnyId(guid);
        }

        #endregion

        public static bool operator ==(AnyId left, AnyId right) {
            return Equals(left, right);
        }

        public static bool operator !=(AnyId left, AnyId right) {
            return !Equals(left, right);
        }

        #region Fields declarations

        private readonly Guid _value;

        #endregion

        #region Constructors declarations

        private AnyId(Guid guid) {
            _value = guid;
        }

        #endregion

        /// <inheritdoc />
        public bool Equals(AnyId other) {
            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return _value.Equals(other._value);
        }

        /// <inheritdoc />
        public override bool Equals(object obj) {
            return ReferenceEquals(this, obj) || (obj is AnyId other && Equals(other));
        }

        /// <inheritdoc />
        public override int GetHashCode() {
            return _value.GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString() {
            return _value.ToString();
        }

    }

}