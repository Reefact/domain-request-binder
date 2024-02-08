#region Usings declarations

using System.Diagnostics;

#endregion

namespace Reefact.FluentRequestBinder;

/// <summary>
///     Represents an argument to bind to a property.
/// </summary>
public interface Argument {

    #region Statics members declarations

#if NET8_0
    public static Argument<TValue> UnNamed<TValue>(TValue? argumentValue) {
        return new Argument<TValue>(string.Empty, argumentValue);
    }

#endif

    #endregion

    /// <summary>Gets the name of the argument.</summary>
    string Name { get; }

    /// <summary>Indicates that argument is missing.</summary>
    /// <seealso cref="IsFulfilled" />
    bool IsMissing { get; }

    /// <summary>Indicates that argument is fulfilled.</summary>
    /// <seealso cref="IsMissing" />
    bool IsFulfilled { get; }

    object? Value { get; }

}

/// <summary>
///     Represents an argument to bind to a property.
/// </summary>
/// <typeparam name="TValue">The type of the argument.</typeparam>
[DebuggerDisplay("{ToString()}")]
public sealed class Argument<TValue> : Argument {

    #region Statics members declarations

#if !NET8_0
    internal static Argument<TValue> UnNamed(TValue? argumentValue) {
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

    /// <inheritdoc />
    public string Name { get; }

    /// <summary>
    ///     Gets the value of the argument.
    /// </summary>
    public TValue? Value { get; }

    /// <inheritdoc />
    object? Argument.Value => Value;

    /// <inheritdoc />
    public bool IsMissing => Value == null;

    /// <inheritdoc />
    public bool IsFulfilled => Value != null;

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