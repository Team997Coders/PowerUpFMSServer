using Microsoft.AspNetCore.Mvc;
using Server.Domain;
using Server.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Controllers
{
  [Route("api/[controller]")]
  public class ScaleController : Controller
  {

    private readonly Game _game;

    public ScaleController(Game game)
    {
      _game = game;
    }

    [HttpGet()]
    public async Task<IActionResult> Get()
    {
      DTO.Scale dto_Scale = new DTO.Scale();
      dto_Scale.LHSOwnsIt = _game.ScaleLHSPlateAsAllianceFieldLeftOwned;
      dto_Scale.RHSOwnsIt = _game.ScaleRHSPlateAsAllianceFieldLeftOwned;
      return Ok(dto_Scale);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromForm]DTO.Scale dto_Scale)
    {
      _game.ScaleLHSPlateAsAllianceFieldLeftOwned = dto_Scale.LHSOwnsIt;
      _game.ScaleRHSPlateAsAllianceFieldLeftOwned = dto_Scale.RHSOwnsIt;
      return new NoContentResult();
    }
  }
}
