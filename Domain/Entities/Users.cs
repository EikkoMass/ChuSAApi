using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace ChuSAApi.Domain.Entities;

[Table("Accounts")]
public class Users
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime creationDate { get; set; }
    public decimal currency { get; set; }
}