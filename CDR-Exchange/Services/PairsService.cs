using System.Text.Json;
using CDR_Exchange.Dal;
using CDR_Exchange.Domain.Models;
using CDR_Exchange.Infrastructure.Exceptions;
using CDR_Exchange.Services.Cil.Interfaces;
using CDR_Exchange.Services.Dtos;
using CDR_Exchange.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CDR_Exchange.Services;

public class PairsService : IPairsService
{
    private readonly ICilWebApi _cilWebApi;
    private readonly ExchangeDbContext _dbContext;

    public PairsService(ICilWebApi cilWebApi, ExchangeDbContext dbContext)
    {
        _cilWebApi = cilWebApi;
        _dbContext = dbContext;
    }

    public async Task<List<PairDto>> GetPairsAsync(CancellationToken cancellationToken = default)
    {
        var pairResponse = await _cilWebApi.GetPairsAsync(cancellationToken);
        return ParsePairs(pairResponse);
    }
    
    public async Task<PairDto> GetPairOrThrowExceptionAsync(TransitWallet transitWallet, CancellationToken cancellationToken = default)
    {
        var pairs = await GetPairsAsync(cancellationToken);
        var pair = pairs.FirstOrDefault(x => x.ToSymbol == transitWallet.TokenTo &&
                                             x.ToCoin == transitWallet.CoinTo &&
                                             x.FromSymbol == transitWallet.TokenFrom &&
                                             x.FromCoin == transitWallet.CoinFrom);

        if (pair == null)
        {
            throw new NotFoundException("Pair not found.");
        }

        return pair;
    }

    public async Task<PairDto> GetPairByTransitAddressOrThrowExceptionAsync(string transitAddress, CancellationToken cancellationToken = default)
    {
        var transitWallet =
            await _dbContext.TransitWallets.FirstOrDefaultAsync(x => x.Address == transitAddress, cancellationToken);

        if (transitWallet == null)
        {
            throw new NotFoundException($"Transit wallet not found. Wallet address: {transitAddress}");
        }

        return await GetPairOrThrowExceptionAsync(transitWallet, cancellationToken);
    }

    private static List<PairDto> ParsePairs(string pairResponse)
    {
        var pairResponseObject = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string[]>>>(pairResponse);

        var result = new List<PairDto>();

        foreach (var response in pairResponseObject!)
        {
            var fromKey = ConvertKey(response.Key);
            
            foreach (var from in response.Value)
            {
                var fromItems = GetItems(from.Key);
                
                foreach (var to in from.Value)
                {
                    var toItems = GetItems(to);

                    var toKey = ConvertKey(toItems[0]);

                    var pair = new PairDto
                    {
                        FromCoin = fromItems[0],
                        FromSymbol = fromItems[1],
                        FromDisplayName = $"{(string.IsNullOrEmpty(fromItems[1]) ? "UBX" : fromItems[1])} ({fromKey})",
                        ToCoin = toItems[0],
                        ToSymbol = toItems[1],
                        ToDisplayName = $"{(string.IsNullOrEmpty(toItems[1]) ? "UBX" : toItems[1])} ({toKey})",

                    };

                    pair.DisplayName = $"{pair.FromDisplayName} - {pair.ToDisplayName}";

                    result.Add(pair);
                }
            }
        }
        
        return result;
    }
    
    private static string ConvertKey(string key) => key == "LAW"
        ? "Native"
        : key;

    private static string[] GetItems(string items)
    {
        var result = items.Split('>');
        if (result.Length != 1)
        {
            return result;
        }
        
        var list = result.ToList();
        list.Add(string.Empty);

        result = list.ToArray();

        return result;
    }
}