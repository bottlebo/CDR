using Newtonsoft.Json.Linq;

namespace CDR_Exchange.Services.Cil.Responses
{
    public class CilWalletListUnspentResponse
    {
        public CilWalletListUnspentItem[] Items { get; set; }

        public CilWalletListUnspentResponse(string response)
        {
            Parse(JArray.Parse(response));
        }

        private void Parse(JArray data)
        {
            Items = data
                .Select(tx => new CilWalletListUnspentItem
                {
                    Hash = tx.Value<string>("hash"), Amount = tx.Value<long>("amount"), NOut = tx.Value<long>("nOut"),
                    IsStable = tx.Value<bool>("isStable")
                }).ToArray();
        }

        public class CilWalletListUnspentItem
        {
            public long Amount { get; set; }
            public string Hash { get; set; }
            public long NOut { get; set; }
            public bool IsStable { get; set; }
        }
    }
}