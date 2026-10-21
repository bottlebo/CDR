namespace CDR_Exchange.Services.Dtos;

public class PairDto
{
    public string FromCoin { get; set; }
    public string FromSymbol { get; set; }
    public string FromDisplayName { get; set; }
    public string ToCoin { get; set; }
    public string ToSymbol { get; set; }
    public string ToDisplayName { get; set; }
    public string DisplayName { get; set; }
}