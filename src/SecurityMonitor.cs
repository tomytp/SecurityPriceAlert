using SecurityPriceAlert.Communication;

namespace SecurityPriceAlert;

public class SecurityMonitor
{
    public event EventHandler<BrApiQuote>? SellTrigger;
    public event EventHandler<BrApiQuote>? BuyTrigger;
    
    private readonly BrApiHandler _apiHandler;
    private readonly string _token;
    private double? _lastPrice;

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
    
    public async Task CheckSecurity(string securitySymbol, double sellTriggerPrice, double buyTriggerPrice)
    {
        BrApiQuoteResponse? response = await _apiHandler.GetSecurityQuoteAsync(securitySymbol, _token);
        if (response?.Results == null || response.Results.Count == 0) return;

        BrApiQuote quote = response.Results.First();
        Console.WriteLine($"[{DateTime.Now}] Current price for {quote.Symbol} is {quote.RegularMarketPrice}");
        
        // Por simplicidade, nao considerei a imprecisao da comparacao de double 
        if (quote.RegularMarketPrice > sellTriggerPrice && (_lastPrice == null || _lastPrice <= sellTriggerPrice))
        {
            SellTrigger?.Invoke(this, quote);
        }
        else if (quote.RegularMarketPrice < buyTriggerPrice && (_lastPrice == null || _lastPrice >= buyTriggerPrice))
        {
            BuyTrigger?.Invoke(this, quote);
        }
        _lastPrice = quote.RegularMarketPrice;
    }
}