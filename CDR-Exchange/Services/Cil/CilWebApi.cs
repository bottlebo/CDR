using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using CDR_Exchange.Services.Cil.Interfaces;
using CDR_Exchange.Services.Cil.Requests;
using CDR_Exchange.Services.Cil.Responses;

namespace CDR_Exchange.Services.Cil;

public class CilWebApi : ICilWebApi
{
    private readonly HttpClient _httpClient;

    public CilWebApi(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetPairsAsync(CancellationToken cancellationToken = default)
    {
        var httpResponseMessage = await _httpClient.GetAsync("/swap/pairs", cancellationToken);
        return await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
    }

    public async Task<CilSwapResponse> GetSwapAddressAsync(string hashAddress, string coinFrom, string coinTo, string symbolFrom, string symbolTo, CancellationToken cancellationToken = default)
    {
        var request = new CilSwapAddressRequest(hashAddress, coinFrom, coinTo, symbolTo, symbolFrom);
            
        var content = new StringContent(request.ToString());
        content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Json);
            
        var httpResponseMessage = await _httpClient.PostAsync("swap/receiver/", content, cancellationToken);
        var result = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);

        return new CilSwapResponse(result);
    }

    public async Task<CilSwapStatusResponse> GetSwapStatusAsync(string hashAddress, CancellationToken cancellationToken)
    {
        var url = $"swap/status/?addr={hashAddress}";

        var httpResponseMessage = await _httpClient.GetAsync(url, cancellationToken);
        var result = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);

        return new CilSwapStatusResponse(result);
    }

    public async Task<SwapRateResponse> GetSwapRateAsync(string coinFrom, string coinTo, string symbolTo, string symbolFrom,
        CancellationToken cancellationToken = default)
    {
        var urlStr = new StringBuilder("swap/rate/?");

        if (!string.IsNullOrEmpty(coinFrom))
        {
            urlStr.Append($"strCoinFrom={coinFrom}&");
        }
        if (!string.IsNullOrEmpty(coinTo))
        {
            urlStr.Append($"strCoinTo={coinTo}&");
        }
        if (!string.IsNullOrEmpty(symbolTo))
        {
            urlStr.Append($"strSymbolTo={symbolTo}&");
        }
        if (!string.IsNullOrEmpty(symbolFrom))
        {
            urlStr.Append($"strSymbolFrom={symbolFrom}&");
        }

        var url = urlStr.ToString().TrimEnd('&');

        var httpResponseMessage = await _httpClient.GetAsync(url, cancellationToken);
        var result = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
            
        return new SwapRateResponse(result);
    }
}