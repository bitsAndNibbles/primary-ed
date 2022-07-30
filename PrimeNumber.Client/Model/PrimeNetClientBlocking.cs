using System;
using System.IO;
using System.Net.Http;

namespace PrimeNumber.Client.Model;

internal class PrimeNetClientBlocking : PrimeNetClient
{
    internal long NextPrime(long n)
    {
        HttpRequestMessage? requestMessage = null;
        HttpResponseMessage? responseMessage = null;
        Stream? responseStream = null;
        StreamReader? responseReader = null;

        try
        {
            requestMessage = new HttpRequestMessage(
                HttpMethod.Get,
                GetRequestUri(n));

            responseMessage = Client.Send(requestMessage);

            responseStream = responseMessage.Content.ReadAsStream();

            var primeStr = new StreamReader(responseStream).ReadToEnd();

            return long.Parse(primeStr);
        }
        finally
        {
            DisposeItems(
                requestMessage,
                responseMessage,
                responseStream,
                responseReader);
        }
    }

    private void DisposeItems(params IDisposable?[] disposables)
    {
        foreach (var disposable in disposables)
        {
            try { disposable?.Dispose(); } catch { }
        }
    }

}
