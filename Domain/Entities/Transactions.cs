using System.ComponentModel.DataAnnotations;

namespace ChuSAApi.Domain.Entities;

public class Transactions
{
    [Key]
    public int Id { get; set; }
    public decimal Value { get; set; }
    public int From_Account_id { get; set; }
    public int To_Account_id { get; set; }
    public DateTime CreationDate { get; set; }
}