using Server.Assemblers;
using Server.DTO;
using Server.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
  [Route("api/[controller]")]
  public class GameSettingsController : Controller
  {
    private readonly Domain.GameSettings _gameSettings;

    public GameSettingsController(Domain.GameSettings gameSettings)
    {
      _gameSettings = gameSettings;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
      List<DTO.GameSettings> dto_GameSettings = new List<DTO.GameSettings>();
      dto_GameSettings.Add(GameSettingsAssembler.ToDTO(_gameSettings));
      return Ok(dto_GameSettings.ToArray());
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromForm]DTO.GameSettings gameSettings)
    {
      if (ModelState.IsValid)
      {
        _gameSettings.Update(
          gameSettings.DriverCountdown,
          gameSettings.AutonomousCountdown, 
          gameSettings.Autonomous, 
          gameSettings.Delay, 
          gameSettings.Teleoperated, 
          gameSettings.EndGame
        );
        return new NoContentResult();
      }
      return BadRequest(ModelState);
    }
  }
}
