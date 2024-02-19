using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ChuSAApi.Domain.Entities;

[Keyless]
[Table("Statements")]
public class BankStatement
{
    public string toUserName { get; set; }
    public string fromUserName { get; set; }
    public DateTime creationDate { get; set; }
    public decimal value { get; set; }
}