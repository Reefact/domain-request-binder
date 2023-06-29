#region Usings declarations

using System;

#endregion

namespace Reefact.FluentRequestBinder.UnitTests.__forTesting {

    internal sealed class Role : IEquatable<Role> {

        public static bool operator ==(Role? left, Role? right) {
            return Equals(left, right);
        }

        public static bool operator !=(Role? left, Role? right) {
            return !Equals(left, right);
        }

        #region Constructors declarations

        public Role(string id, string name) {
            if (id is null) { throw new ArgumentNullException(nameof(id)); }
            if (name is null) { throw new ArgumentNullException(nameof(name)); }

            Id   = id;
            Name = name;
        }

        #endregion

        public string Id   { get; }
        public string Name { get; }

        /// <inheritdoc />
        public bool Equals(Role? other) {
            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return Id == other.Id && Name == other.Name;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj) {
            return ReferenceEquals(this, obj) || (obj is Role other && Equals(other));
        }

        /// <inheritdoc />
        public override int GetHashCode() {
            return HashCode.Combine(Id, Name);
        }

    }

}