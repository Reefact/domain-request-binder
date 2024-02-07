#region Usings declarations

using System.Diagnostics;

#endregion

namespace Reefact.FluentRequestBinder;

/// <summary>
///     Represents an argument to validate.
/// </summary>
/// <typeparam name="TValue">The type of the argument.</typeparam>
[DebuggerDisplay("{ToString()}")]
public sealed class ReferenceArgument<TValue> : Argument
    where TValue : class {

    #region Statics members declarations

#if !NET8_0
        public static ReferenceArgument<TValue> UnNamed(TValue? argumentValue) {
            return new ReferenceArgument<TValue>(string.Empty, argumentValue);
        }
#endif

    #endregion

    #region Constructors declarations

    /// <summary>
    ///     Initializes a new instance of <see cref="ReferenceArgument{TValue}" />.
    /// </summary>
    /// <param name="name">The name of the argument.</param>
    /// <param name="value">The value of the argument.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public ReferenceArgument(string name, TValue? value) {
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
    public ReferenceArgument<TValue> AppendPrefix(string? prefix) {
        if (prefix == null) { return this; }

        string prefixedName = $"{prefix}.{Name}";

        return new ReferenceArgument<TValue>(prefixedName, Value);
    }

}