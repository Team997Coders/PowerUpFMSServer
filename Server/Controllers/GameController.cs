using Server.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Server.Controllers
{
  [Route("api/[controller]")]
  public class GameController : Controller
  {
    private readonly FieldSettings _fieldSettings;
    private readonly GameSettings _gameSettings;
    private readonly Game _game;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private HttpResponse _response;
    private readonly ILogger _logger;

    public GameController(Game game, GameSettings gameSettings, FieldSettings fieldSettings, IHttpContextAccessor httpContextAccessor, ILogger<GameController> logger)
    {
      _fieldSettings = fieldSettings;
      _gameSettings = gameSettings;
      _game = game;
      _httpContextAccessor = httpContextAccessor;
      _logger = logger;
    }

    [HttpGet("state")]
    public async Task<IActionResult> GetState()
    {
      return Ok(_game.GameState.ToString());
    }

    [HttpGet]
    public async Task Get()
    {
      // get the http response so our event handlers can write to it
      _response = _httpContextAccessor.HttpContext.Response;

      // we are streaming messages...
      _response.Headers.Add("Content-Type", "text/event-stream");

      // completion flag
      ValueWrapper<bool> complete = new ValueWrapper<bool>(false);

      // spew out our messages
      StreamingMonitor streamingMonitor = new StreamingMonitor(complete, _game, _response);

      // make sure to dispose of our streaming monitor when this connection is done
      _response.RegisterForDispose(streamingMonitor);

      while(complete.Value == false) {
        if (_game.IsPlaying)
        {
          // TODO: For now, clear these values out manually...timing problems with client need to be fixed
          streamingMonitor.Clear();

          // wait until the game is done
          Task playingMonitor = new Task(() =>
            {
              while(_game.IsPlaying) {
                Thread.Sleep(100);
              };
            }
          );
          playingMonitor.Start();
          await playingMonitor;

          // Write the endgame message so client knows we are done...
          _response.WriteAsync($"data: {{\"EndGame\": 1}}\r\r").Wait();
          _response.Body.Flush();
        }
        await Task.Delay(200);
      }
    }

    [HttpPost("state/{state}")]
    public async Task<IActionResult> PostState([FromRoute]string state)
    {
      switch (state.ToUpper())
      {
        case "RESET":
        {
          _fieldSettings.FieldState = FieldStateEnum.GAME;
          await _game.Reset(_fieldSettings);
          break;
        }
        case "PLAY":
        {
          _fieldSettings.FieldState = FieldStateEnum.GAME;
          _game.Play(_gameSettings);
          break;
        }
        case "FAULT":
        {
          _fieldSettings.FieldState = FieldStateEnum.GAME;
          await _game.Fault();
          break;
        }
        default:
        {
          return BadRequest(state);
        }
      }
      return NoContent();
    }

    [HttpPost("vault")]
    public async Task<IActionResult> PostVaultScore([FromForm]string alliance, [FromForm]int score)
    {
      // Score in this context is 0, 1, 2 or 3...to 9, which is the number of cubes
      // The scoreboard will calculate with actual score.
      try
      {
        if (Alliance.Parse(alliance).IsRed)
        {
          if (_game.ScoreBoard != null)
          {
            _game.ScoreBoard.RedVaultCount = score;
          }
        }
        else
        {
          if (_game.ScoreBoard != null)
          {
            _game.ScoreBoard.BlueVaultCount = score;
          }
        }
        return NoContent();
      }
      catch
      {
        return BadRequest();
      }
    }

    [HttpPost("climb")]
    public async Task<IActionResult> PostClimbScore([FromForm]string alliance, [FromForm]int score)
    {
      // Score in this context is 0, 1, 2 or 3, which is the number of robots climbing
      // The scoreboard will calculate with actual score.
      try
      {
        if (Alliance.Parse(alliance).IsRed)
        {
          if (_game.ScoreBoard != null)
          {
            _game.ScoreBoard.RedClimbCount = score;
          }
        }
        else
        {
          if (_game.ScoreBoard != null)
          {
            _game.ScoreBoard.BlueClimbCount = score;
          }
        }
        return NoContent();
      }
      catch
      {
        return BadRequest();
      }      
    }

    [HttpPost("autorun")]
    public async Task<IActionResult> PostAutorunScore([FromForm]string alliance, [FromForm]int score)
    {
      // Score in this context is 0, 1, 2 or 3, which is the number of robots crossing the line during auto
      // The scoreboard will calculate with actual score.
      try
      {
        if (Alliance.Parse(alliance).IsRed)
        {
          if (_game.ScoreBoard != null)
          {
            _game.ScoreBoard.RedAutorunCount = score;
          }
        }
        else
        {
          if (_game.ScoreBoard != null)
          {
            _game.ScoreBoard.BlueAutorunCount = score;
          }
        }
        return NoContent();
      }
      catch
      {
        return BadRequest();
      }      
    }

    [HttpPost("park")]
    public async Task<IActionResult> PostParkScore([FromForm]string alliance, [FromForm]int score)
    {
      // Score in this context is 0, 1, 2 or 3, which is the number of robots parking on the platform
      // The scoreboard will calculate with actual score.
      try
      {
        if (Alliance.Parse(alliance).IsRed)
        {
          if (_game.ScoreBoard != null)
          {
            _game.ScoreBoard.RedParkCount = score;
          }
        }
        else
        {
          if (_game.ScoreBoard != null)
          {
            _game.ScoreBoard.BlueParkCount = score;
          }
        }
        return NoContent();
      }
      catch
      {
        return BadRequest();
      }      
    }
  }
}
