using System.Globalization;
using CDR_Exchange.Services.Cil.Responses.Base;
using Newtonsoft.Json.Linq;

namespace CDR_Exchange.Services.Cil.Responses;

public class SwapRateResponse : CilResponse
{
    public decimal Rate { get; private set; }
    public decimal MinAmount { get; private set; }

    public decimal Fee { get; private set; }

    public decimal MinAmountWithFee { get; private set; }

    public SwapRateResponse(string response)
        : base(response)
    {
    }

    protected override void Parse(JObject data)
    {
        Rate = data.SelectToken("nRate")?.Value<decimal>() ?? 0;
        MinAmount = data.SelectToken("nMinAmount")?.Value<decimal>() ?? 0;
        Fee = data.SelectToken("nFee")?.Value<decimal>() ?? 0;
        MinAmountWithFee = MinAmount + Fee;
    }
}