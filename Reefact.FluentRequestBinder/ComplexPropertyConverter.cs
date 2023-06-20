#region Usings declarations

using System;

#endregion

namespace Reefact.FluentRequestBinder {

    public interface ComplexPropertyConverter<TInput> {

        RequiredArgument<TOutput> AsRequired<TOutput>(Func<RequestConverter<TInput>, TOutput> func);

    }

}