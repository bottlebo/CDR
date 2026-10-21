using CDR_Exchange.Services.Dtos;

namespace CDR_Exchange.Services.Interfaces;

public interface ITransactionService
{
    Task<TransactionDto> CreateAsync(CreateTransactionRequest request,
        CancellationToken cancellationToken = default);

    Task<TransactionDto> GetByTransitAddress(string transitAddress, CancellationToken cancellationToken = default);
}