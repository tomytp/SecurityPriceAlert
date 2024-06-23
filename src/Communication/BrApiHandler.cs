using System.Net.Http.Json;

namespace SecurityPriceAlert.Communication;

public class BrApiHandler
{
    private readonly HttpClient _httpClient;

    public BrApiHandler()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://brapi.dev/api/")
        };
    }
    
    public async Task<BrApiSearch?> GetSecuritiesBySymbol(string securitySymbol, string token)
    {
        return await _httpClient.GetFromJsonAsync<BrApiSearch>($"available?search={securitySymbol}&token={token}");
    }
    
    public async Task<BrApiQuoteResponse?> GetSecurityQuoteAsync(string securitySymbol, string token)
    {
        return await _httpClient.GetFromJsonAsync<BrApiQuoteResponse>($"quote/{securitySymbol}?token={token}");
    }

}

public record BrApiSearch(
    List<string> indexes,
    List<string> stocks
);

public record BrApiQuote(
    string Currency,
    string ShortName,
    string LongName,
    double RegularMarketChange,
    double RegularMarketChangePercent,
    DateTime RegularMarketTime,
    double RegularMarketPrice,
    double RegularMarketDayHigh,
    string RegularMarketDayRange,
    double RegularMarketDayLow,
    int RegularMarketVolume,
    double RegularMarketPreviousClose,
    double RegularMarketOpen,
    string FiftyTwoWeekRange,
    double FiftyTwoWeekLow,
    double FiftyTwoWeekHigh,
    string Symbol,
    double PriceEarnings,
    double EarningsPerShare,
    string Logourl
);

public record BrApiQuoteResponse(
    List<BrApiQuote> Results,
    DateTime RequestedAt,
    string Took
);
