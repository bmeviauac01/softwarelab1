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

        public static TryResult<T[]> TryRunOperationAndCheckLength<T>(this Func<T[]> operation, int expectedLength, AhkResult ahkResult, string operationNameForError)
        {
            T[] list = null;
            try
            {
                list = operation();
            }
            catch (Exception ex)
            {
                ahkResult.AddProblem(ex, $"{operationNameForError} hibat dob. {operationNameForError} throws error.");
                return TryResult<T[]>.Failed();
            }

            return list.TryCheckLength(expectedLength, ahkResult, operationNameForError);
        }

        public static TryResult<T[]> TryCheckLength<T>(this T[] collection, int expectedLength, AhkResult ahkResult, string operationNameForError)
        {
            if (collection == null)
            {
                ahkResult.AddProblem($"{operationNameForError} null-lal ter vissza. {operationNameForError} yields null value.");
                return TryResult<T[]>.Failed();
            }

            if (collection.Length != expectedLength)
            {
                ahkResult.AddProblem($"{operationNameForError} nem a megfelelo mennyisegu elemet adja vissza. {operationNameForError} does not return the proper amount of items");
                return TryResult<T[]>.Failed();
            }
            else
            {
                return TryResult<T[]>.Ok(collection);
            }
        }

        public static TryResult<T[]> TryCheckLength<T>(this TryResult<T[]> listResult, int expectedLength, AhkResult ahkResult, string operationNameForError)
        {
            if (!listResult.Success)
                return TryResult<T[]>.Failed();
            else
                return listResult.Value.TryCheckLength(expectedLength, ahkResult, operationNameForError);
        }
    }
}
