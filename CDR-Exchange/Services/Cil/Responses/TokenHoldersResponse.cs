using Newtonsoft.Json.Linq;

namespace CDR_Exchange.Services.Cil.Responses
{
    public class TokenHoldersResponse
    {
        public TokenHolders[] Data { get; private set; }

        public TokenHoldersResponse(string response)
        {
            Parse(JArray.Parse(response));
        }

        private void Parse(JArray data)
        {
            Data = data
                .Select(tx => new TokenHolders
                {
                    Symbol = tx.Value<string>("symbol"),
                    Address = tx.Value<string>("address"),
                    Quantity = tx.Value<decimal>("quantity")
                }).ToArray();
        }
    }

    public class TokenHolders
    {
        public string Symbol { get; set; }
        public string Address { get; set; }
        public decimal Quantity { get; set; }
    }
}