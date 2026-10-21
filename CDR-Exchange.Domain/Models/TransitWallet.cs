using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CDR_Exchange.Domain.Models;

[Index(nameof(Address), IsUnique = true)]
public class TransitWallet
{
    [Key]
    public Guid Id { get; set; }
    /// <summary>
    /// Cil Transition wallet hash
    /// </summary>
    public string Address { get; set; }
    /// <summary>
    /// Wallet id
    /// </summary>
    public string SwapAddress { get; set; }
    /// <summary>
    /// CoinFrom
    /// </summary>
    public string CoinFrom { get; set; }
    /// <summary>
    /// CoinTo
    /// </summary>
    public string CoinTo { get; set; }
    /// <summary>
    /// TokenFrom
    /// </summary>
    public string TokenFrom { get; set; }
    /// <summary>
    /// TokenTo
    /// </summary>
    public string TokenTo { get; set; }
    public bool IsActive { get; set; }
    public Rate Rate { get; set; }
    public Transaction? Transaction { get; set; }
    public DateTime CreateDate { get; set; }
    
}