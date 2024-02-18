using ChuSAApi.db;
using ChuSAApi.Domain.Entities;
using ChuSAApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ChuSAApi.Controllers;

[ApiController]
[Route(ApiRoutes.V1 + "transaction/")]
public class TransactionController : ControllerBase
{
    private readonly ILogger<TransactionController> _logger;
    private readonly UserDbContext _db;
    private readonly TransactionService _service;
    private readonly IMemoryCache _cache;

    public TransactionController(ILogger<TransactionController> logger, UserDbContext context, IMemoryCache cache)
    {
        _logger = logger;
        _db = context ?? throw new ArgumentNullException(nameof(context));
        _service = new TransactionService();
        _cache = cache;
    }

    [HttpGet(Name = "GetTransaction")]
    [ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
    public IActionResult Get()
    {
        var transactionList = _cache.GetOrCreate("GetAllTransactions", entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(15);
            return _db.Transactions.ToList();
        });
        
        return  Ok(transactionList);
    }
    
    [HttpPost]
    public IActionResult Create(Transactions transaction)
    {
        if (_service.isHoliday(transaction.CreationDate).Result)
        {
            return NotFound("data invalida, so e possivel fazer transacoes em dias uteis");
        }
        
        var transactions = _db.Transactions.Add(transaction);
        _db.SaveChanges();

        //TODO ao fazer uma transacao, alterar o valor da conta dos usuarios correspondentes
        
        return Ok(transactions.Entity);
    }
    
    [HttpGet(ApiRoutes.FindById)]
    public IActionResult FindById(int id)
    {
        var transaction = _cache.GetOrCreate("FindTransaction_" + id, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(15);
            return _db.Transactions.Find(id);
        });
        
        return Ok(transaction);
    }
    
    [HttpPut]
    public IActionResult Update(Transactions transaction)
    {
        if (_service.isHoliday(transaction.CreationDate).Result)
        {
            return NotFound("data invalida, so e possivel fazer transacoes em dias uteis");
        }
        
        try {
            _db.Transactions.Update(transaction);
            _db.SaveChanges();
            
            return Ok(transaction);
        }
        catch (DbUpdateConcurrencyException error)
        {
            return NotFound("Nao foi possivel encontrar a transacao desejada");
        }
    }
    
    [HttpDelete(ApiRoutes.DeleteById)]
    public IActionResult RemoveById(int id)
    {
        try
        {
            var transactions = new Transactions() { Id = id };
            var data = _db.Transactions.Remove(transactions);
            _db.SaveChanges();

            return Ok("transacao deletada com sucesso");
        }
        catch (DbUpdateConcurrencyException error)
        {
            return NotFound("Nao foi possivel encontrar a transacao desejada");
        }
    }
}