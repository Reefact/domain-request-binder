#region Usings declarations

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

#endregion

namespace Reefact.RequestValidation {

    public interface RequestConverter<TRequest> : Validator {

        SimplePropertyConverter<TInput>  SimpleProperty<TInput>(Expression<Func<TRequest, TInput>>  property);
        ComplexPropertyConverter<TInput> ComplexProperty<TInput>(Expression<Func<TRequest, TInput>> property);

    }

    [DebuggerDisplay("{ToString()}")]
    public sealed class RequestConverter<THandledException, TRequest> : RequestConverter<TRequest>
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
        private readonly PropertyNameProvider                       _argNameProvider;
        private readonly ArgumentsValidator<THandledException> _argumentsValidator;

        #endregion

        #region Constructors declarations

        internal RequestConverter(TRequest request, PropertyNameProvider argNameProvider) {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (argNameProvider is null) { throw new ArgumentNullException(nameof(argNameProvider)); }

            _request            = request;
            _argNameProvider    = argNameProvider;
            _argumentsValidator = new ArgumentsValidator<THandledException>();
        }

        internal RequestConverter(TRequest request, PropertyNameProvider argNameProvider, string argNamePrefix) {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (argNameProvider is null) { throw new ArgumentNullException(nameof(argNameProvider)); }

            _request            = request;
            _argNameProvider    = argNameProvider;
            _argumentsValidator = new ArgumentsValidator<THandledException>(argNamePrefix);
        }

        #endregion

        /// <inheritdoc />
        public bool HasError => _argumentsValidator.HasError;
        /// <inheritdoc />
        public int ErrorCount => _argumentsValidator.ErrorCount;

        /// <inheritdoc />
        public SimplePropertyConverter<TInput> SimpleProperty<TInput>(Expression<Func<TRequest, TInput>> expression) {
            PropertyInfo propertyInfo = GetPropertyInfo(expression);
            string       argName      = _argNameProvider.GetName(propertyInfo);
            var          argValue     = (TInput)propertyInfo.GetValue(_request);

            return new SimplePropertyConverterImp<THandledException, TInput>(_argumentsValidator, argName, argValue);
        }

        /// <inheritdoc />
        public ComplexPropertyConverter<TInput> ComplexProperty<TInput>(Expression<Func<TRequest, TInput>> expression) {
            PropertyInfo propertyInfo = GetPropertyInfo(expression);
            string       argName      = _argNameProvider.GetName(propertyInfo);
            var          argValue     = (TInput)propertyInfo.GetValue(_request);

            return new ComplexPropertyConverterImp<THandledException, TInput>(_argumentsValidator, argName, argValue);
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