#region Usings declarations

using System.Diagnostics.CodeAnalysis;

#endregion

namespace Reefact.FluentRequestBinder {

    /// <summary>
    ///     The exceptions thrown in the event of a request validation error.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class BadRequestException : ApplicationException {

        #region Statics members declarations

        /// <summary>
        ///     Creates a new <see cref="BadRequestException" /> based on a validator.
        /// </summary>
        /// <param name="validator">The validator containing errors.</param>
        /// <returns>
        ///     The exception.
        /// </returns>
        public static BadRequestException From(Validator validator) {
            return new BadRequestException(validator.GetErrors().ToArray());
        }

        #endregion

        #region Constructors declarations

        private BadRequestException(ValidationError[] errors) {
            ValidationErrors = errors;
        }

        #endregion

        /// <summary>
        ///     The validation errors.
        /// </summary>
        public ValidationError[] ValidationErrors { get; }

    }

}