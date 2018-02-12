using System;

namespace Server.Domain
{
  public enum StateOfPlayEnum { DRIVERCOUNTDOWN = 0, AUTONOMOUSCOUNTDOWN = 1, AUTONOMOUS = 2, DELAY = 3, TELEOPERATED = 4, ENDGAME = 5, GAMEOVER = 6, FAULTED = 7, IDLE = 8, RESET = 9 }

  public class ElapsedSecondsUpdatedEventArgs : EventArgs
  {
    public int ElapsedSeconds { get; set; }
  }

  public class AutonomousFieldStringUpdatedEventArgs : EventArgs
  {
    public string AutonomousFieldString { get; set; }
  }

  public class RedOwnershipSecondsUpdatedEventArgs : EventArgs
  {
    public int RedOwnershipSeconds { get; set; }
  }

  public class BlueOwnershipSecondsUpdatedEventArgs : EventArgs
  {
    public int BlueOwnershipSeconds { get; set; }
  }

  public class RedSwitchOwnershipSecondsUpdatedEventArgs : EventArgs
  {
    public int RedSwitchOwnershipSeconds { get; set; }
  }

  public class RedScaleOwnershipSecondsUpdatedEventArgs : EventArgs
  {
    public int RedScaleOwnershipSeconds { get; set; }
  }

  public class BlueSwitchOwnershipSecondsUpdatedEventArgs : EventArgs
  {
    public int BlueSwitchOwnershipSeconds { get; set; }
  }

  public class BlueScaleOwnershipSecondsUpdatedEventArgs : EventArgs
  {
    public int BlueScaleOwnershipSeconds { get; set; }
  }

  public class StateOfPlayUpdatedEventArgs : EventArgs
  {
    public StateOfPlayEnum StateOfPlay { get; set; }
  }

  public class ScoreBoard
  {
    private int _elapsedDeciseconds;
    private int _redOwnershipDeciseconds;
    private int _blueOwnershipDeciseconds;
    private int _redSwitchOwnershipDeciseconds;
    private int _redScaleOwnershipDeciseconds;
    private int _blueSwitchOwnershipDeciseconds;
    private int _blueScaleOwnershipDeciseconds;

    private int _elapsedSeconds;
    private int _redOwnershipSeconds;
    private int _blueOwnershipSeconds;
    private int _redSwitchOwnershipSeconds;
    private int _redScaleOwnershipSeconds;
    private int _blueSwitchOwnershipSeconds;
    private int _blueScaleOwnershipSeconds;

    private string _autonomousFieldString;

    private StateOfPlayEnum _stateOfPlay;

    public int ElapsedDeciseconds
    { 
      get
      {
        return _elapsedDeciseconds;
      }
      set
      {
        _elapsedDeciseconds = value;
        int tempSeconds = Convert.ToInt32(_elapsedDeciseconds * 0.1);
        if (tempSeconds != _elapsedSeconds)
        {
          _elapsedSeconds = tempSeconds;
          ElapsedSecondsUpdatedEventArgs e = new ElapsedSecondsUpdatedEventArgs {
            ElapsedSeconds = _elapsedSeconds
          };
          OnElapsedSecondsUpdated(e);
        }
      }
    }

    public int RedOwnershipDeciseconds
    { 
      get
      {
        return _redOwnershipDeciseconds;
      }
      set
      {
        _redOwnershipDeciseconds = value;
        int tempSeconds = Convert.ToInt32(_redOwnershipDeciseconds * 0.1);
        if (tempSeconds != _redOwnershipSeconds)
        {
          _redOwnershipSeconds = tempSeconds;
          RedOwnershipSecondsUpdatedEventArgs e = new RedOwnershipSecondsUpdatedEventArgs {
            RedOwnershipSeconds = _redOwnershipSeconds
          };
          OnRedOwnershipSecondsUpdated(e);
        }
      }
    }

    public int BlueOwnershipDeciseconds
    { 
      get
      {
        return _blueOwnershipDeciseconds;
      }
      set
      {
        _blueOwnershipDeciseconds = value;
        int tempSeconds = Convert.ToInt32(_blueOwnershipDeciseconds * 0.1);
        if (tempSeconds != _blueOwnershipSeconds)
        {
          _blueOwnershipSeconds = tempSeconds;
          BlueOwnershipSecondsUpdatedEventArgs e = new BlueOwnershipSecondsUpdatedEventArgs {
            BlueOwnershipSeconds = _blueOwnershipSeconds
          };
          OnBlueOwnershipSecondsUpdated(e);
        }
      }
    }

