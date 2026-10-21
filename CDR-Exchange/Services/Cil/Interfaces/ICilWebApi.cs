using CDR_Exchange.Services.Cil.Responses;
using CDR_Exchange.Services.Dtos;

namespace CDR_Exchange.Services.Cil.Interfaces;

public interface ICilWebApi
{
    Task<string> GetPairsAsync(CancellationToken cancellationToken = default);
    Task<CilSwapResponse> GetSwapAddressAsync(string hashAddress, string coinFrom, string coinTo, string symbolFrom, string symbolTo,  CancellationToken cancellationToken = default);
    Task<CilSwapStatusResponse> GetSwapStatusAsync(string hashAddress, CancellationToken cancellationToken);
    Task<SwapRateResponse> GetSwapRateAsync(string coinFrom, string coinTo, string symbolTo, string symbolFrom, CancellationToken cancellationToken = default);
}