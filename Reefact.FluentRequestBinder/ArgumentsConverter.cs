#region Usings declarations

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using Reefact.FluentRequestBinder.Configuration;

#endregion

namespace Reefact.FluentRequestBinder {

    /// <summary>
    ///     Converts simple arguments.
    /// </summary>
    [DebuggerDisplay("{ToString()}")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class ArgumentsConverter : Validator {

        #region Fields declarations

        private readonly Converter _argumentsValidator;

        #endregion

        #region Constructors declarations

        internal ArgumentsConverter(ValidationOptions validationOptions) {
            if (validationOptions is null) { throw new ArgumentNullException(nameof(validationOptions)); }

            _argumentsValidator = new Converter(validationOptions);
        }

        #endregion

        /// <inheritdoc />
        public bool HasError => _argumentsValidator.HasError;
        /// <inheritdoc />
        public int ErrorCount => _argumentsValidator.ErrorCount;

        /// <summary>
        ///     Converts a field to a simple property.
        /// </summary>
        /// <typeparam name="TArgument">The type of the field.</typeparam>
        /// <param name="argumentName">The name of the argument.</param>
        /// <param name="argumentValue">The value of the argument.</param>
        /// <returns>An instance of <see cref="SimplePropertyConverter{TArgument}" />.</returns>
        public SimplePropertyConverter<TArgument> SimpleProperty<TArgument>(string argumentName, TArgument? argumentValue) {
            if (argumentName is null) { throw new ArgumentNullException(nameof(argumentName)); }

            Argument<TArgument> argument = new(argumentName, argumentValue);

            return new SimplePropertyConverter<TArgument>(_argumentsValidator, argument);
        }

        /// <summary>
        ///     Convert a field to a complex property.
        /// </summary>
        /// <typeparam name="TArgument">The type of the field.</typeparam>
        /// <param name="argumentValue">The value of the argument.</param>
        /// <returns>An instance of <see cref="ComplexPropertyConverter{TArgument}" />.</returns>
        public ComplexPropertyConverter<TArgument> ComplexProperty<TArgument>(TArgument? argumentValue) {
#if NET8_0
            Argument<TArgument> argument = Argument.UnNamed(argumentValue);
#else
            Argument<TArgument> argument = Argument<TArgument>.UnNamed(argumentValue);
#endif

            return new ComplexPropertyConverter<TArgument>(_argumentsValidator, argument);
        }

        /// <summary>
        ///     Convert a list of simple properties.
        /// </summary>
        /// <typeparam name="TArgument">The type of the field.</typeparam>
        /// <param name="argumentName">The name of the argument.</param>
        /// <param name="argumentValue">The value of the argument.</param>
        /// <returns>An instance of <see cref="ListOfComplexPropertiesConverter{TArgument}" />.</returns>
        public ListOfSimplePropertiesConverter<TArgument> ListOfSimpleProperties<TArgument>(string argumentName, IEnumerable<TArgument>? argumentValue) {
            if (argumentName is null) { throw new ArgumentNullException(nameof(argumentName)); }

            return new ListOfSimplePropertiesConverter<TArgument>(_argumentsValidator, argumentName, argumentValue);
        }

        /// <inheritdoc />
        public void RecordError(ValidationError error) {
            if (error is null) { throw new ArgumentNullException(nameof(error)); }

            _argumentsValidator.RecordError(error);
        }

        /// <inheritdoc />
        public void RecordErrors(IEnumerable<ValidationError> errors) {
            if (errors is null) { throw new ArgumentNullException(nameof(errors)); }

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