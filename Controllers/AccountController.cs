using Microsoft.AspNetCore.Mvc;
using ChuSAApi.db;
using ChuSAApi.Domain.Entities;

namespace ChuSAApi.Controllers;

[ApiController]
[Route("api/v1/account/[controller]")]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private UserDbContext _db;

    public AccountController(ILogger<AccountController> logger, UserDbContext context)
    {
        _logger = logger;
        _db = context ?? throw new ArgumentNullException(nameof(context));
    }

    [HttpGet(Name = "GetAccount")]
    public IActionResult Get()
    {
        return  Ok(_db.Users.ToList());
    }
    
    [HttpPost]
    public IActionResult Add(Users user)
    {
        var users = _db.Users.Add(user);
        _db.SaveChanges();

        return Ok(users.Entity);
    }
}
