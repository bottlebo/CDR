using CDR_Exchange.Services.Cil.Responses.Base;
using Newtonsoft.Json.Linq;

namespace CDR_Exchange.Services.Cil.Responses;

public class CilBalanceResponse : CilResponse
{
    public decimal Balance { get; private set; }

    public CilBalanceResponse(string response)
        : base(response)
    {
    }

    protected override void Parse(JObject data)
    {
        Balance = (decimal)data.SelectToken("balance");
    }
}