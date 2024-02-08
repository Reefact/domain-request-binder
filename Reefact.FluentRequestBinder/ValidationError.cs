#region Usings declarations

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

#endregion

namespace Reefact.FluentRequestBinder {

    /// <summary>
    ///     Represents an error that has occurred during binding of a request argument.
    /// </summary>
    [DebuggerDisplay("{ToString()}")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public sealed class ValidationError : IEquatable<ValidationError> {

        #region Statics members declarations

        public static ValidationError ArgumentIsRequired(Argument argument) {
            if (argument is null) { throw new ArgumentNullException(nameof(argument)); }

            return new ValidationError(argument.Name, "Argument is required.");
        }

        #endregion

        /// <summary>Determines whether the specified <see cref="ValidationError">validation errors</see> instances are equals.</summary>
        /// <param name="left">The first <see cref="ValidationError">validation error</see> to compare.</param>
        /// <param name="right">The second <see cref="ValidationError">validation error</see> to compare.</param>
        /// <returns>
        ///     <c>true</c> if the <see cref="ValidationError">validation errors</see> are considered equal; otherwise
        ///     <c>false</c>. If both <paramref name="left" /> and <paramref name="right" /> are null, the method returns
        ///     <c>true</c>.
        /// </returns>
        public static bool operator ==(ValidationError left, ValidationError right) {
            return Equals(left, right);
        }

        /// <summary>Determines whether the specified <see cref="ValidationError">validation errors</see> instances are not equals.</summary>
        /// <param name="left">The first <see cref="ValidationError">validation error</see> to compare.</param>
        /// <param name="right">The second <see cref="ValidationError">validation error</see> to compare.</param>
        /// <returns>
        ///     <c>true</c> if the <see cref="ValidationError">validation errors</see> are considered not equal; otherwise
        ///     <c>false</c>. If both <paramref name="left" /> and <paramref name="right" /> are null, the method returns
        ///     <c>false</c>.
        /// </returns>
        public static bool operator !=(ValidationError left, ValidationError right) {
            return !Equals(left, right);
        }

        #region Constructors declarations

        /// <summary>
        ///     Instantiates a new <see cref="ValidationError">validation error</see>.
        /// </summary>
        /// <param name="argumentName">The name of the argument causing the validation error.</param>
        /// <param name="errorMessage">The validation error message.</param>
        /// <exception cref="ArgumentNullException">
        ///     Parameters <paramref name="argumentName" /> and <paramref name="errorMessage" />
        ///     cannot be null.
        /// </exception>
        public ValidationError(string argumentName, string errorMessage) {
            if (argumentName == null) { throw new ArgumentNullException(nameof(argumentName)); }
            if (errorMessage == null) { throw new ArgumentNullException(nameof(errorMessage)); }

            ArgumentName = argumentName;
            ErrorMessage = errorMessage;
        }

        #endregion

        /// <summary>The name of the argument causing the validation error.</summary>
        public string ArgumentName { get; }

        /// <summary>
        ///     The validation error message.
        /// </summary>
        public string ErrorMessage { get; }

        /// <inheritdoc />
        public bool Equals(ValidationError? other) {
            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return ArgumentName == other.ArgumentName && ErrorMessage == other.ErrorMessage;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj) {
            return ReferenceEquals(this, obj) || (obj is ValidationError other && Equals(other));
        }

        /// <inheritdoc />
        public override int GetHashCode() {
            unchecked {
                return (ArgumentName.GetHashCode() * 397) ^ ErrorMessage.GetHashCode();
            }
        }

        /// <inheritdoc />
        public override string ToString() {
            return $"{ArgumentName}: {ErrorMessage}";
        }

    }

}