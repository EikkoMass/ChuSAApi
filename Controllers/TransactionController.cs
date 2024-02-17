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
    private UserDbContext _db;

    public TransactionController(ILogger<TransactionController> logger, UserDbContext context)
    {
        _logger = logger;
        _db = context ?? throw new ArgumentNullException(nameof(context));
    }

    [HttpGet(Name = "GetTransaction")]
    public IActionResult GetTransactions()
    {
        return  Ok(_db.Transactions.ToList());
    }
    
    [HttpPost]
    public IActionResult AddNewTransaction(Transactions user)
    {
        var transactions = _db.Transactions.Add(user);
        _db.SaveChanges();

        return Ok(transactions.Entity);
    }
    
    // [HttpGet(ApiRoutes.Account.FindById)]
    // public IActionResult GetAccountById(int id)
    // {
    //     return Ok(_db.Transactions.Find(id));
    // }
    
    [HttpPut]
    public IActionResult UpdateAccount(Transactions transactions)
    {
        try {
            _db.Transactions.Update(transactions);
            _db.SaveChanges();
            
            return Ok(transactions);
        }
        catch (DbUpdateConcurrencyException error)
        {
            return NotFound("Nao foi possivel encontrar a transacao desejada");
        }
    }
    
    [HttpDelete(ApiRoutes.Account.DeleteById)]
    public IActionResult RemoveAccountById(int id)
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