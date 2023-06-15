#region Usings declarations

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

#endregion

namespace Reefact.RequestValidation {

    public interface RequestValidator<TRequest> : ArgumentsValidator {

        RequiredArgument<TOutput> ConvertRequired<TInput, TOutput>(string                             argName,    Expression<Func<TRequest, TInput>> expression, Func<TInput, TOutput> convert);
        RequiredArgument<TOutput> ConvertRequired<TInput, TOutput>(Expression<Func<TRequest, TInput>> expression, Func<TInput, TOutput>              convert);
        OptionalArgument<TOutput> ConvertOptional<TInput, TOutput>(string                             argName,    Expression<Func<TRequest, TInput>> expression, Func<TInput?, TOutput> convert);
        OptionalArgument<TOutput> ConvertOptional<TInput, TOutput>(Expression<Func<TRequest, TInput>> expression, Func<TInput?, TOutput>             convert);
        RequiredArgument<TValue>  IsRequired<TValue>(string                                           argName,    Expression<Func<TRequest, TValue>> expression);
        RequiredArgument<TValue>  IsRequired<TValue>(Expression<Func<TRequest, TValue>>               expression);

    }

    [DebuggerDisplay("{ToString()}")]
    public sealed class RequestValidator<THandledException, TRequest> : RequestValidator<TRequest>
        where THandledException : ApplicationException {

        #region Statics members declarations

        private static PropertyInfo GetPropertyInfo<TObject, TProperty>(Expression<Func<TObject, TProperty>> expression) {
            var body = expression.Body as MemberExpression;
            if (body == null) { throw new InvalidOperationException("Expression body is not of MemberExpression type."); }

            var member = body.Member as PropertyInfo;
            if (member == null) { throw new InvalidOperationException("MemberExpression is not of PropertyInfo type."); }

            return member;
        }

        #endregion

        #region Fields declarations

        private readonly TRequest                              _request;
        private readonly ArgumentsValidator<THandledException> _argumentsValidator = new ArgumentsValidator<THandledException>();

        #endregion

        #region Constructors declarations

        internal RequestValidator([DisallowNull] TRequest request) {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }

            _request = request;
        }

        #endregion

        /// <inheritdoc />
        public bool HasError => _argumentsValidator.HasError;
        /// <inheritdoc />
        public int ErrorCount => _argumentsValidator.ErrorCount;

        /// <inheritdoc />
        public RequiredArgument<TOutput> ConvertRequired<TInput, TOutput>(Expression<Func<TRequest, TInput>> expression, Func<TInput, TOutput> convertArgValue) {
            if (expression      == null) { throw new ArgumentNullException(nameof(expression)); }
            if (convertArgValue == null) { throw new ArgumentNullException(nameof(convertArgValue)); }

            PropertyInfo              propertyInfo  = GetPropertyInfo(expression);
            var                       propertyValue = (TInput?)propertyInfo.GetValue(_request)!;
            RequiredArgument<TOutput> validatedArg  = _argumentsValidator.ConvertRequired(propertyInfo.Name, propertyValue, convertArgValue);

            return validatedArg;
        }

        /// <inheritdoc />
        public RequiredArgument<TOutput> ConvertRequired<TInput, TOutput>(string argName, Expression<Func<TRequest, TInput>> expression, Func<TInput, TOutput> convertArgValue) {
            if (argName is null) { throw new ArgumentNullException(nameof(argName)); }
            if (expression is null) { throw new ArgumentNullException(nameof(expression)); }
            if (convertArgValue is null) { throw new ArgumentNullException(nameof(convertArgValue)); }

            PropertyInfo              propertyInfo  = GetPropertyInfo(expression);
            var                       propertyValue = (TInput?)propertyInfo.GetValue(_request)!;
            RequiredArgument<TOutput> validatedArg  = _argumentsValidator.ConvertRequired(argName, propertyValue, convertArgValue);

            return validatedArg;
        }

