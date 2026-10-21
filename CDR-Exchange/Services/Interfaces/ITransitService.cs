using CDR_Exchange.Services.Dtos;

namespace CDR_Exchange.Services.Interfaces;

public interface ITransitService
{
    public Task<string> CreateAsync(string address, PairDto pair,
        CancellationToken cancellationToken = default);
    public Task<TransitWalletRateDto?> GetRateByTransitAddressAsync(string transitAddress, CancellationToken cancellationToken);
}