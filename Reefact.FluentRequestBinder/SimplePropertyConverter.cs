#region Usings declarations

using System;

#endregion

namespace Reefact.FluentRequestBinder {

    public sealed class SimplePropertyConverter<TInput> {

        #region Fields declarations

        private readonly ArgumentsConverter _argumentsValidator;
        private readonly string             _argName;
        private readonly TInput             _argValue;

        #endregion

        #region Constructors declarations

        public SimplePropertyConverter(ArgumentsConverter argumentsValidator, string argName, TInput argValue) {
            if (argumentsValidator is null) { throw new ArgumentNullException(nameof(argumentsValidator)); }
            if (argName is null) { throw new ArgumentNullException(nameof(argName)); }

            _argumentsValidator = argumentsValidator;
            _argName            = argName;
            _argValue           = argValue;
        }

        #endregion

        public RequiredArgument<TOutput> AsRequired<TOutput>(Func<TInput, TOutput> convert) {
            return _argumentsValidator.ConvertRequired(_argName, _argValue, convert);
        }
        
        public RequiredArgument<TInput> AsRequired() {
            return _argumentsValidator.IsRequired(_argName, _argValue);
        }

    }

}