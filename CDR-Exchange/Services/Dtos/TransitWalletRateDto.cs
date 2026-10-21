namespace CDR_Exchange.Services.Dtos;

public class TransitWalletRateDto
{
    public string SwapAddress { get; set; }
    public string TransitAddress { get; set; }
    public decimal Amount { get; set; }
    public decimal MinAmount { get; set; }
    public decimal Fee { get; set; }
    public decimal MinAmountWithFee { get; set; }
    public PairDto Pair { get; set; }
}