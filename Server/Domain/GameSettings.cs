using System;

namespace Server.Domain
{
  public class GameSettings
  {
    #region Singleton stuff
    private static readonly object _padlock = new Object();
    private static volatile GameSettings _instance = null;
    // Default settings on newly installed driver station
    private GameSettings () : this(3, 5, 15, 1, 105, 30) { }

    public static GameSettings Instance
    {
      get
      {
        if (_instance == null)
        {
          lock (_padlock)
          {
            if (_instance == null)
            {
              _instance = new GameSettings();
            }
          }
        }
        return _instance;
      }
    }
    #endregion

    public int DriverCountdown { get; private set; }
    public int AutonomousCountdown { get; private set; }
    public int Autonomous { get; private set; }
    public int Delay { get; private set; }
    public int Teleoperated { get; private set; }
    public int EndGame { get; private set; }

    public int DriverCountdownStart { get { return 1; } } 
    public int AutonomousCountdownStart { get { return DriverCountdown; } }
    public int AutonomousStart { get { return DriverCountdown + AutonomousCountdown; } }
    public int DelayStart { get { return DriverCountdown + AutonomousCountdown + Autonomous; } }
    public int TeleoperatedStart { get { return DriverCountdown + AutonomousCountdown + Autonomous + Delay; } }
    public int EndGameStart { get { return DriverCountdown + AutonomousCountdown + Autonomous + Delay + Teleoperated - EndGame; } }
    
    public GameSettings(
      int driverCountdown,
      int autonomousCountdown,
      int autonomous,
      int delay,
      int teleoperated,
      int endGame
    )
    {
      DriverCountdown = driverCountdown;
      AutonomousCountdown = autonomousCountdown;
      Autonomous = autonomous;
      Delay = delay;
      Teleoperated = teleoperated;
      EndGame = endGame;
    }

    public void Update(
      int driverCountdown,
      int autonomousCountdown,
      int autonomous,
      int delay,
      int teleoperated,
      int endGame
    )
    {
      DriverCountdown = driverCountdown;
      AutonomousCountdown = autonomousCountdown;
      Autonomous = autonomous;
      Delay = delay;
      Teleoperated = teleoperated;
      EndGame = endGame;
    }
  }
}