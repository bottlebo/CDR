using CDR_Exchange.Services.Dtos;
using CDR_Exchange.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CDR_Exchange.Controllers;

/// <summary>
/// Controller API for working with user swap.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TransitController : ControllerBase
{
    private readonly ITransitService _transitService;

    public TransitController(ITransitService transitService)
    {
        _transitService = transitService;
    }
    
    /// <summary>
    /// Creates swap
    /// </summary>
    /// <returns></returns>
    [HttpPost("{address}")]
    [ProducesResponseType(200, Type = typeof(string))]
    [ProducesResponseType(400)]
    public async Task<ActionResult<string>> CreateTransit([FromRoute] string address, [FromBody] PairDto pair)
    {
        var result = await _transitService.CreateAsync(address, pair, HttpContext.RequestAborted);
        return Ok(result);
    }


    /// <summary>
    /// Gets swap by transit wallet
    /// </summary>
    /// <returns></returns>
    [HttpGet("{transitAddress}")]
    [ProducesResponseType(200, Type = typeof(TransitWalletRateDto))]
    [ProducesResponseType(400)]
    public async Task<ActionResult<TransitWalletRateDto>> GetRateByTransitWallet([FromRoute] string transitAddress)
    {
        var result = await _transitService.GetRateByTransitAddressAsync(transitAddress, HttpContext.RequestAborted);
        if (result == null)
        {
            return NotFound();
        }

        return result;
    }
}