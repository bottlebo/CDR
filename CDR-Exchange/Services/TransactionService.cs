using CDR_Exchange.Dal;
using CDR_Exchange.Domain.Enums;
using CDR_Exchange.Domain.Models;
using CDR_Exchange.Infrastructure.Exceptions;
using CDR_Exchange.Services.Dtos;
using CDR_Exchange.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CDR_Exchange.Services;

public class TransactionService : ITransactionService
{
    private readonly ExchangeDbContext _dbContext;
    private readonly IPairsService _pairsService;

    public TransactionService(ExchangeDbContext dbContext, IPairsService pairsService)
    {
        _dbContext = dbContext;
        _pairsService = pairsService;
    }

    public async Task<TransactionDto> CreateAsync(CreateTransactionRequest request, CancellationToken cancellationToken = default)
    {
        var transitWallet =
            await _dbContext.TransitWallets.FirstOrDefaultAsync(x => x.Address == request.TransitAddress,
                cancellationToken);

        if (transitWallet == null)
        {
            throw new NotFoundException($"Transit wallet not found. Wallet address {request.TransitAddress}");
        }

        var now = DateTime.UtcNow;

        var transaction = new Transaction
        {
            Address = request.Address,
            Amount = request.Amount,
            CreateDate = now,
            TransitWalletId = transitWallet.Id,
            TransactionHistories = new List<TransactionHistory>
            {
                new ()
                {
                    CreateDate = now,
                    Status = TransactionStatus.Created
                }
            }
        };

        var entity = await _dbContext.Transactions.AddAsync(transaction, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new TransactionDto
        {
            Id = entity.Entity.Id,
            Amount = entity.Entity.Amount,
            CreateDate = now,
            Status = TransactionStatus.Created,
            TransitAddress = transitWallet.Address,
            Pair = await _pairsService.GetPairOrThrowExceptionAsync(transitWallet, cancellationToken)
        };
    }

    public async Task<TransactionDto> GetByTransitAddress(string transitAddress, CancellationToken cancellationToken = default)
    {
        var transitWallet =
            await _dbContext.TransitWallets
                .Include(x => x.Transaction)
                .FirstOrDefaultAsync(x => x.Address == transitAddress, cancellationToken);

        if (transitWallet == null)
        {
            throw new NotFoundException($"Transit wallet not found. Wallet address {transitAddress}");
        }

        if (transitWallet.Transaction == null)
        {
            throw new NotFoundException($"Transaction not found. Wallet address {transitAddress}");
        }

        var status = await _dbContext.TransactionHistories
            .Where(x => x.TransactionId == transitWallet.Transaction.Id)
            .OrderBy(x => x.CreateDate)
            .FirstAsync(cancellationToken);

        return new TransactionDto
        {
            Id = transitWallet.Transaction.Id,
            Amount = transitWallet.Transaction.Amount,
            CreateDate = transitWallet.Transaction.CreateDate,
            Status = status.Status,
            TransitAddress = transitWallet.Address, 
            SwapAddress = transitWallet.SwapAddress,
            TransactionAddress = transitWallet.Transaction.Address,
            Pair = await _pairsService.GetPairOrThrowExceptionAsync(transitWallet, cancellationToken)
        };
    }
}