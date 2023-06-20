#region Usings declarations

using System.Collections.Generic;

#endregion

namespace Reefact.FluentRequestBinder {

    public interface Validator {

        bool HasError   { get; }
        int  ErrorCount { get; }

        void AddError(ValidationError error);

        void AddErrors(IEnumerable<ValidationError> errors);

        void AssertHasNoError();

        ValidationError[] GetErrors();

    }

}