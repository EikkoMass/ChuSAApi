using ChuSAApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace ChuSAApi.db;

public class UserDbContext : DbContext {

  private IConfiguration _configuration;
  public DbSet<Users> Users { get; set; }
  public DbSet<Transactions> Transactions { get; set; }
  
  public DbSet<BankStatement> BankStatement { get; set; }

  public UserDbContext(IConfiguration configuration, DbContextOptions options) : base(options)
  {
    _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    var typeDatabase = _configuration["TypeDatabase"];
    var connectionString = _configuration.GetConnectionString(typeDatabase);

    if (typeDatabase == "Mysql")
    {
      optionsBuilder.UseMySQL(connectionString);
    }
  }

}