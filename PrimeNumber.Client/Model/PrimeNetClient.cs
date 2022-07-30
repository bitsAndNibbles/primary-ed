using System;
using System.Net.Http;

namespace PrimeNumber.Client.Model;

internal abstract class PrimeNetClient : IDisposable
{
    protected HttpClient Client;

    protected static readonly Uri BaseUri =
        new Uri("http://localhost:31147/", UriKind.Absolute);

    internal PrimeNetClient()
    {
        Client = new HttpClient();
    }

    public void Dispose()
    {
        Client.Dispose();
    }

    protected Uri GetRequestUri(long n)
    {
        return new Uri(BaseUri, n.ToString());
    }

}
