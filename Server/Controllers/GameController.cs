using Server.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

    public GameController(Game game, GameSettings gameSettings, FieldSettings fieldSettings, IHttpContextAccessor httpContextAccessor)
    {
      _fieldSettings = fieldSettings;
      _gameSettings = gameSettings;
      _game = game;
      _httpContextAccessor = httpContextAccessor;
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

      if (_game.IsPlaying)
      {
        // TODO: For now, clear these values out manually...timing problems with client need to be fixed
        OnBlueOwnershipSecondsUpdated(null, new BlueOwnershipSecondsUpdatedEventArgs{BlueOwnershipSeconds=0});
        OnBlueSwitchOwnershipSecondsUpdated(null, new BlueSwitchOwnershipSecondsUpdatedEventArgs{BlueSwitchOwnershipSeconds=0});
        OnBlueScaleOwnershipSecondsUpdated(null, new BlueScaleOwnershipSecondsUpdatedEventArgs{BlueScaleOwnershipSeconds=0});
        OnBlueVaultScoreUpdated(null, new BlueVaultScoreUpdatedEventArgs{BlueVaultScore=0});
        OnBlueParkScoreUpdated(null, new BlueParkScoreUpdatedEventArgs{BlueParkScore=0});
        OnBlueAutorunScoreUpdated(null, new BlueAutorunScoreUpdatedEventArgs{BlueAutorunScore=0});
        OnBlueClimbScoreUpdated(null, new BlueClimbScoreUpdatedEventArgs{BlueClimbScore=0});
        OnRedOwnershipSecondsUpdated(null, new RedOwnershipSecondsUpdatedEventArgs{RedOwnershipSeconds=0});
        OnRedSwitchOwnershipSecondsUpdated(null, new RedSwitchOwnershipSecondsUpdatedEventArgs{RedSwitchOwnershipSeconds=0});
        OnRedScaleOwnershipSecondsUpdated(null, new RedScaleOwnershipSecondsUpdatedEventArgs{RedScaleOwnershipSeconds=0});
        OnRedVaultScoreUpdated(null, new RedVaultScoreUpdatedEventArgs{RedVaultScore=0});
        OnRedParkScoreUpdated(null, new RedParkScoreUpdatedEventArgs{RedParkScore=0});
        OnRedAutorunScoreUpdated(null, new RedAutorunScoreUpdatedEventArgs{RedAutorunScore=0});
        OnRedClimbScoreUpdated(null, new RedClimbScoreUpdatedEventArgs{RedClimbScore=0});

        // wire up event handlers
        _game.ScoreBoard.ElapsedSecondsUpdated += OnElapsedSecondsUpdated;
        _game.ScoreBoard.BlueOwnershipSecondsUpdated += OnBlueOwnershipSecondsUpdated;
        _game.ScoreBoard.BlueSwitchOwnershipSecondsUpdated += OnBlueSwitchOwnershipSecondsUpdated;
        _game.ScoreBoard.BlueScaleOwnershipSecondsUpdated += OnBlueScaleOwnershipSecondsUpdated;
        _game.ScoreBoard.BlueVaultScoreUpdated += OnBlueVaultScoreUpdated;
        _game.ScoreBoard.BlueParkScoreUpdated += OnBlueParkScoreUpdated;
        _game.ScoreBoard.BlueAutorunScoreUpdated += OnBlueAutorunScoreUpdated;
        _game.ScoreBoard.BlueClimbScoreUpdated += OnBlueClimbScoreUpdated;
        _game.ScoreBoard.RedOwnershipSecondsUpdated += OnRedOwnershipSecondsUpdated;
        _game.ScoreBoard.RedSwitchOwnershipSecondsUpdated += OnRedSwitchOwnershipSecondsUpdated;
        _game.ScoreBoard.RedScaleOwnershipSecondsUpdated += OnRedScaleOwnershipSecondsUpdated;
        _game.ScoreBoard.RedVaultScoreUpdated += OnRedVaultScoreUpdated;
        _game.ScoreBoard.RedParkScoreUpdated += OnRedParkScoreUpdated;
        _game.ScoreBoard.RedAutorunScoreUpdated += OnRedAutorunScoreUpdated;
        _game.ScoreBoard.RedClimbScoreUpdated += OnRedClimbScoreUpdated;
        _game.ScoreBoard.StateOfPlayUpdated += OnStateOfPlayUpdated;

        // wait until the game is done
        SpinWait.SpinUntil(() => !_game.IsPlaying);

        _response.WriteAsync($"data: {{\"EndGame\": 1}}\r\r").Wait();
        _response.Body.Flush();

        _game.ScoreBoard.ElapsedSecondsUpdated -= OnElapsedSecondsUpdated;
        _game.ScoreBoard.BlueOwnershipSecondsUpdated -= OnBlueOwnershipSecondsUpdated;
        _game.ScoreBoard.BlueSwitchOwnershipSecondsUpdated -= OnBlueSwitchOwnershipSecondsUpdated;
        _game.ScoreBoard.BlueScaleOwnershipSecondsUpdated -= OnBlueScaleOwnershipSecondsUpdated;
        _game.ScoreBoard.BlueVaultScoreUpdated -= OnBlueVaultScoreUpdated;
        _game.ScoreBoard.BlueParkScoreUpdated -= OnBlueParkScoreUpdated;
        _game.ScoreBoard.BlueAutorunScoreUpdated -= OnBlueAutorunScoreUpdated;
        _game.ScoreBoard.BlueClimbScoreUpdated -= OnBlueClimbScoreUpdated;
        _game.ScoreBoard.RedOwnershipSecondsUpdated -= OnRedOwnershipSecondsUpdated;
        _game.ScoreBoard.RedSwitchOwnershipSecondsUpdated -= OnRedSwitchOwnershipSecondsUpdated;
        _game.ScoreBoard.RedScaleOwnershipSecondsUpdated -= OnRedScaleOwnershipSecondsUpdated;
        _game.ScoreBoard.RedVaultScoreUpdated -= OnRedVaultScoreUpdated;
        _game.ScoreBoard.RedParkScoreUpdated -= OnRedParkScoreUpdated;
        _game.ScoreBoard.RedAutorunScoreUpdated -= OnRedAutorunScoreUpdated;
        _game.ScoreBoard.RedClimbScoreUpdated -= OnRedClimbScoreUpdated;
        _game.ScoreBoard.StateOfPlayUpdated -= OnStateOfPlayUpdated;
      }
      else
      {
        return;
      }
    }

    private void OnBlueOwnershipSecondsUpdated(object sender, BlueOwnershipSecondsUpdatedEventArgs e)
    {
      _response.WriteAsync($"data: {{\"BlueOwnershipSeconds\": {e.BlueOwnershipSeconds}}}\r\r").Wait();
      _response.Body.Flush();
    }

    private void OnBlueSwitchOwnershipSecondsUpdated(object sender, BlueSwitchOwnershipSecondsUpdatedEventArgs e)
    {
      _response.WriteAsync($"data: {{\"BlueSwitchOwnershipSeconds\": {e.BlueSwitchOwnershipSeconds}}}\r\r").Wait();
      _response.Body.Flush();
    }

    private void OnBlueScaleOwnershipSecondsUpdated(object sender, BlueScaleOwnershipSecondsUpdatedEventArgs e)
    {
      _response.WriteAsync($"data: {{\"BlueScaleOwnershipSeconds\": {e.BlueScaleOwnershipSeconds}}}\r\r").Wait();
      _response.Body.Flush();
    }

    private void OnRedOwnershipSecondsUpdated(object sender, RedOwnershipSecondsUpdatedEventArgs e)
    {
      _response.WriteAsync($"data: {{\"RedOwnershipSeconds\": {e.RedOwnershipSeconds}}}\r\r").Wait();
      _response.Body.Flush();
    }

    private void OnRedSwitchOwnershipSecondsUpdated(object sender, RedSwitchOwnershipSecondsUpdatedEventArgs e)
    {
      _response.WriteAsync($"data: {{\"RedSwitchOwnershipSeconds\": {e.RedSwitchOwnershipSeconds}}}\r\r").Wait();
      _response.Body.Flush();
    }

    private void OnRedScaleOwnershipSecondsUpdated(object sender, RedScaleOwnershipSecondsUpdatedEventArgs e)
    {
      _response.WriteAsync($"data: {{\"RedScaleOwnershipSeconds\": {e.RedScaleOwnershipSeconds}}}\r\r").Wait();
      _response.Body.Flush();
    }

    private void OnElapsedSecondsUpdated(object sender, ElapsedSecondsUpdatedEventArgs e)
    {
      _response.WriteAsync($"data: {{\"ElapsedSeconds\": {e.ElapsedSeconds}}}\r\r").Wait();
      _response.Body.Flush();
    }

    private void OnStateOfPlayUpdated(object sender, StateOfPlayUpdatedEventArgs e)
    {
      _response.WriteAsync($"data: {{\"StateOfPlay\": \"{e.StateOfPlay.ToString()}\"}}\r\r").Wait();
      _response.Body.Flush();
    }

    private void OnRedVaultScoreUpdated(object sender, RedVaultScoreUpdatedEventArgs e)
    {
      _response.WriteAsync($"data: {{\"RedVaultScore\": {e.RedVaultScore}}}\r\r").Wait();
      _response.Body.Flush();
    }

    private void OnBlueVaultScoreUpdated(object sender, BlueVaultScoreUpdatedEventArgs e)
    {
      _response.WriteAsync($"data: {{\"BlueVaultScore\": {e.BlueVaultScore}}}\r\r").Wait();
      _response.Body.Flush();
    }

    private void OnRedParkScoreUpdated(object sender, RedParkScoreUpdatedEventArgs e)
    {
      _response.WriteAsync($"data: {{\"RedParkScore\": {e.RedParkScore}}}\r\r").Wait();
      _response.Body.Flush();
    }

    private void OnBlueParkScoreUpdated(object sender, BlueParkScoreUpdatedEventArgs e)
    {
      _response.WriteAsync($"data: {{\"BlueParkScore\": {e.BlueParkScore}}}\r\r").Wait();
      _response.Body.Flush();
    }

    private void OnRedAutorunScoreUpdated(object sender, RedAutorunScoreUpdatedEventArgs e)
    {
      _response.WriteAsync($"data: {{\"RedAutorunScore\": {e.RedAutorunScore}}}\r\r").Wait();
      _response.Body.Flush();
    }

    private void OnBlueAutorunScoreUpdated(object sender, BlueAutorunScoreUpdatedEventArgs e)
    {
      _response.WriteAsync($"data: {{\"BlueAutorunScore\": {e.BlueAutorunScore}}}\r\r").Wait();
      _response.Body.Flush();
    }

    private void OnRedClimbScoreUpdated(object sender, RedClimbScoreUpdatedEventArgs e)
    {
      _response.WriteAsync($"data: {{\"RedClimbScore\": {e.RedClimbScore}}}\r\r").Wait();
      _response.Body.Flush();
    }

    private void OnBlueClimbScoreUpdated(object sender, BlueClimbScoreUpdatedEventArgs e)
    {
      _response.WriteAsync($"data: {{\"BlueClimbScore\": {e.BlueClimbScore}}}\r\r").Wait();
      _response.Body.Flush();
    }


    [HttpPost("state/{state}")]
    public async Task<IActionResult> PostState([FromRoute]string state)
    {
      ScoreBoard scoreboard = new ScoreBoard();
      switch (state.ToUpper())
      {
        case "RESET":
        {
          _fieldSettings.FieldState = FieldStateEnum.GAME;
          await _game.Reset(_fieldSettings, scoreboard);
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