    public int RedSwitchOwnershipDeciseconds
    { 
      get
      {
        return _redSwitchOwnershipDeciseconds;
      }
      set
      {
        _redSwitchOwnershipDeciseconds = value;
        int tempSeconds = Convert.ToInt32(_redSwitchOwnershipDeciseconds * 0.1);
        if (tempSeconds != _redSwitchOwnershipSeconds)
        {
          _redSwitchOwnershipSeconds = tempSeconds;
          RedSwitchOwnershipSecondsUpdatedEventArgs e = new RedSwitchOwnershipSecondsUpdatedEventArgs {
            RedSwitchOwnershipSeconds = _redSwitchOwnershipSeconds
          };
          OnRedSwitchOwnershipSecondsUpdated(e);
        }
      }
    }

    public int BlueSwitchOwnershipDeciseconds
    { 
      get
      {
        return _blueSwitchOwnershipDeciseconds;
      }
      set
      {
        _blueSwitchOwnershipDeciseconds = value;
        int tempSeconds = Convert.ToInt32(_blueSwitchOwnershipDeciseconds * 0.1);
        if (tempSeconds != _blueSwitchOwnershipSeconds)
        {
          _blueSwitchOwnershipSeconds = tempSeconds;
          BlueSwitchOwnershipSecondsUpdatedEventArgs e = new BlueSwitchOwnershipSecondsUpdatedEventArgs {
            BlueSwitchOwnershipSeconds = _blueSwitchOwnershipSeconds
          };
          OnBlueSwitchOwnershipSecondsUpdated(e);
        }
      }
    }

    public int RedScaleOwnershipDeciseconds
    { 
      get
      {
        return _redScaleOwnershipDeciseconds;
      }
      set
      {
        _redScaleOwnershipDeciseconds = value;
        int tempSeconds = Convert.ToInt32(_redScaleOwnershipDeciseconds * 0.1);
        if (tempSeconds != _redScaleOwnershipSeconds)
        {
          _redScaleOwnershipSeconds = tempSeconds;
          RedScaleOwnershipSecondsUpdatedEventArgs e = new RedScaleOwnershipSecondsUpdatedEventArgs {
            RedScaleOwnershipSeconds = _redScaleOwnershipSeconds
          };
          OnRedScaleOwnershipSecondsUpdated(e);
        }
      }
    }

    public int BlueScaleOwnershipDeciseconds
    { 
      get
      {
        return _blueScaleOwnershipDeciseconds;
      }
      set
      {
        _blueScaleOwnershipDeciseconds = value;
        int tempSeconds = Convert.ToInt32(_blueScaleOwnershipDeciseconds * 0.1);
        if (tempSeconds != _blueScaleOwnershipSeconds)
        {
          _blueScaleOwnershipSeconds = tempSeconds;
          BlueScaleOwnershipSecondsUpdatedEventArgs e = new BlueScaleOwnershipSecondsUpdatedEventArgs {
            BlueScaleOwnershipSeconds = _blueScaleOwnershipSeconds
          };
          OnBlueScaleOwnershipSecondsUpdated(e);
        }
      }
    }

    public string AutonomousFieldString
    {
      get
      {
        return _autonomousFieldString;
      }
      set
      {
        _autonomousFieldString = value;
        AutonomousFieldStringUpdatedEventArgs e = new AutonomousFieldStringUpdatedEventArgs {
          AutonomousFieldString = value
        };
        OnAutonomousFieldStringUpdated(e);
      }
    }

    public StateOfPlayEnum StateOfPlay
    {
      get
      {
        return _stateOfPlay;
      }
      set
      {
        _stateOfPlay = value;
        StateOfPlayUpdatedEventArgs e = new StateOfPlayUpdatedEventArgs {
          StateOfPlay = value
        };
        OnStateOfPlayUpdated(e);
      }
    }
    public int ElapsedSeconds
    { 
      get { return _elapsedSeconds; }
    }

    public int RedOwnershipSeconds
    { 
      get { return _redOwnershipSeconds; }
    }

    public int BlueOwnershipSeconds
    { 
      get { return _blueOwnershipSeconds; }
    }

    public int RedSwitchOwnershipSeconds
    { 
      get { return _redSwitchOwnershipSeconds; }
    }

    public int BlueSwitchOwnershipSeconds
    { 
      get { return _blueSwitchOwnershipSeconds; }
    }

