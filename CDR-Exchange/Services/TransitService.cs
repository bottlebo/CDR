using CDR_Exchange.Dal;
using CDR_Exchange.Domain.Models;
using CDR_Exchange.Services.Cil.Interfaces;
using CDR_Exchange.Services.Dtos;
using CDR_Exchange.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CDR_Exchange.Services;

public class TransitService : ITransitService
{
    private readonly ICilWebApi _cilWebApi;
    private readonly ExchangeDbContext _dbContext;
    private readonly IPairsService _pairsService;

    public TransitService(ICilWebApi cilWebApi, ExchangeDbContext dbContext, IPairsService pairsService)
    {
        _cilWebApi = cilWebApi;
        _dbContext = dbContext;
        _pairsService = pairsService;
    }

    public async Task<string> CreateAsync(string address, PairDto pair, CancellationToken cancellationToken = default)
    {
        var response = await _cilWebApi.GetSwapAddressAsync(address, 
            pair.FromCoin, 
            pair.ToCoin, 
            pair.FromSymbol, 
            pair.ToSymbol, cancellationToken);

        var rate = await _cilWebApi.GetSwapRateAsync(pair.FromCoin, pair.ToCoin, pair.ToSymbol, pair.FromSymbol,
            cancellationToken);

        var entity = await _dbContext.TransitWallets.AddAsync(new TransitWallet
        {
            Address = response.Wallet,
            CoinFrom = pair.FromCoin,
            CoinTo = pair.ToCoin,
            TokenFrom = pair.FromSymbol,
            TokenTo = pair.ToSymbol,
            SwapAddress = address,
            CreateDate = DateTime.UtcNow,
            IsActive = true,
            Rate = new Rate
            {
                Fee = rate.Fee,
                Amount = rate.Rate,
                MinAmount = rate.MinAmount
            }
        }, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return entity.Entity.Address;
    }
    
    public async Task<TransitWalletRateDto?> GetRateByTransitAddressAsync(string transitAddress, CancellationToken cancellationToken)
    {
        var result = await _dbContext.TransitWallets
            .Include(x => x.Rate)
            .FirstOrDefaultAsync(x => x.Address == transitAddress, cancellationToken);

        if (result == null)
        {
            return null;
        }

        return new TransitWalletRateDto
        {
            SwapAddress = result.SwapAddress,
            TransitAddress = transitAddress,
            MinAmount = result.Rate.MinAmount,
            MinAmountWithFee = result.Rate.MinAmount + result.Rate.Fee,
            Amount = result.Rate.Amount,
            Fee = result.Rate.Fee,
            Pair = await _pairsService.GetPairOrThrowExceptionAsync(result, cancellationToken)
        };
    }
    
    
}