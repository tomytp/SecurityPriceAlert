using SecurityPriceAlert.Communication;

namespace SecurityPriceAlert;

public class SecurityMonitor
{
    private readonly BrApiHandler _apiHandler;
    private readonly string _token;
    
    public SecurityMonitor(string token)
    {
        _apiHandler = new BrApiHandler();
        _token = token;
    }
    
    public async Task<bool?> IsValidSymbol(string securitySymbol)
    {
        BrApiSearch? response = await _apiHandler.GetSecuritiesBySymbol(securitySymbol, _token);
        if (response == null) return null;
        return response.indexes.Contains(securitySymbol) || response.stocks.Contains(securitySymbol);
    }
}