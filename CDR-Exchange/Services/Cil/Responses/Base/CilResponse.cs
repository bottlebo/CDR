using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;

namespace CDR_Exchange.Services.Cil.Responses.Base;

public abstract class CilResponse
{
    private readonly string _response;

    protected CilResponse(string response)
    {
        _response = response;
        Fill();
    }

    private void Fill()
    {
        try
        {
            var obj = JObject.Parse(_response);
            var msg = obj.SelectToken("error.message")
                ?.Value<string>();
                
            if (!string.IsNullOrEmpty(msg))
            {
                throw new Exception(msg);
            }

            Parse(obj);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    protected abstract void Parse(JObject data);
}