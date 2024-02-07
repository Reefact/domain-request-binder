#region Usings declarations

using System.Diagnostics;

#endregion

namespace Reefact.FluentRequestBinder {

    /// <summary>
    ///     Represents an argument to validate.
    /// </summary>
    public interface Argument {

        #region Statics members declarations

#if NET8_0
        public static Argument<TValue> UnNamed<TValue>(TValue? argumentValue) {
            return new Argument<TValue>(string.Empty, argumentValue);
        }
#endif

        #endregion

        /// <summary>
        ///     The name of the argument.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     The value of the argument.
        /// </summary>
        public object? Value { get; }

        /// <summary>
        ///     Gets if argument is missing ; meaning <see cref="Value" /> is <c>null</c>.
        /// </summary>
        bool IsMissing { get; }

        /// <summary>
        ///     Gets if argument is fulfilled ; meaning <see cref="Value" /> is not <c>null</c>.
        /// </summary>
        bool IsFulfilled { get; }

    }

    /// <summary>
    ///     Represents an argument to validate.
    /// </summary>
    /// <typeparam name="TValue">The type of the argument.</typeparam>
    [DebuggerDisplay("{ToString()}")]
    public sealed class Argument<TValue> : Argument {

        #region Statics members declarations

#if !NET8_0
        public static Argument<TValue> UnNamed(TValue? argumentValue) {
            return new Argument<TValue>(string.Empty, argumentValue);
        }
#endif

        #endregion

        #region Constructors declarations

        /// <summary>
        ///     Initializes a new instance of <see cref="Argument{TValue}" />.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The value of the argument.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public Argument(string name, TValue? value) {
            if (name is null) { throw new ArgumentNullException(nameof(name)); }

            Name  = name;
            Value = value;
        }

        #endregion

        /// <summary>
        ///     The name of the argument.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     The value of the argument.
        /// </summary>
        public TValue? Value { get; }

        /// <inheritdoc />
        public bool IsMissing => Value == null;

        /// <inheritdoc />
        public bool IsFulfilled => Value != null;

        /// <inheritdoc />
        object? Argument.Value => Value;

        /// <inheritdoc />
        public override string ToString() {
            return Name;
        }

        /// <summary>
        ///     Appends a prefixed to the name of current argument.
        /// </summary>
        /// <param name="prefix">The name prefix to append.</param>
        /// <returns>A prefixed argument based on the original value.</returns>
        public Argument<TValue> AppendPrefix(string? prefix) {
            if (prefix == null) { return this; }

            string prefixedName = $"{prefix}.{Name}";

            return new Argument<TValue>(prefixedName, Value);
        }

    }

}