#region Usings declarations

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

using Reefact.FluentRequestBinder.Configuration;

#endregion

namespace Reefact.FluentRequestBinder {

    [DebuggerDisplay("{ToString()}")]
    public sealed class RequestConverter<TRequest> : Validator {

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

        private readonly TRequest           _request;
        private readonly ValidationOptions  _validationOptions;
        private readonly ArgumentsConverter _argumentsValidator;

        #endregion

        #region Constructors declarations

        internal RequestConverter(TRequest request, ValidationOptions validationOptions) {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (validationOptions is null) { throw new ArgumentNullException(nameof(validationOptions)); }

            _request            = request;
            _validationOptions  = validationOptions;
            _argumentsValidator = new ArgumentsConverter(validationOptions);
        }

        internal RequestConverter(TRequest request, ValidationOptions validationOptions, string argNamePrefix) {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (validationOptions is null) { throw new ArgumentNullException(nameof(validationOptions)); }

            _request            = request;
            _validationOptions  = validationOptions;
            _argumentsValidator = new ArgumentsConverter(validationOptions, argNamePrefix);
        }

        #endregion

        /// <inheritdoc />
        public bool HasError => _argumentsValidator.HasError;
        /// <inheritdoc />
        public int ErrorCount => _argumentsValidator.ErrorCount;

        public SimplePropertyConverter<TInput> SimpleProperty<TInput>(Expression<Func<TRequest, TInput>> expression) {
            PropertyInfo propertyInfo = GetPropertyInfo(expression);
            string       argName      = _validationOptions.PropertyNameProvider.GetName(propertyInfo);
            var          argValue     = (TInput)propertyInfo.GetValue(_request);

            return new SimplePropertyConverter<TInput>(_argumentsValidator, argName, argValue);
        }

        public ComplexPropertyConverter<TInput> ComplexProperty<TInput>(Expression<Func<TRequest, TInput>> expression) {
            PropertyInfo propertyInfo = GetPropertyInfo(expression);
            string       argName      = _validationOptions.PropertyNameProvider.GetName(propertyInfo);
            var          argValue     = (TInput)propertyInfo.GetValue(_request);

            return new ComplexPropertyConverter<TInput>(_argumentsValidator, argName, argValue);
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