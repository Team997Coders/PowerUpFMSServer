using System;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Domain
{
  public enum GameStateEnum { RESET = 0, FAULTED = 1, PLAYING = 2, ENDED = 3 }
  public sealed class Game
  {
    #region Singleton stuff
    private static readonly object _padlock = new Object();
    private static volatile Game _instance = null;
    private Game ()
    {
      _gameTimer = new Timer(new TimerCallback(OnGameQuantum), null, Timeout.Infinite, Timeout.Infinite);
      _driverCountdownTimer = new Timer(new TimerCallback(OnDriverCountdown), null, Timeout.Infinite, Timeout.Infinite);
      _autonomousCountdownTimer = new Timer(new TimerCallback(OnAutonomousCountdown), null, Timeout.Infinite, Timeout.Infinite);
      _autonomousCountdownSignaler = new Timer(new TimerCallback(OnAutonomousCountdownStart), null, Timeout.Infinite, Timeout.Infinite);
      _autonomousSignaler = new Timer(new TimerCallback(OnAutonomousStart), null, Timeout.Infinite, Timeout.Infinite);
      _delaySignaler = new Timer(new TimerCallback(OnDelayStart), null, Timeout.Infinite, Timeout.Infinite);
      _teleoperatedSignaler = new Timer(new TimerCallback(OnTeleoperatedStart), null, Timeout.Infinite, Timeout.Infinite);
      _endGameSignaler = new Timer(new TimerCallback(OnEndGameStart), null, Timeout.Infinite, Timeout.Infinite);
      _gameOverSignaler = new Timer(new TimerCallback(OnGameOver), null, Timeout.Infinite, Timeout.Infinite);
      GameState = GameStateEnum.ENDED;
      ZeroScore();
    }
    public static Game Instance
    {
      get
      {
        if (_instance == null)
        {
          lock (_padlock)
          {
            if (_instance == null)
            {
              _instance = new Game();
            }
          }
        }
        return _instance;
      }
    }

    #endregion

    // All duration values are stored in deciseconds (tenths of a second)
    private GameSettings _gameSettings;
    private Field _field;
    private Field Field
    {
      get { return _field; }
      set
      {
        _field = value;
        if (_field.RedSwitch != null)
        {
          _field.RedSwitch.Owned += OnRedAllianceOwnsRedSwitch;
          _field.RedSwitch.Unowned += OnRedAllianceUnownsRedSwitch;
        }
        if (_field.BlueSwitch != null)
        {
          _field.BlueSwitch.Owned += OnBlueAllianceOwnsBlueSwitch;
          _field.BlueSwitch.Unowned += OnBlueAllianceUnownsBlueSwitch;
        }
        if (_field.Scale != null)
        {
          _field.Scale.RedOwned += OnRedAllianceOwnsScale;
          _field.Scale.RedUnowned += OnRedAllianceUnownsScale;
          _field.Scale.BlueOwned += OnBlueAllianceOwnsScale;
          _field.Scale.BlueUnowned += OnBlueAllianceUnownsScale;
        }
      }
    }
    private ScoreBoard _scoreBoard;
    private int _redOwnershipDeciseconds;
    private int _blueOwnershipDeciseconds;
    private int _redSwitchOwnershipDeciseconds;
    private int _redScaleOwnershipDeciseconds;
    private int _blueSwitchOwnershipDeciseconds;
    private int _blueScaleOwnershipDeciseconds;
    private int _elapsedDeciseconds;

    private int ElapsedDeciseconds
    {
      get { return _elapsedDeciseconds; }
      set
      {
        _elapsedDeciseconds = value;
        if (_scoreBoard != null)
          _scoreBoard.ElapsedDeciseconds = _elapsedDeciseconds;
      }
    }

    private int RedOwnershipDeciseconds
    {
      get { return _redOwnershipDeciseconds; }
      set
      {
        _redOwnershipDeciseconds = value;
        if (_scoreBoard != null)
          _scoreBoard.RedOwnershipDeciseconds = _redOwnershipDeciseconds;
      }
    }

    private int BlueOwnershipDeciseconds
    {
      get { return _blueOwnershipDeciseconds; }
      set
      {
        _blueOwnershipDeciseconds = value;
        if (_scoreBoard != null)
          _scoreBoard.BlueOwnershipDeciseconds = _blueOwnershipDeciseconds;
      }
    }

    private int RedSwitchOwnershipDeciseconds
    {
      get { return _redSwitchOwnershipDeciseconds; }
      set
      {
        _redSwitchOwnershipDeciseconds = value;
        if (_scoreBoard != null)
          _scoreBoard.RedSwitchOwnershipDeciseconds = _redSwitchOwnershipDeciseconds;
      }
    }

    private int RedScaleOwnershipDeciseconds
    {
      get { return _redScaleOwnershipDeciseconds; }
      set
      {
        _redScaleOwnershipDeciseconds = value;
        if (_scoreBoard != null)
          _scoreBoard.RedScaleOwnershipDeciseconds = _redScaleOwnershipDeciseconds;
      }
    }

    private int BlueSwitchOwnershipDeciseconds
    {
      get { return _blueSwitchOwnershipDeciseconds; }
      set
      {
        _blueSwitchOwnershipDeciseconds = value;
        if (_scoreBoard != null)
          _scoreBoard.BlueSwitchOwnershipDeciseconds = _blueSwitchOwnershipDeciseconds;
      }
    }

    private int BlueScaleOwnershipDeciseconds
    {
      get { return _blueScaleOwnershipDeciseconds; }
      set
      {
        _blueScaleOwnershipDeciseconds = value;
        if (_scoreBoard != null)
          _scoreBoard.BlueScaleOwnershipDeciseconds = _blueScaleOwnershipDeciseconds;
      }
    }

    private Timer _gameTimer;
    private Timer _driverCountdownTimer;
    private Timer _autonomousCountdownTimer;
    private Timer _autonomousCountdownSignaler;
    private Timer _autonomousSignaler;
    private Timer _delaySignaler;
    private Timer _teleoperatedSignaler;
    private Timer _endGameSignaler;
    private Timer _gameOverSignaler;
    public GameStateEnum GameState { get; private set; }

    public ScoreBoard ScoreBoard
    {
      get { return _scoreBoard; }
    }
    
    public bool RedSwitchLHSPlateOwned
    {
      get
      {
        if (Field != null)
        {
          if (Field.RedSwitch != null)
          {
            return Field.RedSwitch.LHSPlate.IsOwned;
          }
        }
        return false;
      }
      set
      {
        if (Field != null)
        {
          if (Field.RedSwitch != null)
          {
            if (value)
              Field.RedSwitch.LHSPlate.OnOwned(null);
            else
              Field.RedSwitch.LHSPlate.OnUnowned(null);
          }
        }
      }
    }

    public bool RedSwitchRHSPlateOwned
    {
      get
      {
        if (Field != null)
        {
          if (Field.RedSwitch != null)
          {
            return Field.RedSwitch.RHSPlate.IsOwned;
          }
        }
        return false;
      }
      set
      {
        if (Field != null)
        {
          if (Field.RedSwitch != null)
          {
            if (value)
              Field.RedSwitch.RHSPlate.OnOwned(null);
            else
              Field.RedSwitch.RHSPlate.OnUnowned(null);
          }
        }
      }
    }

    public bool BlueSwitchLHSPlateOwned
    {
      get
      {
        if (Field != null)
        {
          if (Field.BlueSwitch != null)
          {
            return Field.BlueSwitch.LHSPlate.IsOwned;
          }
        }
        return false;
      }
      set
      {
        if (Field != null)
        {
          if (Field.BlueSwitch != null)
          {
            if (value)
              Field.BlueSwitch.LHSPlate.OnOwned(null);
            else
              Field.BlueSwitch.LHSPlate.OnUnowned(null);
          }
        }
      }
    }

    public bool BlueSwitchRHSPlateOwned
    {
      get
      {
        if (Field != null)
        {
          if (Field.BlueSwitch != null)
          {
            return Field.BlueSwitch.RHSPlate.IsOwned;
          }
        }
        return false;
      }
      set
      {
        if (Field != null)
        {
          if (Field.BlueSwitch != null)
          {
            if (value)
              Field.BlueSwitch.RHSPlate.OnOwned(null);
            else
              Field.BlueSwitch.RHSPlate.OnUnowned(null);
          }
        }
      }
    }

    public bool ScaleLHSPlateAsAllianceFieldLeftOwned
    {
      get
      {
        if (Field != null)
        {
          if (Field.Scale != null)
          {
            return Field.Scale.LHSPlateAsAllianceFieldLeft.IsOwned;
          }
        }
        return false;
      }
      set
      {
        if (Field != null)
        {
          if (Field.Scale != null)
          {
            if (value)
              Field.Scale.LHSPlateAsAllianceFieldLeft.OnOwned(null);
            else
              Field.Scale.LHSPlateAsAllianceFieldLeft.OnUnowned(null);
          }
        }
      }
    }

    public bool ScaleRHSPlateAsAllianceFieldLeftOwned
    {
      get
      {
        if (Field != null)
        {
          if (Field.Scale != null)
          {
            return Field.Scale.RHSPlateAsAllianceFieldLeft.IsOwned;
          }
        }
        return false;
      }
      set
      {
        if (Field != null)
        {
          if (Field.Scale != null)
          {
            if (value)
              Field.Scale.RHSPlateAsAllianceFieldLeft.OnOwned(null);
            else
              Field.Scale.RHSPlateAsAllianceFieldLeft.OnUnowned(null);
          }
        }
      }
    }
    
    public bool IsPlaying
    {
      get
      {
        return (GameState == GameStateEnum.PLAYING);
      }
    }

    public bool IsReset
    {
      get
      {
        return (GameState == GameStateEnum.RESET);
      }
    }

    public bool IsEnded
    {
      get
      {
        return (GameState == GameStateEnum.ENDED);
      }
    }

    public bool IsFaulted
    {
      get
      {
        return (GameState == GameStateEnum.FAULTED);
      }
    }

    private void ZeroScore()
    {
      RedOwnershipDeciseconds = 0;
      BlueOwnershipDeciseconds = 0;
      RedSwitchOwnershipDeciseconds = 0;
      RedScaleOwnershipDeciseconds = 0;
      BlueSwitchOwnershipDeciseconds = 0;
      BlueScaleOwnershipDeciseconds = 0;
      ElapsedDeciseconds = 0;
    }

    private void OnRedAllianceOwnsRedSwitch(object sender, EventArgs e)
    {
      if (_scoreBoard.StateOfPlay == StateOfPlayEnum.AUTONOMOUS)
      {
        RedSwitchOwnershipDeciseconds += 20;
        RedOwnershipDeciseconds += 20;
      }
      else
      {
        RedSwitchOwnershipDeciseconds += 10;
        RedOwnershipDeciseconds += 10;
      }
    }

    private void OnRedAllianceUnownsRedSwitch(object sender, EventArgs e)
    {
    }

    private void OnBlueAllianceOwnsBlueSwitch(object sender, EventArgs e)
    {
      if (_scoreBoard.StateOfPlay == StateOfPlayEnum.AUTONOMOUS)
      {
        BlueSwitchOwnershipDeciseconds += 20;
        BlueOwnershipDeciseconds += 20;
      }
      else
      {
        BlueSwitchOwnershipDeciseconds += 10;
        BlueOwnershipDeciseconds += 10;
      }
    }

    private void OnBlueAllianceUnownsBlueSwitch(object sender, EventArgs e)
    {
    }

    private void OnRedAllianceOwnsScale(object sender, EventArgs e)
    {
      if (_scoreBoard.StateOfPlay == StateOfPlayEnum.AUTONOMOUS)
      {
        RedScaleOwnershipDeciseconds += 20;
        RedOwnershipDeciseconds += 20;
      }
      else
      {
        RedScaleOwnershipDeciseconds += 10;
        RedOwnershipDeciseconds += 10;
      }
    }

    private void OnRedAllianceUnownsScale(object sender, EventArgs e)
    {
    }

    private void OnBlueAllianceOwnsScale(object sender, EventArgs e)
    {
      if (_scoreBoard.StateOfPlay == StateOfPlayEnum.AUTONOMOUS)
      {
        BlueScaleOwnershipDeciseconds += 20;
        BlueOwnershipDeciseconds += 20;
      }
      else
      {
        BlueScaleOwnershipDeciseconds += 10;
        BlueOwnershipDeciseconds += 10;
      }
    }

    private void OnBlueAllianceUnownsScale(object sender, EventArgs e)
    {
    }

    private void OnDriverCountdownStart()
    {
      Field.Off();
      _scoreBoard.StateOfPlay = StateOfPlayEnum.DRIVERCOUNTDOWN;
      ElapsedDeciseconds = _gameSettings.DriverCountdown * -10;
      _autonomousCountdownSignaler.Change(_gameSettings.DriverCountdown * 1000, Timeout.Infinite);
      _driverCountdownTimer.Change(0, 100);
    }

    private void OnAutonomousCountdownStart(object state)
    {
      _scoreBoard.StateOfPlay = StateOfPlayEnum.AUTONOMOUSCOUNTDOWN;
//      WaitHandle waitHandle = new AutoResetEvent(false);
      _driverCountdownTimer.Change(Timeout.Infinite, Timeout.Infinite);
//      WaitHandle.WaitAll(new[] {waitHandle});
      ElapsedDeciseconds = _gameSettings.AutonomousCountdown * -10;
      _autonomousSignaler.Change(_gameSettings.AutonomousCountdown * 1000, Timeout.Infinite);
      _autonomousCountdownTimer.Change(0, 100);
    }

    private void OnAutonomousStart(object state)
    {
      _scoreBoard.StateOfPlay = StateOfPlayEnum.AUTONOMOUS;
//      WaitHandle waitHandle = new AutoResetEvent(false);
      _autonomousCountdownTimer.Change(Timeout.Infinite, Timeout.Infinite);
//      WaitHandle.WaitAll(new[] {waitHandle});
      Field.Play();
      _delaySignaler.Change(_gameSettings.Autonomous * 1000, Timeout.Infinite);
      _gameTimer.Change(0, 100);
    }

    private void OnDelayStart(object state)
    {
      _scoreBoard.StateOfPlay = StateOfPlayEnum.DELAY;
      // Stop game timer for the delay
//      WaitHandle waitHandle = new AutoResetEvent(false);
      _gameTimer.Change(Timeout.Infinite, Timeout.Infinite);
//      WaitHandle.WaitAll(new[] {waitHandle});
      // Restart the game timer after the delay
      _teleoperatedSignaler.Change(_gameSettings.Delay * 1000, Timeout.Infinite);
    }

    private void OnTeleoperatedStart(object state)
    {
      _scoreBoard.StateOfPlay = StateOfPlayEnum.TELEOPERATED;
      _endGameSignaler.Change((_gameSettings.Teleoperated - _gameSettings.EndGame) * 1000, Timeout.Infinite);
      _gameOverSignaler.Change(_gameSettings.Teleoperated * 1000, Timeout.Infinite);
      _gameTimer.Change(0, 100);
    }

    private void OnEndGameStart(object state)
    {
      _scoreBoard.StateOfPlay = StateOfPlayEnum.ENDGAME;
    } 

    private void OnGameOver(object state)
    {
      _scoreBoard.StateOfPlay = StateOfPlayEnum.GAMEOVER;
      GameState = GameStateEnum.ENDED;
      StopGameTimers();
      Field.Off();
    } 

    public void Play(GameSettings gameSettings)
    {
      if (GameState != GameStateEnum.RESET)
        throw new InvalidOperationException("Cannot play game without resetting the field first.");
      _gameSettings = gameSettings;
      GameState = GameStateEnum.PLAYING;
      StartGameTimers();
    }

    private void StartGameTimers()
    {
      OnDriverCountdownStart();
    }

    private void StopGameTimers()
    {
      _gameTimer.Change(Timeout.Infinite, Timeout.Infinite);
      _endGameSignaler.Change(Timeout.Infinite, Timeout.Infinite);
      _teleoperatedSignaler.Change(Timeout.Infinite, Timeout.Infinite);
      _delaySignaler.Change(Timeout.Infinite, Timeout.Infinite);
      _autonomousSignaler.Change(Timeout.Infinite, Timeout.Infinite);
      _autonomousCountdownSignaler.Change(Timeout.Infinite, Timeout.Infinite);
      _gameOverSignaler.Change(Timeout.Infinite, Timeout.Infinite);
      _driverCountdownTimer.Change(Timeout.Infinite, Timeout.Infinite);
      _autonomousCountdownTimer.Change(Timeout.Infinite, Timeout.Infinite);
    }

    private void OnDriverCountdown(object state)
    {
      ElapsedDeciseconds += 1;
    }

    private void OnAutonomousCountdown(object state)
    {
      ElapsedDeciseconds += 1;
    }

    private void OnGameQuantum(object state)
    {
      ElapsedDeciseconds += 1;

      if (Field.BlueSwitch != null)
      {
        if (Field.BlueSwitch.IsOwned)
        {
          if (_scoreBoard.StateOfPlay == StateOfPlayEnum.AUTONOMOUS)
          {
            BlueSwitchOwnershipDeciseconds += 2;
            BlueOwnershipDeciseconds += 2;
          }
          else
          {
            BlueSwitchOwnershipDeciseconds += 1;
            BlueOwnershipDeciseconds += 1;
          }
        }
      }
      if (Field.RedSwitch != null)
      {
        if (Field.RedSwitch.IsOwned)
        {
          if (_scoreBoard.StateOfPlay == StateOfPlayEnum.AUTONOMOUS)
          {
            RedSwitchOwnershipDeciseconds += 2;
            RedOwnershipDeciseconds += 2;
          }
          else
          {
            RedSwitchOwnershipDeciseconds += 1;
            RedOwnershipDeciseconds += 1;              
          }
        }
      }
      if (Field.Scale != null)
      {
        if (Field.Scale.IsRedOwned)
        {
          if (_scoreBoard.StateOfPlay == StateOfPlayEnum.AUTONOMOUS)
          {
            RedScaleOwnershipDeciseconds += 2;
            RedOwnershipDeciseconds += 2;              
          }
          else
          {
            RedScaleOwnershipDeciseconds += 1;
            RedOwnershipDeciseconds += 1;              
          }
        }
        if (Field.Scale.IsBlueOwned)
        {
          if (_scoreBoard.StateOfPlay == StateOfPlayEnum.AUTONOMOUS)
          {
            BlueScaleOwnershipDeciseconds += 2;
            BlueOwnershipDeciseconds += 2;              
          } 
          else
          {
            BlueScaleOwnershipDeciseconds += 1;
            BlueOwnershipDeciseconds += 1;              
          }         
        }
      }
    }

    public async Task Fault()
    {
      if (GameState != GameStateEnum.PLAYING)
        throw new InvalidOperationException("Cannot fault the field unless playing is in process.");
      _scoreBoard.StateOfPlay = StateOfPlayEnum.FAULTED;
      GameState = GameStateEnum.FAULTED;
      StopGameTimers();
      await Field.Off();
    }


    public async Task Reset(FieldSettings fieldSettings, ScoreBoard scoreBoard)
    {
      if (GameState == GameStateEnum.PLAYING)
        throw new InvalidOperationException("Cannot reset game while playing in process.");
      _scoreBoard = scoreBoard;
      ZeroScore();
      GameState = GameStateEnum.RESET;
      Field fieldToRandomize = new Field(fieldSettings);
      await fieldToRandomize.Randomize(fieldSettings);
      _scoreBoard.AutonomousFieldString = fieldSettings.AutonomousFieldString;
      _scoreBoard.StateOfPlay = StateOfPlayEnum.RESET;
      Field = new Field(fieldSettings);
    }
  }
}