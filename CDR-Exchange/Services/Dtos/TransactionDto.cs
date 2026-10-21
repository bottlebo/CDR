using CDR_Exchange.Domain.Enums;
using CDR_Exchange.Domain.Models;

namespace CDR_Exchange.Services.Dtos;

public class TransactionDto
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreateDate { get; set; }
    public string TransitAddress { get; set; }
    public string SwapAddress { get; set; }
    public string TransactionAddress { get; set; }
    public TransactionStatus Status { get; set; }
    public PairDto Pair { get; set; }
}