#region Usings declarations

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#endregion

namespace Reefact.FluentRequestBinder {

    /// <summary>
    ///     Represents a request validator.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    public interface Validator {

        /// <summary>
        ///     Indicates whether the current instance of <see cref="Validator">validator</see> has recorded one or more
        ///     <see cref="ValidationError">errors</see> (<c>true</c>) or not (<c>false</c>).
        /// </summary>
        bool HasError { get; }

        /// <summary>
        ///     The number of <see cref="ValidationError">error</see> recorded by the current instance of
        ///     <see cref="Validator">validator</see>.
        /// </summary>
        int ErrorCount { get; }

        /// <summary>Records an <see cref="ValidationError">error</see>.</summary>
        /// <param name="error">The validation error to record.</param>
        void RecordError(ValidationError error);

        /// <summary>
        ///     Records several <see cref="ValidationError">validation errors</see>.
        /// </summary>
        /// <param name="errors">The validation errors to record.</param>
        void RecordErrors(IEnumerable<ValidationError> errors);

        /// <summary>
        ///     Asserts that the current instance of the <see cref="Validator">validator</see> has not recorded any
        ///     <see cref="ValidationError">errors</see> by raising an <see cref="BadRequestException">exception</see>
        ///     if this is not the cas.
        /// </summary>
        void AssertHasNoError();

        /// <summary>
        ///     Provides all recorded <see cref="ValidationError">validation errors</see>.
        /// </summary>
        /// <returns>An array of <see cref="ValidationError">validation errors</see>. </returns>
        ValidationError[] GetErrors();

    }

}