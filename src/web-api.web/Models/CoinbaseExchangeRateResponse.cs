using System.Text.Json.Serialization;

namespace web_api.web.Models;

/// <summary>
/// Represents the response from Coinbase API v2/exchange-rates endpoint
/// </summary>
public record CoinbaseExchangeRateResponse(
    [property: JsonPropertyName("data")]
    ExchangeRateData Data
);

/// <summary>
/// Contains the exchange rate data including base currency and rates
/// </summary>
public record ExchangeRateData(
    [property: JsonPropertyName("currency")]
    string Currency,

    [property: JsonPropertyName("rates")]
    Dictionary<string, string> Rates
);
