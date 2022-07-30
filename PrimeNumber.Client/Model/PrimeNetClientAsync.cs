using System.Threading.Tasks;

namespace PrimeNumber.Client.Model;

internal class PrimeNetClientAsync : PrimeNetClient
{
    /// <summary>
    /// Returns a task that will compute the next prime
    /// number larger than <paramref name="n"/>.
    /// </summary>
    /// <param name="n">any integer number</param>
    /// <returns>
    /// Returns a task that produces the next prime number
    /// number larger than <paramref name="n"/>.
    /// </returns>
    internal async Task<long> NextPrimeAsync(long n)
    {
        var response = await Client.GetAsync(GetRequestUri(n))
            .ConfigureAwait(false);

        string responseBody =
            await response.Content.ReadAsStringAsync()
                .ConfigureAwait(false);

        return long.Parse(responseBody);
    }

}
