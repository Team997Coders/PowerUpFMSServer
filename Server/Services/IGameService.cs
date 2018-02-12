namespace Server.Services
{
  public interface IGameService
  {
    bool IsPlaying { get; }
    bool IsReset { get; }
    bool IsOver { get; }
  }
}