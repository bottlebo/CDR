using System.ComponentModel.DataAnnotations;
using CDR_Exchange.Domain.Enums;

namespace CDR_Exchange.Domain.Models;

public class TransactionHistory
{
    [Key]
    public Guid Id { get; set; }
    public Guid TransactionId { get; set; }
    public Transaction Transaction { get; set; }
    public DateTime CreateDate { get; set; }
    public TransactionStatus Status { get; set; }
}