using CDR_Exchange.Domain.Models;
using CDR_Exchange.Services.Dtos;

namespace CDR_Exchange.Services.Interfaces;

public interface IPairsService
{
    public Task<List<PairDto>> GetPairsAsync(CancellationToken cancellationToken = default);
    Task<PairDto> GetPairOrThrowExceptionAsync(TransitWallet transitWallet, CancellationToken cancellationToken = default);
    Task<PairDto> GetPairByTransitAddressOrThrowExceptionAsync(string transitAddress, CancellationToken cancellationToken = default);
}