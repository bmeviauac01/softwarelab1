using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ahk.common
{
    public static class CollectionTestHelper
    {
        public static TryResult<T> TryRunOperationAndFindItem<T>(this Func<IEnumerable<T>> operation, Func<T, bool> matcher, AhkResult ahkResult, string operationNameForError)
        {
            IEnumerable<T> list = null;
            try
            {
                list = operation();
            }
            catch (Exception ex)
            {
                ahkResult.AddProblem(ex, $"{operationNameForError} hibat dob. {operationNameForError} throws error.");
                return TryResult<T>.Failed();
            }

            return list.TryFindItem(matcher, ahkResult, operationNameForError);
        }

        public static TryResult<T> TryFindItem<T>(this IEnumerable<T> collection, Func<T, bool> matcher, AhkResult ahkResult, string operationNameForError)
        {
            if (collection == null)
            {
                ahkResult.AddProblem($"{operationNameForError} null-lal ter vissza. {operationNameForError} yields null value.");
                return TryResult<T>.Failed();
            }

            var record = collection.FirstOrDefault(matcher);
            if (record == null)
            {
                ahkResult.AddProblem($"{operationNameForError} nem tartalmaz egy elemet. {operationNameForError} does not return an existing record.");
                return TryResult<T>.Failed();
            }
            else
            {
                return TryResult<T>.Ok(record);
            }
        }

        public static TryResult<T> TryFindItem<T>(this TryResult<T[]> listResult, Func<T, bool> matcher, AhkResult ahkResult, string operationNameForError)
        {
            if (!listResult.Success)
                return TryResult<T>.Failed();
            else
                return listResult.Value.TryFindItem(matcher, ahkResult, operationNameForError);
        }

        public static async Task<TryResult<T>> TryFindItem<T>(this Task<TryResult<T[]>> listResult, Func<T, bool> matcher, AhkResult ahkResult, string operationNameForError)
        {
            return (await listResult).TryFindItem<T>(matcher, ahkResult, operationNameForError);
        }
    }
}
