#region Usings declarations

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;

#endregion

namespace Reefact.FluentRequestBinder {

    /// <summary>
    ///     The exceptions thrown in the event of a request validation error.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class BadRequestException : ApplicationException {

        #region Statics members declarations

        /// <summary>
        /// Creates a new <see cref="BadRequestException"/> based on a validator.
        /// </summary>
        /// <param name="validator">The validator containing errors.</param>
        /// <returns>The exception.
        /// </returns>
        public static BadRequestException From(Validator validator) {
            return new BadRequestException(validator.GetErrors().ToArray());
        }

        #endregion

        #region Constructors declarations

        /// <summary>Instantiate a new <see cref="BadRequestException"/>.</summary>
        protected BadRequestException(SerializationInfo info, StreamingContext context) : base(info, context) {
            ValidationErrors = (ValidationError[])info.GetValue(FieldSerializationKey.ValidationErrors, FieldSerializationType.ValidationErrors)!;
        }

        private BadRequestException(ValidationError[] errors) {
            ValidationErrors = errors;
        }

        #endregion

        /// <summary>
        ///     The validation errors.
        /// </summary>
        public ValidationError[] ValidationErrors { get; }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue(FieldSerializationKey.ValidationErrors, ValidationErrors, FieldSerializationType.ValidationErrors);
            base.GetObjectData(info, context);
        }

        #region Nested types declarations

        private static class FieldSerializationKey {

            public const string ValidationErrors = "ValidationErrors";

        }

        private static class FieldSerializationType {

            #region Statics members declarations

            public static readonly Type ValidationErrors = typeof(ValidationError[]);

            #endregion

        }

        #endregion

    }

}