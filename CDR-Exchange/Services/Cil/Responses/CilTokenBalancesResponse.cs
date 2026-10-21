using Newtonsoft.Json.Linq;

namespace CDR_Exchange.Services.Cil.Responses
{
    public class CilTokenBalancesResponse
    {
        public CilTokenBalance[] Data { get; private set; }

        public CilTokenBalancesResponse(string response)
        {
            Parse(JArray.Parse(response));
        }

        private void Parse(JArray data)
        {
            Data = data
                .Select(tx => new CilTokenBalance
                {
                    Symbol = tx.Value<string>("symbol"),
                    Balance = tx.Value<decimal>("balance")
                }).ToArray();
        }
    }

    public class CilTokenBalance
    {
        public string Symbol { get; set; }
        public decimal Balance { get; set; }
    }
}