    public int RedScaleOwnershipSeconds
    { 
      get { return _redScaleOwnershipSeconds; }
    }

    public int BlueScaleOwnershipSeconds
    { 
      get { return _blueScaleOwnershipSeconds; }
    }

    public event EventHandler<ElapsedSecondsUpdatedEventArgs> ElapsedSecondsUpdated;
    public event EventHandler<RedOwnershipSecondsUpdatedEventArgs> RedOwnershipSecondsUpdated;
    public event EventHandler<BlueOwnershipSecondsUpdatedEventArgs> BlueOwnershipSecondsUpdated;
    public event EventHandler<RedSwitchOwnershipSecondsUpdatedEventArgs> RedSwitchOwnershipSecondsUpdated;
    public event EventHandler<BlueSwitchOwnershipSecondsUpdatedEventArgs> BlueSwitchOwnershipSecondsUpdated;
    public event EventHandler<RedScaleOwnershipSecondsUpdatedEventArgs> RedScaleOwnershipSecondsUpdated;
    public event EventHandler<BlueScaleOwnershipSecondsUpdatedEventArgs> BlueScaleOwnershipSecondsUpdated;
    public event EventHandler<AutonomousFieldStringUpdatedEventArgs> AutonomousFieldStringUpdated;
    public event EventHandler<StateOfPlayUpdatedEventArgs> StateOfPlayUpdated;

    protected virtual void OnElapsedSecondsUpdated(ElapsedSecondsUpdatedEventArgs e)
    {
      EventHandler<ElapsedSecondsUpdatedEventArgs> handler = ElapsedSecondsUpdated;
      if (handler != null)
      {
        handler(this, e);
      }
    }    

    protected virtual void OnRedOwnershipSecondsUpdated(RedOwnershipSecondsUpdatedEventArgs e)
    {
      EventHandler<RedOwnershipSecondsUpdatedEventArgs> handler = RedOwnershipSecondsUpdated;
      if (handler != null)
      {
        handler(this, e);
      }
    }    

    protected virtual void OnBlueOwnershipSecondsUpdated(BlueOwnershipSecondsUpdatedEventArgs e)
    {
      EventHandler<BlueOwnershipSecondsUpdatedEventArgs> handler = BlueOwnershipSecondsUpdated;
      if (handler != null)
      {
        handler(this, e);
      }
    }    

    protected virtual void OnRedSwitchOwnershipSecondsUpdated(RedSwitchOwnershipSecondsUpdatedEventArgs e)
    {
      EventHandler<RedSwitchOwnershipSecondsUpdatedEventArgs> handler = RedSwitchOwnershipSecondsUpdated;
      if (handler != null)
      {
        handler(this, e);
      }
    }    

    protected virtual void OnBlueSwitchOwnershipSecondsUpdated(BlueSwitchOwnershipSecondsUpdatedEventArgs e)
    {
      EventHandler<BlueSwitchOwnershipSecondsUpdatedEventArgs> handler = BlueSwitchOwnershipSecondsUpdated;
      if (handler != null)
      {
        handler(this, e);
      }
    }    

    protected virtual void OnRedScaleOwnershipSecondsUpdated(RedScaleOwnershipSecondsUpdatedEventArgs e)
    {
      EventHandler<RedScaleOwnershipSecondsUpdatedEventArgs> handler = RedScaleOwnershipSecondsUpdated;
      if (handler != null)
      {
        handler(this, e);
      }
    }    

    protected virtual void OnBlueScaleOwnershipSecondsUpdated(BlueScaleOwnershipSecondsUpdatedEventArgs e)
    {
      EventHandler<BlueScaleOwnershipSecondsUpdatedEventArgs> handler = BlueScaleOwnershipSecondsUpdated;
      if (handler != null)
      {
        handler(this, e);
      }
    }    

    protected virtual void OnAutonomousFieldStringUpdated(AutonomousFieldStringUpdatedEventArgs e)
    {
      EventHandler<AutonomousFieldStringUpdatedEventArgs> handler = AutonomousFieldStringUpdated;
      if (handler != null)
      {
        handler(this, e);
      }
    }    

    protected virtual void OnStateOfPlayUpdated(StateOfPlayUpdatedEventArgs e)
    {
      EventHandler<StateOfPlayUpdatedEventArgs> handler = StateOfPlayUpdated;
      if (handler != null)
      {
        handler(this, e);
      }
    }    
  }
}