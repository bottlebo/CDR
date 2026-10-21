using CDR_Exchange.Services.Cil.Responses;

namespace CDR_Exchange.Services.Cil.Interfaces;

public interface ICilExplorerApi
{
    Task<CilBalanceResponse> GetBalance(string address);
    Task<CilTokenBalancesResponse> GetTokenBalances(string address);
    Task<TokenHoldersResponse> GetTokenHolders(string symbol);
    Task<CilWalletListUnspentResponse> GetWalletListUnspent(string wallet);
}