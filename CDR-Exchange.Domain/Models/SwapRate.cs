using System.ComponentModel.DataAnnotations;

namespace CDR_Exchange.Domain.Models;

public class Rate
{
    [Key]
    public Guid Id { get; set; }
    public Guid TransitWalletId { get; set; }
    public TransitWallet TransitWallet { get; set; }
    public decimal Amount { get; set; }
    public decimal MinAmount { get; set; }
    public decimal Fee { get; set; }
}