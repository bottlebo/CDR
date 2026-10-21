using CDR_Exchange.Services.Cil.Responses.Base;
using Newtonsoft.Json.Linq;

namespace CDR_Exchange.Services.Cil.Responses;

public class CilSwapResponse : CilResponse
{
    public string Wallet { get; private set; }

    public CilSwapResponse(string response)
        : base(response)
    {
    }

    protected override void Parse(JObject data)
    {
        Wallet = (string) data.SelectToken("result");
    }
}