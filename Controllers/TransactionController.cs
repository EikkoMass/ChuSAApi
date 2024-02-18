using ChuSAApi.db;
using ChuSAApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChuSAApi.Controllers;

[ApiController]
[Route("api/v1/transaction/")]
public class TransactionController : ControllerBase
{
    private readonly ILogger<TransactionController> _logger;
    private readonly UserDbContext _db;

    public TransactionController(ILogger<TransactionController> logger, UserDbContext context)
    {
        _logger = logger;
        _db = context ?? throw new ArgumentNullException(nameof(context));
    }

    [HttpGet(Name = "GetTransaction")]
    public IActionResult Get()
    {
        return  Ok(_db.Transactions.ToList());
    }
    
    [HttpPost]
    public IActionResult AddNew(Transactions transaction)
    {
        var transactions = _db.Transactions.Add(transaction);
        _db.SaveChanges();

        //TODO ao fazer uma transacao, alterar o valor da conta dos usuarios correspondentes
        
        return Ok(transactions.Entity);
    }
    
    [HttpGet(ApiRoutes.FindById)]
    public IActionResult FindById(int id)
    {
        return Ok(_db.Transactions.Find(id));
    }
    
    [HttpPut]
    public IActionResult Update(Transactions transaction)
    {
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