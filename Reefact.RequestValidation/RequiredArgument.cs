#region Usings declarations

using System;
using System.Diagnostics;

#endregion

namespace Reefact.RequestValidation {

    internal static class RequiredArgument {

        #region Statics members declarations

        public static RequiredArgument<TOutput> CreateValid<TOutput>(string argName, object argValue, TOutput convertedArgValue) {
            if (argName == null) { throw new ArgumentNullException(nameof(argName)); }

            return new RequiredArgument<TOutput>(argName, argValue, convertedArgValue, true);
        }

        public static RequiredArgument<TOutput> CreateInvalid<TOutput>(string argName, object argValue) {
            if (argName == null) { throw new ArgumentNullException(nameof(argName)); }

            return new RequiredArgument<TOutput>(argName, argValue, default, false);
        }

        #endregion

    }

    [DebuggerDisplay("{ToString()}")]
    public sealed class RequiredArgument<TConvertedValue> {

        public static implicit operator TConvertedValue(RequiredArgument<TConvertedValue> RequiredArg) {
            if (RequiredArg == null) { throw new ArgumentNullException(nameof(RequiredArg)); }

            return RequiredArg.Value;
        }

        #region Fields declarations

        private readonly TConvertedValue _value;

        #endregion

        #region Constructors declarations

        internal RequiredArgument(string argName, object originalValue, TConvertedValue convertedArgValue, bool isValid) {
            if (argName == null) { throw new ArgumentNullException(nameof(argName)); }
            if (isValid && convertedArgValue == null) { throw new ArgumentException("A required argument could not be valid if argument original value is null.", nameof(convertedArgValue)); }
            if (isValid && convertedArgValue == null) { throw new ArgumentException("A required argument could not be valid if converted argument value is null.", nameof(convertedArgValue)); }

            Name          = argName;
            OriginalValue = originalValue;
            _value        = convertedArgValue;
            IsValid       = isValid;
        }

        #endregion

        public bool IsValid { get; }

        public TConvertedValue Value {
            get {
                if (!IsValid) { throw new InvalidOperationException("Argument is not valid."); }

                return _value;
            }
        }
        public string Name          { get; }
        public object OriginalValue { get; }

        /// <inheritdoc />
        public override string ToString() {
            return (IsValid ? _value?.ToString() : OriginalValue?.ToString()) ?? string.Empty;
        }

    }

}