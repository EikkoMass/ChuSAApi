using ChuSAApi.db;
using ChuSAApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ChuSAApi.Controllers;

[ApiController]
[Route(ApiRoutes.V1 + "bankStatement/")]
public class BankStatementController : ControllerBase
{
    private readonly ILogger<BankStatementController> _logger;
    private readonly UserDbContext _db;
    private readonly IMemoryCache _cache;
    
    public BankStatementController(ILogger<BankStatementController> logger, UserDbContext context, IMemoryCache cache)
    {
        _logger = logger;
        _db = context ?? throw new ArgumentNullException(nameof(context));
        _cache = cache;
    }
    
    [HttpGet]
    [ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
    public IActionResult Get()
    {
        var statementList = _cache.GetOrCreate("GetAllStatements", entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(15);
            return _db.BankStatement.ToList();
        });
        return Ok(statementList);
    }
    
    [HttpPost("between")]
    public IActionResult GetBetween(BankStatementSearch search)
    {
        var toDate = search.ToDate.ToDateTime(TimeOnly.MinValue);
        var fromDate = search.FromDate.ToDateTime(TimeOnly.MaxValue);
        
        var statements = _db.BankStatement.Where(bs => bs.creationDate >= toDate && bs.creationDate <= fromDate);
        
        return Ok(statements.ToList());
    }
}