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

  public class BlueVaultScoreUpdatedEventArgs : EventArgs
  {
    public int BlueVaultScore { get; set; }
  }

  public class RedVaultScoreUpdatedEventArgs : EventArgs
  {
    public int RedVaultScore { get; set; }
  }

  public class BlueParkScoreUpdatedEventArgs : EventArgs
  {
    public int BlueParkScore { get; set; }
  }

  public class RedParkScoreUpdatedEventArgs : EventArgs
  {
    public int RedParkScore { get; set; }
  }

  public class BlueAutorunScoreUpdatedEventArgs : EventArgs
  {
    public int BlueAutorunScore { get; set; }
  }

  public class RedAutorunScoreUpdatedEventArgs : EventArgs
  {
    public int RedAutorunScore { get; set; }
  }

  public class BlueClimbScoreUpdatedEventArgs : EventArgs
  {
    public int BlueClimbScore { get; set; }
  }

  public class RedClimbScoreUpdatedEventArgs : EventArgs
  {
    public int RedClimbScore { get; set; }
  }

  public class ScoreBoard
  {
    private int _elapsedDeciseconds;
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
    private int _blueVaultCount;
    private int _blueParkCount;
    private int _blueAutorunCount;
    private int _blueClimbCount;
    private int _redVaultCount;
    private int _redParkCount;
    private int _redAutorunCount;
    private int _redClimbCount;

    private string _autonomousFieldString;

    private StateOfPlayEnum _stateOfPlay;

    public int BlueVaultCount
    {
      get
      {
        return _blueVaultCount;
      }
      set
      {
        _blueVaultCount = value;
        BlueVaultScoreUpdatedEventArgs e = new BlueVaultScoreUpdatedEventArgs {
          BlueVaultScore = this.BlueVaultScore
        };
        OnBlueVaultScoreUpdated(e);
        SumBlueOwnershipSeconds();
      }
    }

    public int RedVaultCount
    {
      get
      {
        return _redVaultCount;
      }
      set
      {
        _redVaultCount = value;
        RedVaultScoreUpdatedEventArgs e = new RedVaultScoreUpdatedEventArgs {
          RedVaultScore = this.RedVaultScore
        };
        OnRedVaultScoreUpdated(e);
        SumRedOwnershipSeconds();
      }
    }

    public int BlueParkCount
    {
      get
      {
        return _blueParkCount;
      }
      set
      {
        _blueParkCount = value;
        BlueParkScoreUpdatedEventArgs e = new BlueParkScoreUpdatedEventArgs {
          BlueParkScore = this.BlueParkScore
        };
        OnBlueParkScoreUpdated(e);
        SumBlueOwnershipSeconds();
      }
    }

    public int RedParkCount
    {
      get
      {
        return _redParkCount;
      }
      set
      {
        _redParkCount = value;
        RedParkScoreUpdatedEventArgs e = new RedParkScoreUpdatedEventArgs {
          RedParkScore = this.RedParkScore
        };
        OnRedParkScoreUpdated(e);
        SumRedOwnershipSeconds();
      }
    }

    public int BlueAutorunCount
    {
      get
      {
        return _blueAutorunCount;
      }
      set
      {
        _blueAutorunCount = value;
        BlueAutorunScoreUpdatedEventArgs e = new BlueAutorunScoreUpdatedEventArgs {
          BlueAutorunScore = this.BlueAutorunScore
        };
        OnBlueAutorunScoreUpdated(e);
        SumBlueOwnershipSeconds();
      }
    }

    public int RedAutorunCount
    {
      get
      {
        return _redAutorunCount;
      }
      set
      {
        _redAutorunCount = value;
        RedAutorunScoreUpdatedEventArgs e = new RedAutorunScoreUpdatedEventArgs {
          RedAutorunScore = this.RedAutorunScore
        };
        OnRedAutorunScoreUpdated(e);
        SumRedOwnershipSeconds();
      }
    }

    public int BlueClimbCount
    {
      get
      {
        return _blueClimbCount;
      }
      set
      {
        _blueClimbCount = value;
        BlueClimbScoreUpdatedEventArgs e = new BlueClimbScoreUpdatedEventArgs {
          BlueClimbScore = this.BlueClimbScore
        };
        OnBlueClimbScoreUpdated(e);
        SumBlueOwnershipSeconds();
      }
    }

    public int RedClimbCount
    {
      get
      {
        return _redClimbCount;
      }
      set
      {
        _redClimbCount = value;
        RedClimbScoreUpdatedEventArgs e = new RedClimbScoreUpdatedEventArgs {
          RedClimbScore = this.RedClimbScore
        };
        OnRedClimbScoreUpdated(e);
        SumRedOwnershipSeconds();
      }
    }

    public void Clear()
    {
      ElapsedDeciseconds = 0;
      BlueAutorunCount = 0;
      BlueVaultCount = 0;
      BlueParkCount = 0;
      BlueClimbCount = 0;
      BlueSwitchOwnershipDeciseconds = 0;
      BlueScaleOwnershipDeciseconds = 0;
      RedAutorunCount = 0;
      RedVaultCount = 0;
      RedParkCount = 0;
      RedClimbCount = 0;
      RedSwitchOwnershipDeciseconds = 0;
      RedScaleOwnershipDeciseconds = 0;
    }

    private int CalculateVaultScore(int count)
    {
      return count * 5;
    }

    private int CalculateParkScore(int count)
    {
      return count * 5;
    }

    private int CalculateClimbScore(int count)
    {
      return count * 30;
    }

    private int CalculateAutorunScore(int count)
    {
      return count * 5;
    }

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

    private void SumBlueOwnershipSeconds()
    {
      int tempSeconds = 
        Convert.ToInt32(_blueSwitchOwnershipDeciseconds * 0.1) 
        + Convert.ToInt32(_blueScaleOwnershipDeciseconds * 0.1)
        + BlueVaultScore
        + BlueAutorunScore
        + BlueParkScore
        + BlueClimbScore;
      if (tempSeconds != _blueOwnershipSeconds)
      {
        _blueOwnershipSeconds = tempSeconds;
        BlueOwnershipSecondsUpdatedEventArgs e = new BlueOwnershipSecondsUpdatedEventArgs {
          BlueOwnershipSeconds = _blueOwnershipSeconds
        };
        OnBlueOwnershipSecondsUpdated(e);
      }
    }

    private void SumRedOwnershipSeconds()
    {
      int tempSeconds = 
        Convert.ToInt32(_redSwitchOwnershipDeciseconds * 0.1) 
        + Convert.ToInt32(_redScaleOwnershipDeciseconds * 0.1)
        + RedVaultScore
        + RedAutorunScore
        + RedParkScore
        + RedClimbScore;
      if (tempSeconds != _redOwnershipSeconds)
      {
        _redOwnershipSeconds = tempSeconds;
        RedOwnershipSecondsUpdatedEventArgs e = new RedOwnershipSecondsUpdatedEventArgs {
          RedOwnershipSeconds = _redOwnershipSeconds
        };
        OnRedOwnershipSecondsUpdated(e);
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
          SumRedOwnershipSeconds();
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
          SumBlueOwnershipSeconds();
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
          SumRedOwnershipSeconds();
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
          SumBlueOwnershipSeconds();
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

    public int BlueVaultScore
    {
      get { return CalculateVaultScore(_blueVaultCount); }
    }

    public int RedVaultScore
    {
      get { return CalculateVaultScore(_redVaultCount); }
    }

    public int BlueParkScore
    {
      get { return CalculateParkScore(_blueParkCount); }
    }

    public int RedParkScore
    {
      get { return CalculateParkScore(_redParkCount); }
    }

    public int BlueAutorunScore
    {
      get { return CalculateAutorunScore(_blueAutorunCount); }
    }

    public int RedAutorunScore
    {
      get { return CalculateAutorunScore(_redAutorunCount); }
    }

    public int BlueClimbScore
    {
      get { return CalculateClimbScore(_blueClimbCount); }
    }

    public int RedClimbScore
    {
      get { return CalculateClimbScore(_redClimbCount); }
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
    public event EventHandler<BlueVaultScoreUpdatedEventArgs> BlueVaultScoreUpdated;
    public event EventHandler<RedVaultScoreUpdatedEventArgs> RedVaultScoreUpdated;
    public event EventHandler<BlueParkScoreUpdatedEventArgs> BlueParkScoreUpdated;
    public event EventHandler<RedParkScoreUpdatedEventArgs> RedParkScoreUpdated;
    public event EventHandler<BlueAutorunScoreUpdatedEventArgs> BlueAutorunScoreUpdated;
    public event EventHandler<RedAutorunScoreUpdatedEventArgs> RedAutorunScoreUpdated;
    public event EventHandler<BlueClimbScoreUpdatedEventArgs> BlueClimbScoreUpdated;
    public event EventHandler<RedClimbScoreUpdatedEventArgs> RedClimbScoreUpdated;

    protected virtual void OnBlueVaultScoreUpdated(BlueVaultScoreUpdatedEventArgs e)
    {
      EventHandler<BlueVaultScoreUpdatedEventArgs> handler = BlueVaultScoreUpdated;
      if (handler != null)
      {
        handler(this, e);
      }
    }

    protected virtual void OnRedVaultScoreUpdated(RedVaultScoreUpdatedEventArgs e)
    {
      EventHandler<RedVaultScoreUpdatedEventArgs> handler = RedVaultScoreUpdated;
      if (handler != null)
      {
        handler(this, e);
      }
    }

    protected virtual void OnBlueParkScoreUpdated(BlueParkScoreUpdatedEventArgs e)
    {
      EventHandler<BlueParkScoreUpdatedEventArgs> handler = BlueParkScoreUpdated;
      if (handler != null)
      {
        handler(this, e);
      }
    }

    protected virtual void OnRedParkScoreUpdated(RedParkScoreUpdatedEventArgs e)
    {
      EventHandler<RedParkScoreUpdatedEventArgs> handler = RedParkScoreUpdated;
      if (handler != null)
      {
        handler(this, e);
      }
    }

    protected virtual void OnBlueAutorunScoreUpdated(BlueAutorunScoreUpdatedEventArgs e)
    {
      EventHandler<BlueAutorunScoreUpdatedEventArgs> handler = BlueAutorunScoreUpdated;
      if (handler != null)
      {
        handler(this, e);
      }
    }

    protected virtual void OnRedAutorunScoreUpdated(RedAutorunScoreUpdatedEventArgs e)
    {
      EventHandler<RedAutorunScoreUpdatedEventArgs> handler = RedAutorunScoreUpdated;
      if (handler != null)
      {
        handler(this, e);
      }
    }

    protected virtual void OnBlueClimbScoreUpdated(BlueClimbScoreUpdatedEventArgs e)
    {
      EventHandler<BlueClimbScoreUpdatedEventArgs> handler = BlueClimbScoreUpdated;
      if (handler != null)
      {
        handler(this, e);
      }
    }

    protected virtual void OnRedClimbScoreUpdated(RedClimbScoreUpdatedEventArgs e)
    {
      EventHandler<RedClimbScoreUpdatedEventArgs> handler = RedClimbScoreUpdated;
      if (handler != null)
      {
        handler(this, e);
      }
    }

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