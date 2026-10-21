using CDR_Exchange.Services.Cil.Interfaces;
using CDR_Exchange.Services.Cil.Responses;
using CDR_Exchange.Services.Extensions;

namespace CDR_Exchange.Services.Cil;

public class CilExplorerApi : ICilExplorerApi
{
    private readonly HttpClient _httpClient;

    public CilExplorerApi(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CilBalanceResponse> GetBalance(string address)
    {
        var url = $"api/Balance/{address.Remove(0, 2)}";
        var result = await _httpClient.GetAndReturnStringAsync(url);
        return new CilBalanceResponse(result);
    }

    public async Task<CilTokenBalancesResponse> GetTokenBalances(string address)
    {
        var url = $"api/Token/Balances/{address.Remove(0, 2)}";
        var result = await _httpClient.GetAndReturnStringAsync(url);
        return new CilTokenBalancesResponse(result);
    }

    public async Task<TokenHoldersResponse> GetTokenHolders(string symbol)
    {
        var url = $"api/Token/Holders/{symbol}";
        var result = await _httpClient.GetAndReturnStringAsync(url);
        return new TokenHoldersResponse(result);
    }

    public async Task<CilWalletListUnspentResponse> GetWalletListUnspent(string wallet)
    {
        var url = $"api/Unspent/{wallet.Remove(0, 2)}";
        var result = await _httpClient.GetAndReturnStringAsync(url);
        return new CilWalletListUnspentResponse(result);
    }
}