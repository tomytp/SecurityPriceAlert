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
}

public record BrApiSearch(
    List<string> indexes,
    List<string> stocks
);  