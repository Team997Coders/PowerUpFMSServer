namespace Server.DTO
{
  public class GameSettings
  {
    public int DriverCountdown { get; set; }
    public int AutonomousCountdown { get; set; }
    public int Autonomous { get; set; }
    public int Delay { get; set; }
    public int Teleoperated { get; set; }
    public int EndGame { get; set; }
  }
}