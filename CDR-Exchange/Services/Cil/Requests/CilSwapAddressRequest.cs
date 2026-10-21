using System.Dynamic;
using System.Text.Json;

namespace CDR_Exchange.Services.Cil.Requests;

public class CilSwapAddressRequest
{
    private readonly object _requestModel;
        
    public CilSwapAddressRequest(string strAddress, string strCoinFrom, string strCoinTo, string strSymbolTo, string strSymbolFrom)
    {
        dynamic dynObject = new ExpandoObject();
        dynObject.strAddress = strAddress;

        if (!string.IsNullOrEmpty(strCoinFrom))
        {
            dynObject.strCoinFrom = strCoinFrom;
        }
        if (!string.IsNullOrEmpty(strCoinTo))
        {
            dynObject.strCoinTo = strCoinTo;
        }
        if (!string.IsNullOrEmpty(strSymbolTo))
        {
            dynObject.strSymbolTo = strSymbolTo;
        }
        if (!string.IsNullOrEmpty(strSymbolFrom))
        {
            dynObject.strSymbolFrom = strSymbolFrom;
        }
            
        _requestModel = dynObject;
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(_requestModel);
    }
}