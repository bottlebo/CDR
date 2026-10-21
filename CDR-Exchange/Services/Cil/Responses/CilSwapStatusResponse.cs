using CDR_Exchange.Services.Cil.Responses.Base;
using Newtonsoft.Json.Linq;

namespace CDR_Exchange.Services.Cil.Responses;

public class CilSwapStatusResponse : CilResponse
{
    public string Status { get; private set; }

    public CilSwapStatusResponse(string response)
        : base(response)
    {
    }

    protected override void Parse(JObject data)
    {
        Status = (string) data.SelectToken("result");
    }
}