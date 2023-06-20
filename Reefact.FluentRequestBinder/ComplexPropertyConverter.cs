#region Usings declarations

using System;

#endregion

namespace Reefact.FluentRequestBinder {

    public sealed class ComplexPropertyConverter<TInput> {

        #region Fields declarations

        private readonly ArgumentsConverter _argumentsValidator;
        private readonly TInput             _argValue;
        private readonly string             _argName;

        #endregion

        #region Constructors declarations

        public ComplexPropertyConverter(ArgumentsConverter argumentsValidator, string argName, TInput argValue) {
            if (argumentsValidator is null) { throw new ArgumentNullException(nameof(argumentsValidator)); }
            if (argName is null) { throw new ArgumentNullException(nameof(argName)); }

            _argumentsValidator = argumentsValidator;
            _argName            = argName;
            _argValue           = argValue;
        }

        #endregion

        public RequiredArgument<TOutput> AsRequired<TOutput>(Func<RequestConverter<TInput>, TOutput> convert) {
            try {
                TOutput output = convert(new RequestConverter<TInput>(_argValue, _argumentsValidator.Options, _argName));

                return RequiredArgument.CreateValid(_argName, _argValue, output);
            } catch (BadRequestException ex) {
                _argumentsValidator.AddErrors(ex.ValidationErrors);

                return RequiredArgument.CreateInvalid<TOutput>(_argName, _argValue);
            }
        }

    }

}