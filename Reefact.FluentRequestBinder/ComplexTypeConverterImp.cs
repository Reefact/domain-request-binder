#region Usings declarations

using System;

#endregion

namespace Reefact.FluentRequestBinder {

    internal sealed class ComplexPropertyConverterImp<THandledException, TInput> : ComplexPropertyConverter<TInput>
        where THandledException : ApplicationException {

        #region Fields declarations

        private readonly ArgumentsValidator<THandledException> _argumentsValidator;
        private readonly TInput                                _argValue;
        private readonly string                                _argName;

        #endregion

        #region Constructors declarations

        public ComplexPropertyConverterImp(ArgumentsValidator<THandledException> argumentsValidator, string argName, TInput argValue) {
            if (argumentsValidator is null) { throw new ArgumentNullException(nameof(argumentsValidator)); }
            if (argName is null) { throw new ArgumentNullException(nameof(argName)); }

            _argumentsValidator = argumentsValidator;
            _argName            = argName;
            _argValue           = argValue;
        }

        #endregion

        /// <inheritdoc />
        public RequiredArgument<TOutput> AsRequired<TOutput>(Func<RequestConverter<TInput>, TOutput> convert) {
            try {
                TOutput output = convert(new RequestConverter<THandledException, TInput>(_argValue, new DefaultPropertyNameProvider(), _argName));

                return RequiredArgument.CreateValid(_argName, _argValue, output);
            } catch (BadRequestException ex) {
                _argumentsValidator.AddErrors(ex.ValidationErrors);

                return RequiredArgument.CreateInvalid<TOutput>(_argName, _argValue);
            }
        }

    }

}