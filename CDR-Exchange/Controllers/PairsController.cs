using CDR_Exchange.Infrastructure.Exceptions;
using CDR_Exchange.Services.Dtos;
using CDR_Exchange.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CDR_Exchange.Controllers;

/// <summary>
/// Controller API for working with pairs.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PairsController : ControllerBase
{
    private readonly IPairsService _pairsService;

    public PairsController(IPairsService pairsService)
    {
        _pairsService = pairsService;
    }

    /// <summary>
    /// Gets pairs
    /// </summary>
    /// <returns></returns>
    [HttpGet("list")]
    [ProducesResponseType(200, Type = typeof(List<PairDto>))]
    [ProducesResponseType(400)]
    public async Task<ActionResult<List<PairDto>>> GetPairs() => await _pairsService.GetPairsAsync(HttpContext.RequestAborted);

    /// <summary>
    /// Gets pair by transit address
    /// </summary>
    /// <returns></returns>
    [HttpGet("by-transit-address/{transitAddress}")]
    [ProducesResponseType(200, Type = typeof(PairDto))]
    [ProducesResponseType(400)]
    public async Task<ActionResult<PairDto>> GetPairByTransitAddress([FromRoute] string transitAddress)
    {
        try
        {
            return await _pairsService.GetPairByTransitAddressOrThrowExceptionAsync(transitAddress,
                    HttpContext.RequestAborted);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}