        /// <inheritdoc />
        public RequiredArgument<TOutput> ConvertRequired<TInput, TOutput>(string argName, TInput argValue, Func<TInput, TOutput> convertArgValue) {
            return _argumentsValidator.ConvertRequired(argName, argValue, convertArgValue);
        }

        /// <inheritdoc />
        public OptionalArgument<TOutput> ConvertOptional<TInput, TOutput>(string argName, Expression<Func<TRequest, TInput>> expression, Func<TInput?, TOutput> convertArgValue) {
            if (argName is null) { throw new ArgumentNullException(nameof(argName)); }
            if (expression is null) { throw new ArgumentNullException(nameof(expression)); }
            if (convertArgValue is null) { throw new ArgumentNullException(nameof(convertArgValue)); }

            PropertyInfo              propertyInfo  = GetPropertyInfo(expression);
            var                       propertyValue = (TInput?)propertyInfo.GetValue(_request);
            OptionalArgument<TOutput> validatedArg  = _argumentsValidator.ConvertOptional(argName, propertyValue, convertArgValue);

            return validatedArg;
        }

        /// <inheritdoc />
        public OptionalArgument<TOutput> ConvertOptional<TInput, TOutput>(Expression<Func<TRequest, TInput>> expression, Func<TInput?, TOutput> convertArgValue) {
            if (expression is null) { throw new ArgumentNullException(nameof(expression)); }
            if (convertArgValue is null) { throw new ArgumentNullException(nameof(convertArgValue)); }

            PropertyInfo              propertyInfo  = GetPropertyInfo(expression);
            var                       propertyValue = (TInput?)propertyInfo.GetValue(_request);
            OptionalArgument<TOutput> validatedArg  = _argumentsValidator.ConvertOptional(propertyInfo.Name, propertyValue, convertArgValue);

            return validatedArg;
        }

        public OptionalArgument<TOutput> ConvertOptional<TInput, TOutput>(string argName, TInput? argValue, Func<TInput?, TOutput> convertArgValue) {
            return _argumentsValidator.ConvertOptional(argName, argValue, convertArgValue);
        }

        /// <inheritdoc />
        public RequiredArgument<TValue> IsRequired<TValue>(string argName, Expression<Func<TRequest, TValue>> expression) {
            if (argName is null) { throw new ArgumentNullException(nameof(argName)); }
            if (expression is null) { throw new ArgumentNullException(nameof(expression)); }

            PropertyInfo             propertyInfo  = GetPropertyInfo(expression);
            var                      propertyValue = (TValue?)propertyInfo.GetValue(_request);
            RequiredArgument<TValue> validatedArg  = _argumentsValidator.IsRequired(argName, propertyValue);

            return validatedArg;
        }

        /// <inheritdoc />
        public RequiredArgument<TValue> IsRequired<TValue>(Expression<Func<TRequest, TValue>> expression) {
            if (expression is null) { throw new ArgumentNullException(nameof(expression)); }

            PropertyInfo             propertyInfo  = GetPropertyInfo(expression);
            var                      propertyValue = (TValue?)propertyInfo.GetValue(_request);
            RequiredArgument<TValue> validatedArg  = _argumentsValidator.IsRequired(propertyInfo.Name, propertyValue);

            return validatedArg;
        }

        /// <inheritdoc />
        public RequiredArgument<TValue> IsRequired<TValue>(string argName, TValue? argValue) {
            return _argumentsValidator.IsRequired(argName, argValue);
        }

        /// <inheritdoc />
        public void AddError(ValidationError error) {
            _argumentsValidator.AddError(error);
        }

        /// <inheritdoc />
        public void AddErrors(IEnumerable<ValidationError> errors) {
            _argumentsValidator.AddErrors(errors);
        }

        /// <inheritdoc />
        public void AssertHasNoError() {
            _argumentsValidator.AssertHasNoError();
        }

        /// <inheritdoc />
        public ValidationError[] GetErrors() {
            return _argumentsValidator.GetErrors();
        }

        /// <inheritdoc />
        public override string ToString() {
            return _argumentsValidator.ToString();
        }

    }

}