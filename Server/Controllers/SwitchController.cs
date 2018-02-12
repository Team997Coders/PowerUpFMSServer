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
  public class SwitchController : Controller
  {

    private readonly Game _game;

    public SwitchController(Game game)
    {
      _game = game;
    }

    /*
    [HttpGet]
    public IEnumerable<string> Get()
    {
      return new string[] { "value1", "value2" };
    }
    */
    
    [HttpGet("{alliance}")]
    public async Task<IActionResult> Get(string alliance)
    {
      if (Alliance.TryParse(alliance))
      {
        Alliance v_alliance = Alliance.Parse(alliance);
        DTO.Switch dto_Switch = new DTO.Switch();
        if (v_alliance.IsRed)
        {
          dto_Switch.Alliance = v_alliance.ToString();
          dto_Switch.LHSOwnsIt = _game.RedSwitchLHSPlateOwned;
          dto_Switch.RHSOwnsIt = _game.RedSwitchRHSPlateOwned;
        }
        if (v_alliance.IsBlue)
        {
          dto_Switch.Alliance = v_alliance.ToString();
          dto_Switch.LHSOwnsIt = _game.BlueSwitchLHSPlateOwned;
          dto_Switch.RHSOwnsIt = _game.BlueSwitchRHSPlateOwned;
        }
        return Ok(dto_Switch);
      }
      else
      {
        return NotFound(alliance);
      }
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromForm]DTO.Switch dto_Switch)
    {
      if (Alliance.TryParse(dto_Switch.Alliance))
      {
        Alliance alliance = Alliance.Parse(dto_Switch.Alliance);
        if (alliance.IsRed)
        {
          _game.RedSwitchLHSPlateOwned = dto_Switch.LHSOwnsIt;
          _game.RedSwitchRHSPlateOwned = dto_Switch.RHSOwnsIt;
        }
        else
        {
          _game.BlueSwitchLHSPlateOwned = dto_Switch.LHSOwnsIt;
          _game.BlueSwitchRHSPlateOwned = dto_Switch.RHSOwnsIt;
        }
        return new NoContentResult();
      }
      else
      {
        return BadRequest();          
      }
    }
  }
}
