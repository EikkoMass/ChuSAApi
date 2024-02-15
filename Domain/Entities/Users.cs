using System.ComponentModel.DataAnnotations;

namespace ChuSAApi.Domain.Entities;

public class Users
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
}