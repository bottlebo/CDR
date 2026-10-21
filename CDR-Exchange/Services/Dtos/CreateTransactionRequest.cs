namespace CDR_Exchange.Services.Dtos;

public class CreateTransactionRequest
{
    public string Address { get; set; }
    public string TransitAddress { get; set; }
    public decimal Amount { get; set; }
}