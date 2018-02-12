using Server.Domain;

namespace Server.Services
{
  public class GameService : IGameService
  {
    private readonly Game _game;
    public GameService(Game game)
    {
      _game = game;
    }
    public bool IsPlaying
    {
      get
      {
        return _game.IsPlaying;
      }
    }

    public bool IsReset
    {
      get
      {
        return _game.IsReset;
      }
    }

    public bool IsOver
    {
      get
      {
        return (_game.IsEnded || _game.IsFaulted);
      }
    }
  }
}