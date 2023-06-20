#region Usings declarations

using System;

#endregion

namespace Reefact.RequestValidation {

    public interface ComplexPropertyConverter<TInput> {

        RequiredArgument<TOutput> AsRequired<TOutput>(Func<RequestConverter<TInput>, TOutput> func);

    }

}