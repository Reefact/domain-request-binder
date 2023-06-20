#region Usings declarations

using System;

#endregion

namespace Reefact.FluentRequestBinder {

    internal static class OptionalArgument {

        #region Statics members declarations

        public static OptionalArgument<TOutput> CreateValid<TOutput>(string argName, object argValue, TOutput convertedArgValue) {
            if (argName == null) { throw new ArgumentNullException(nameof(argName)); }

            return new OptionalArgument<TOutput>(argName, argValue, convertedArgValue, true);
        }

        public static OptionalArgument<TOutput> CreateInvalid<TOutput>(string argName, object argValue) {
            if (argName == null) { throw new ArgumentNullException(nameof(argName)); }

            return new OptionalArgument<TOutput>(argName, argValue, default, false);
        }

        #endregion

    }

    public sealed class OptionalArgument<TConvertedValue> {

        public static implicit operator TConvertedValue(OptionalArgument<TConvertedValue> OptionalArgument) {
            if (OptionalArgument == null) { throw new ArgumentNullException(nameof(OptionalArgument)); }

            if (!OptionalArgument.IsValid) { throw new InvalidOperationException($"Value '{OptionalArgument.OriginalValue}' of argument '{OptionalArgument.Name}' is not valid."); }

            return OptionalArgument.Value;
        }

        #region Fields declarations

        private readonly TConvertedValue _value;

        #endregion

        #region Constructors declarations

        internal OptionalArgument(string argName, object originalValue, TConvertedValue convertedArgValue, bool isValid) {
            if (argName == null) { throw new ArgumentNullException(nameof(argName)); }

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

    }

}