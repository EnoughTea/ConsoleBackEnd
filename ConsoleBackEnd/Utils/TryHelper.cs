using System;
using LanguageExt;
using LanguageExt.Common;

namespace ConsoleBackEnd
{
    public static class TryHelper
    {
        public static Try<T> Fail<T>(Exception e) => () => new Result<T>(e);

        public static Try<T> FailNull<T>(string paramName) => () => new Result<T>(new ArgumentNullException(paramName));
    }
}