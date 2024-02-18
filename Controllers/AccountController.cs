using Microsoft.AspNetCore.Mvc;
using ChuSAApi.db;
using ChuSAApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ChuSAApi.Controllers;

[ApiController]
[Route(ApiRoutes.V1 + "account/")]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly UserDbContext _db;
    private readonly IMemoryCache _cache;

    public AccountController(ILogger<AccountController> logger, UserDbContext context, IMemoryCache cache)
    {
        _logger = logger;
        _db = context ?? throw new ArgumentNullException(nameof(context));
        _cache = cache;
    }

    [HttpGet]
    [ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
    public IActionResult Get()
    {
        var userList = _cache.GetOrCreate("GetAllAccounts", entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(15);
            return _db.Users.ToList();
        });
        
        return  Ok(userList);
    }
    
    [HttpPost]
    public IActionResult AddNew(Users user)
    {
        var users = _db.Users.Add(user);
        _db.SaveChanges();

        return Ok(users.Entity);
    }
    
    [HttpGet(ApiRoutes.FindById)]
    public IActionResult FindById(int id)
    {
        var user = _cache.GetOrCreate("FindAccount_" + id, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(15);
            return _db.Users.Find(id);
        });
        
        return Ok(user);
    }
    
    [HttpPut]
    public IActionResult Update(Users user)
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
    
    [HttpDelete(ApiRoutes.DeleteById)]
    public IActionResult RemoveById(int id)
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
