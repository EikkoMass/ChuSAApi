using ChuSAApi.db;
using ChuSAApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ChuSAApi.Controllers;

[ApiController]
[Route("api/v1/bankStatement/")]
public class BankStatementController : ControllerBase
{
    private readonly ILogger<BankStatementController> _logger;
    private readonly UserDbContext _db;
    
    public BankStatementController(ILogger<BankStatementController> logger, UserDbContext context)
    {
        _logger = logger;
        _db = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_db.BankStatement.ToList());
    }
    
    [HttpPost("between")]
    public IActionResult Get(BankStatementSearch search)
    {
        var toDate = search.ToDate.ToDateTime(TimeOnly.MinValue);
        var fromDate = search.FromDate.ToDateTime(TimeOnly.MaxValue);
        
        var statements = _db.BankStatement.Where(bs => bs.creationDate >= toDate && bs.creationDate <= fromDate);
        
        return Ok(statements.ToList());
    }
}