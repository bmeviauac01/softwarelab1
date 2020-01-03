using ahk.common;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace adatvez.Helpers
{
    public static class HttpClientHelper
    {
        public static async Task<TryResult<TResult>> TryGet<TResult>(this HttpClient httpClient, string url, AhkResult ahkResult, bool allowNotFound = false)
        {
            return await httpClient.GetAsync(url).TryExecuteAndReadResponse<TResult>("GET", url, ahkResult, allowNotFound: allowNotFound);
        }

        public static async Task<TryResult<TResult>> TryPostWithReturnValue<TResult>(this HttpClient httpClient, string url, object value, AhkResult ahkResult, bool requireLocationHeader = false)
        {
            return await httpClient.PostAsJsonAsync(url, value).TryExecuteAndReadResponse<TResult>("POST", url, ahkResult,
                responseAdditionalCheck: httpResponse =>
                {
                    if (requireLocationHeader)
                    {
                        if (!httpResponse.Headers.Contains(Microsoft.Net.Http.Headers.HeaderNames.Location) || httpResponse.Headers.Location == null)
                        {
                            ahkResult.AddProblem($"POST {url} valaszban hianyzo header. POST {url} reponse missing header.");
                            return false;
                        }
                    }

                    return true;
                });
        }

        public static async Task<TryResult<bool>> TryHead(this HttpClient httpClient, string url, AhkResult ahkResult)
        {
            var requestMethodAndUrl = $"HEAD {url}";
            try
            {
                var responseMessage = await httpClient.HeadAsync(url);
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                    return TryResult<bool>.Ok(true);
                else
                    return TryResult<bool>.Ok(false);
            }
            catch (Exception ex)
            {
                ahkResult.AddProblem(ex, $"{requestMethodAndUrl} keres sikertelen. {requestMethodAndUrl} request unsuccessful.");
                return TryResult<bool>.Failed();
            }
        }

        public static async Task<TryResult<bool>> TryDelete(this HttpClient httpClient, string url, AhkResult ahkResult)
        {
            var requestMethodAndUrl = $"DELETE {url}";
            try
            {
                var responseMessage = await httpClient.DeleteAsync(url);
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK || responseMessage.StatusCode == System.Net.HttpStatusCode.NoContent)
                    return TryResult<bool>.Ok(true);
                else
                    return TryResult<bool>.Ok(false);
            }
            catch (Exception ex)
            {
                ahkResult.AddProblem(ex, $"{requestMethodAndUrl} keres sikertelen. {requestMethodAndUrl} request unsuccessful.");
                return TryResult<bool>.Failed();
            }
        }

        public static async Task<HttpResponseMessage> HeadAsync(this HttpClient httpClient, string url)
        {
            return await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));
        }

        public static async Task<TryResult<T>> TryExecuteAndReadResponse<T>(this Task<HttpResponseMessage> send, string httpMethod, string url, AhkResult ahkResult,
            bool allowNotFound = false, Predicate<HttpResponseMessage> responseAdditionalCheck = null)
        {
            var requestMethodAndUrl = $"{httpMethod.ToUpperInvariant()} {url}";
            HttpResponseMessage responseMessage;
            try
            {
                responseMessage = await send;
            }
            catch (Exception ex)
            {
                ahkResult.AddProblem(ex, $"{requestMethodAndUrl} keres sikertelen. {requestMethodAndUrl} request unsuccessful.");
                return TryResult<T>.Failed();
            }

            if (responseAdditionalCheck != null)
            {
                if (!responseAdditionalCheck(responseMessage))
                    return TryResult<T>.Failed();
            }

            if (allowNotFound && responseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                return TryResult<T>.Ok(default(T));

            return await responseMessage.TryReadResponse<T>(ahkResult);
        }

        public static async Task<TryResult<TResult>> TryReadResponse<TResult>(this HttpResponseMessage httpResponse, AhkResult ahkResult)
        {
            var requestMethodAndUrl = $"{httpResponse.RequestMessage.Method.Method.ToUpperInvariant()} {httpResponse.RequestMessage.RequestUri}";

            if (!httpResponse.IsSuccessStatusCode)
            {
                ahkResult.AddProblem($"{requestMethodAndUrl} hibas valaszkod {httpResponse.StatusCode}. {requestMethodAndUrl} yields invalid response {httpResponse.StatusCode}.");
                return TryResult<TResult>.Failed();
            }

            try
            {
                var value = await httpResponse.Content.ReadAsAsync<TResult>();

                if (value == null)
                {
                    ahkResult.AddProblem($"{requestMethodAndUrl} valasz tartalma hibas. {requestMethodAndUrl} yields invalid content.");
                    return TryResult<TResult>.Failed();
                }

                return TryResult<TResult>.Ok(value);
            }
            catch (Exception ex)
            {
                ahkResult.AddProblem(ex, $"{requestMethodAndUrl} valasz tartalma hibas. {requestMethodAndUrl} yields invalid content.");
                return TryResult<TResult>.Failed();
            }
        }
    }
}
