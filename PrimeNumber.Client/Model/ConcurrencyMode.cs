using System.ComponentModel;

namespace PrimeNumber.Client.Model;

public enum ConcurrencyMode
{
    [Description("Blocking")]
    Blocking,

    [Description("Threading")]
    Threading,

    [Description("Async and Await")]
    AsyncAndAwait

}
