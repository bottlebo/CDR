using System.ComponentModel.DataAnnotations;

namespace CDR_Exchange.Domain.Models;

public class Transaction
{
    [Key]
    public Guid Id { get; set; }
    public Guid TransitWalletId { get; set; }
    public TransitWallet TransitWallet { get; set; }
    public string Address { get; set; }
    public decimal Amount { get; set; }
    public ICollection<TransactionHistory> TransactionHistories { get; set; }
    public DateTime CreateDate { get; set; }
}