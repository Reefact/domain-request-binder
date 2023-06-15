#region Usings declarations

using System;
using System.Diagnostics;

#endregion

namespace Reefact.RequestValidation {

    [DebuggerDisplay("{ToString()}")]
    public sealed class ValidationError : IEquatable<ValidationError> {

        public static bool operator ==(ValidationError left, ValidationError right) {
            return Equals(left, right);
        }

        public static bool operator !=(ValidationError left, ValidationError right) {
            return !Equals(left, right);
        }

        #region Constructors declarations

        public ValidationError(string fieldName, string errorMessage) {
            if (fieldName    == null) { throw new ArgumentNullException(nameof(fieldName)); }
            if (errorMessage == null) { throw new ArgumentNullException(nameof(errorMessage)); }

            FieldName    = fieldName;
            ErrorMessage = errorMessage;
        }

        #endregion

        public string FieldName    { get; }
        public string ErrorMessage { get; }

        /// <inheritdoc />
        public bool Equals(ValidationError other) {
            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return FieldName == other.FieldName && ErrorMessage == other.ErrorMessage;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) {
            return ReferenceEquals(this, obj) || (obj is ValidationError other && Equals(other));
        }

        /// <inheritdoc />
        public override int GetHashCode() {
            unchecked {
                return (FieldName.GetHashCode() * 397) ^ ErrorMessage.GetHashCode();
            }
        }

        /// <inheritdoc />
        public override string ToString() {
            return $"{FieldName}: {ErrorMessage}";
        }

    }

}