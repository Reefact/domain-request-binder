#region Usings declarations

using System;
using System.Linq;
using System.Runtime.Serialization;

#endregion

namespace Reefact.RequestValidation {

    public class BadRequestException : ApplicationException {

        #region Statics members declarations

        public static BadRequestException From(ArgumentsValidator validator) {
            return new BadRequestException(validator.GetErrors().ToArray());
        }

        #endregion

        #region Constructors declarations

        protected BadRequestException(SerializationInfo info, StreamingContext context) : base(info, context) {
            ValidationErrors = (ValidationError[])info.GetValue(FieldSerializationKey.ValidationErrors, FieldSerializationType.ValidationErrors)!;
        }

        private BadRequestException(ValidationError[] errors) {
            ValidationErrors = errors;
        }

        #endregion

        public ValidationError[] ValidationErrors { get; }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue(FieldSerializationKey.ValidationErrors, ValidationErrors, FieldSerializationType.ValidationErrors);
            base.GetObjectData(info, context);
        }

        #region Nested types declarations

        private class FieldSerializationKey {

            public const string ValidationErrors = "ValidationErrors";

        }

        private class FieldSerializationType {

            #region Statics members declarations

            public static readonly Type ValidationErrors = typeof(ValidationError[]);

            #endregion

        }

        #endregion

    }

}