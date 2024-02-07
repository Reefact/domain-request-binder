#region Usings declarations

#endregion

namespace Reefact.FluentRequestBinder {

    /// <summary>
    ///     Represents an argument to validate.
    /// </summary>
    public interface Argument {

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

#if NET8_0
        public static ReferenceArgument<TValue> UnNamed<TValue>(TValue? argumentValue)
            where TValue : class {
            return new ReferenceArgument<TValue>(string.Empty, argumentValue);
        }

        public static ValueArgument<TValue> UnNamed<TValue>(TValue? argumentValue)
            where TValue : struct {
            return new ValueArgument<TValue>(string.Empty, argumentValue);
        }
#endif

    }

}