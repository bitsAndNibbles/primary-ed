using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace PrimeNumber.Client.Model;

internal class PrimeNetClient
{
    HttpClient _client;

    internal PrimeNetClient()
    {
        _client = new HttpClient();
    }

    internal async Task<int> NextPrimeAsync(int n)
    {
        var response = await _client.GetAsync(
            $"http://localhost:31147/{n}",
            HttpCompletionOption.ResponseContentRead);

        string responseBody =
            await response.Content.ReadAsStringAsync();

        return int.Parse(responseBody);
    }

    internal int NextPrime(int n)
    {
        Task<int> task = NextPrimeAsync(n);

        task.Start();

        return task.Result;
    }

}
