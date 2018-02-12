using Server.Domain;
using Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    public class FieldController : Controller
    {
    private readonly FieldSettings _fieldSettings;
    private readonly IGameService _gameService;

    public FieldController(FieldSettings fieldSettings, IGameService gameService)
    {
      _fieldSettings = fieldSettings;
      _gameService = gameService;
    }

    [HttpGet("state")]
    public async Task<IActionResult> GetState()
    {
      return Ok(_fieldSettings.FieldState.ToString());
    }

    [HttpGet("fieldstring")]
    public async Task<IActionResult> GetFieldString()
    {
      return Ok(_fieldSettings.AutonomousFieldString);
    }

    [HttpPost("{state}")]
    public async Task<IActionResult> Post(string state)
    {
      if (_gameService.IsPlaying)
      {
        return BadRequest(state);
      }
      else
      {
        Field field = new Field(_fieldSettings);
        switch (state.ToUpper())
        {
          case "SAFE":
          {
            await field.Safe();
            _fieldSettings.FieldState = FieldStateEnum.SAFE;
            break;
          }
          case "STAFFSAFE":
          {
            await field.StaffSafe();
            _fieldSettings.FieldState = FieldStateEnum.STAFFSAFE;
            break;
          }
          case "OFF":
          {
            await field.Off();
            _fieldSettings.FieldState = FieldStateEnum.OFF;
            break;
          }
          default:
          {
            return BadRequest(state);
          }
        }
        return NoContent();
      }
    }
  }
}
