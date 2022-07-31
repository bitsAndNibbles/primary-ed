using System;
using System.Net.Http;

namespace PrimeNumber.Client.Model;

internal abstract class PrimeNetClient
{
    protected static HttpClient Client = new HttpClient();

    protected static readonly Uri BaseUri =
        new Uri("http://127.0.0.1:31147/", UriKind.Absolute);

    protected static Uri GetRequestUri(long n)
    {
        return new Uri(BaseUri, n.ToString());
    }

}
