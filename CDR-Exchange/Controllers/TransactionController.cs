using CDR_Exchange.Infrastructure.Exceptions;
using CDR_Exchange.Services.Dtos;
using CDR_Exchange.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CDR_Exchange.Controllers;

/// <summary>
/// Controller API for working with transactions.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    /// <summary>
    /// Creates transaction
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(200, Type = typeof(TransactionDto))]
    [ProducesResponseType(400)]
    public async Task<ActionResult<TransactionDto>> Create([FromBody] CreateTransactionRequest request)
    {
        try
        {
            return await _transactionService.CreateAsync(request, HttpContext.RequestAborted);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
    /// <summary>
    /// Returns transaction by transit address
    /// </summary>
    /// <returns></returns>
    [HttpGet("by-transit-address/{transitAddress}")]
    [ProducesResponseType(200, Type = typeof(TransactionDto))]
    [ProducesResponseType(400)]
    public async Task<ActionResult<TransactionDto>> GetTransactionByTransitAddress([FromRoute] string transitAddress)
    {
        try
        {
            return await _transactionService.GetByTransitAddress(transitAddress, HttpContext.RequestAborted);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}