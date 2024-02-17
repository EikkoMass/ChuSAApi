using Microsoft.AspNetCore.Mvc;
using ChuSAApi.db;
using ChuSAApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChuSAApi.Controllers;

[ApiController]
[Route("api/v1/account/")]
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
    public IActionResult GetAccounts()
    {
        return  Ok(_db.Users.ToList());
    }
    
    [HttpPost]
    public IActionResult AddNewAccount(Users user)
    {
        var users = _db.Users.Add(user);
        _db.SaveChanges();

        return Ok(users.Entity);
    }
    
    [HttpGet(ApiRoutes.Account.FindById)]
    public IActionResult GetAccountById(int id)
    {
        return Ok(_db.Users.Find(id));
    }
    
    [HttpPut]
    public IActionResult UpdateAccount(Users user)
    {
        try
        {
            _db.Users.Update(user);
            _db.SaveChanges();

            return Ok(user);
        }
        catch (DbUpdateConcurrencyException error)
        {
            return NotFound("Nao foi possivel encontrar a conta desejada");
        }
    }
    
    [HttpDelete(ApiRoutes.Account.DeleteById)]
    public IActionResult RemoveAccountById(int id)
    {
        try {
            var user = new Users() { Id = id };
            _db.Users.Remove(user);
            _db.SaveChanges();

            return Ok("conta deletada com sucesso");
        }
        catch (DbUpdateConcurrencyException error)
        {
            return NotFound("Nao foi possivel encontrar a conta desejada");
        }
    }
}
