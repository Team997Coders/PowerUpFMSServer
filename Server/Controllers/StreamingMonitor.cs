using Microsoft.AspNetCore.Http;
using Server.Domain;
using System;

namespace Server.Controllers
{
  public class StreamingMonitor : IDisposable {
    private readonly ValueWrapper<bool> _complete;
    private readonly Game _game;
    private HttpResponse _response;

    public void Dispose()
    {
      _complete.Value = true;
      // Clean up event handlers
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

    public StreamingMonitor(ValueWrapper<bool> complete, Game game, HttpResponse response)
    {
      _complete = complete;
      _game = game;
      _response = response;

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
    }

    public void Clear()
    {
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
  }
}