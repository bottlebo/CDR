namespace CDR_Exchange.Domain.Enums;

public enum TransactionStatus
{
    Unknown = 0,
    Created,
    Send,
    Confirmed,
    Cancelled,
    Reactivated
}