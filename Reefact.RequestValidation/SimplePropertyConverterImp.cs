#region Usings declarations

using System;

#endregion

namespace Reefact.RequestValidation {

    internal sealed class SimplePropertyConverterImp<THandledException, TInput> : SimplePropertyConverter<TInput>
        where THandledException : ApplicationException {

        #region Fields declarations

        private readonly ArgumentsValidator<THandledException> _argumentsValidator;
        private readonly string                                _argName;
        private readonly TInput                                _argValue;

        #endregion

        #region Constructors declarations

        public SimplePropertyConverterImp(ArgumentsValidator<THandledException> argumentsValidator, string argName, TInput argValue) {
            if (argumentsValidator is null) { throw new ArgumentNullException(nameof(argumentsValidator)); }
            if (argName is null) { throw new ArgumentNullException(nameof(argName)); }

            _argumentsValidator = argumentsValidator;
            _argName            = argName;
            _argValue           = argValue;
        }

        #endregion

        /// <inheritdoc />
        public RequiredArgument<TOutput> AsRequired<TOutput>(Func<TInput, TOutput> convert) {
            return _argumentsValidator.ConvertRequired(_argName, _argValue, convert);
        }

        /// <inheritdoc />
        public RequiredArgument<TInput> AsRequired() {
            return _argumentsValidator.IsRequired(_argName, _argValue);
        }

    }

}