using System;
using System.Collections.Generic;

namespace ahk.common
{
    public static class Op
    {
        public static Func<bool> Func(Action a) => () => { a(); return true; };
        public static Func<T> Func<T>(Func<T> a) => a;
        public static Func<IEnumerable<T>> Func<T>(Func<IEnumerable<T>> a) => a;

        public static TryResult<T> TryRunOperation<T>(this Func<T> operation, AhkResult ahkResult, string operationNameForError)
        {
            try
            {
                return TryResult<T>.Ok(operation());
            }
            catch (Exception ex)
            {
                ahkResult.AddProblem(ex, $"{operationNameForError} hibat dob. {operationNameForError} throws error.");
                return TryResult<T>.Failed();
            }
        }
    }
}
