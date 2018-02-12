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
        OnRedOwnershipSecondsUpdated(null, new RedOwnershipSecondsUpdatedEventArgs{RedOwnershipSeconds=0});
        OnRedSwitchOwnershipSecondsUpdated(null, new RedSwitchOwnershipSecondsUpdatedEventArgs{RedSwitchOwnershipSeconds=0});
        OnRedScaleOwnershipSecondsUpdated(null, new RedScaleOwnershipSecondsUpdatedEventArgs{RedScaleOwnershipSeconds=0});

        // wire up event handlers
        _game.ScoreBoard.ElapsedSecondsUpdated += OnElapsedSecondsUpdated;
        _game.ScoreBoard.BlueOwnershipSecondsUpdated += OnBlueOwnershipSecondsUpdated;
        _game.ScoreBoard.BlueSwitchOwnershipSecondsUpdated += OnBlueSwitchOwnershipSecondsUpdated;
        _game.ScoreBoard.BlueScaleOwnershipSecondsUpdated += OnBlueScaleOwnershipSecondsUpdated;
        _game.ScoreBoard.RedOwnershipSecondsUpdated += OnRedOwnershipSecondsUpdated;
        _game.ScoreBoard.RedSwitchOwnershipSecondsUpdated += OnRedSwitchOwnershipSecondsUpdated;
        _game.ScoreBoard.RedScaleOwnershipSecondsUpdated += OnRedScaleOwnershipSecondsUpdated;
        _game.ScoreBoard.StateOfPlayUpdated += OnStateOfPlayUpdated;

        // wait until the game is done
        SpinWait.SpinUntil(() => !_game.IsPlaying);

        _response.WriteAsync($"data: {{\"EndGame\": 1}}\r\r").Wait();
        _response.Body.Flush();

        _game.ScoreBoard.ElapsedSecondsUpdated -= OnElapsedSecondsUpdated;
        _game.ScoreBoard.BlueOwnershipSecondsUpdated -= OnBlueOwnershipSecondsUpdated;
        _game.ScoreBoard.BlueSwitchOwnershipSecondsUpdated -= OnBlueSwitchOwnershipSecondsUpdated;
        _game.ScoreBoard.BlueScaleOwnershipSecondsUpdated -= OnBlueScaleOwnershipSecondsUpdated;
        _game.ScoreBoard.RedOwnershipSecondsUpdated -= OnRedOwnershipSecondsUpdated;
        _game.ScoreBoard.RedSwitchOwnershipSecondsUpdated -= OnRedSwitchOwnershipSecondsUpdated;
        _game.ScoreBoard.RedScaleOwnershipSecondsUpdated -= OnRedScaleOwnershipSecondsUpdated;
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

    [HttpPost("{state}")]
    public async Task<IActionResult> Post([FromRoute]string state)
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
  }
}
