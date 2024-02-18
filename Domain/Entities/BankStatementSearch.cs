namespace ChuSAApi.Domain.Entities;

public class BankStatementSearch
{
    public DateOnly ToDate { get; set; }
    public DateOnly FromDate { get; set; }
}