#region Usings declarations

using System;

#endregion

namespace Reefact.RequestValidation {

    public interface SimplePropertyConverter<TInput> {

        RequiredArgument<TOutput> AsRequired<TOutput>(Func<TInput, TOutput> convert);

        RequiredArgument<TInput> AsRequired();

    }

}