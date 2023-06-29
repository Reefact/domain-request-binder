#region Usings declarations

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

using Reefact.FluentRequestBinder.Configuration;

#endregion

namespace Reefact.FluentRequestBinder {

    /// <summary>
    ///     Converts requests.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    [DebuggerDisplay("{ToString()}")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class RequestConverter<TRequest> : Validator {

        #region Statics members declarations

        private static PropertyInfo GetPropertyInfo<TObject, TProperty>(Expression<Func<TObject, TProperty>> expression) {
            MemberExpression? body = expression.Body as MemberExpression;
            if (body == null) { throw new InvalidOperationException("Expression body is not of MemberExpression type."); }

            PropertyInfo? member = body.Member as PropertyInfo;
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

        /// <summary>
        ///     Converts a field to a simple property.
        /// </summary>
        /// <param name="expression">The field selector.</param>
        /// <typeparam name="TArgument">The type of the field.</typeparam>
        /// <returns>An instance of <see cref="SimplePropertyConverter{TArgument}" />.</returns>
        public SimplePropertyConverter<TArgument> SimpleProperty<TArgument>(Expression<Func<TRequest, TArgument>> expression) {
            PropertyInfo propertyInfo  = GetPropertyInfo(expression);
            string       argumentName  = _validationOptions.PropertyNameProvider.GetArgumentNameFrom(propertyInfo);
            TArgument?   argumentValue = (TArgument?)propertyInfo.GetValue(_request);

            return new SimplePropertyConverter<TArgument>(_argumentsValidator, argumentName, argumentValue);
        }

        /// <summary>
        ///     Convert a field to a complex property.
        /// </summary>
        /// <param name="expression">The field selector.</param>
        /// <typeparam name="TArgument">The type of the field.</typeparam>
        /// <returns>An instance of <see cref="ComplexPropertyConverter{TArgument}" />.</returns>
        public ComplexPropertyConverter<TArgument> ComplexProperty<TArgument>(Expression<Func<TRequest, TArgument>> expression) {
            PropertyInfo propertyInfo  = GetPropertyInfo(expression);
            string       argumentName  = _validationOptions.PropertyNameProvider.GetArgumentNameFrom(propertyInfo);
            TArgument?   argumentValue = (TArgument?)propertyInfo.GetValue(_request);

            return new ComplexPropertyConverter<TArgument>(_argumentsValidator, argumentName, argumentValue);
        }

        /// <summary>
        ///     Convert a list of simple properties.
        /// </summary>
        /// <param name="expression">The field selector.</param>
        /// <typeparam name="TArgument">The type of the field.</typeparam>
        /// <returns>An instance of <see cref="ListOfComplexPropertiesConverter{TArgument}" />.</returns>
        public ListOfSimplePropertiesConverter<TArgument> ListOfSimpleProperties<TArgument>(Expression<Func<TRequest, IEnumerable<TArgument>>> expression) {
            PropertyInfo propertyInfo  = GetPropertyInfo(expression);
            string       argumentName  = _validationOptions.PropertyNameProvider.GetArgumentNameFrom(propertyInfo);
            var          argumentValue = (IEnumerable<TArgument>?)propertyInfo.GetValue(_request);

            return new ListOfSimplePropertiesConverter<TArgument>(_argumentsValidator, argumentName, argumentValue);
        }

        /// <summary>
        ///     Convert a list of complex properties.
        /// </summary>
        /// <param name="expression">The field selector.</param>
        /// <typeparam name="TArgument">The type of the field.</typeparam>
        /// <returns>An instance of <see cref="ListOfComplexPropertiesConverter{TArgument}" />.</returns>
        public ListOfComplexPropertiesConverter<TArgument> ListOfComplexProperties<TArgument>(Expression<Func<TRequest, IEnumerable<TArgument>>> expression) {
            PropertyInfo propertyInfo  = GetPropertyInfo(expression);
            string       argumentName  = _validationOptions.PropertyNameProvider.GetArgumentNameFrom(propertyInfo);
            var          argumentValue = (IEnumerable<TArgument>?)propertyInfo.GetValue(_request);

            return new ListOfComplexPropertiesConverter<TArgument>(_argumentsValidator, argumentName, argumentValue);
        }

        /// <inheritdoc />
        public void RecordError(ValidationError error) {
            _argumentsValidator.RecordError(error);
        }

        /// <inheritdoc />
        public void RecordErrors(IEnumerable<ValidationError> errors) {
            _argumentsValidator.RecordErrors(errors);
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