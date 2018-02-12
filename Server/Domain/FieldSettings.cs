using System;
using System.Threading;

namespace Server.Domain
{
  public enum FieldStateEnum { OFF = 0, SAFE = 1, STAFFSAFE = 2, GAME = 3 }
  public sealed class FieldSettings
  {
    #region Singleton stuff
    private static readonly object _padlock = new Object();
    private static volatile FieldSettings _instance = null;
    private FieldSettings () {
      FieldState = FieldStateEnum.OFF;
    }
    public static FieldSettings Instance
    {
      get
      {
        if (_instance == null)
        {
          lock (_padlock)
          {
            if (_instance == null)
            {
              _instance = new FieldSettings();
            }
          }
        }
        return _instance;
      }
    }
    #endregion

    private SwitchSettings _redSwitchSettings;
    private SwitchSettings _blueSwitchSettings;
    private ScaleSettings _scaleSettings;
    private volatile int _updatesRemaining;
    public SwitchSettings RedSwitchSettings { get { return _redSwitchSettings; } }
    public SwitchSettings BlueSwitchSettings { get { return _blueSwitchSettings; } }
    public ScaleSettings ScaleSettings { get { return _scaleSettings; } }
    public FieldStateEnum FieldState { get; set; }
    public string AutonomousFieldString
    {
      get
      {
        string close = "_";
        string middle = "_";
        string far = "_";
        // Who is field left?
        if (ScaleSettings != null)
        {
          if (ScaleSettings.FieldLeftAlliance.IsRed)
          {
            if (RedSwitchSettings != null)
            {
              if (RedSwitchSettings.LHSAlliance.IsRed)
                close = "L";
              else
                close = "R";
            }
            if (ScaleSettings.LHSAlliance(ScaleSettings.FieldLeftAlliance).IsRed)
              middle = "L";
            else
              middle = "R";
            if (BlueSwitchSettings != null)
            {
              if (BlueSwitchSettings.LHSAlliance.IsRed)
                far = "R";
              else
                far = "L";
            }
          }
          else
          {
            if (BlueSwitchSettings != null)
            {
              if (BlueSwitchSettings.LHSAlliance.IsBlue)
                close = "L";
              else
                close = "R";
            }
            if (ScaleSettings.LHSAlliance(ScaleSettings.FieldLeftAlliance).IsBlue)
              middle = "L";
            else
              middle = "R";
            if (RedSwitchSettings != null)
            {
              if (RedSwitchSettings.LHSAlliance.IsBlue)
                far = "R";
              else
                far = "L";
            }              
          }
        }
        return $"{close}{middle}{far}";
      }
    }
    
    public void UpdateSwitchSettings(
      string forAlliance,
      string lhsAlliance,
      string rhsAlliance,
      string ipAddress
    )
    {
      Alliance alliance = Alliance.Parse(forAlliance);
      switch (alliance.Is)
      {
        case Alliance.Red:
        {
          _redSwitchSettings = new SwitchSettings(lhsAlliance, rhsAlliance, forAlliance, ipAddress);
          break;
        }
        case Alliance.Blue:
        {
          _blueSwitchSettings = new SwitchSettings(lhsAlliance, rhsAlliance, forAlliance, ipAddress);
          break;
        }
      }
      lock (_padlock)
      {
        _updatesRemaining -= 1;
      }
    }

    public void UpdateScaleSettings(
      string fieldLeftAlliance,
      string lhsAllianceAsFieldLeft,
      string rhsAllianceAsFieldLeft,
      string ipAddress
    )
    {
      _scaleSettings = new ScaleSettings(
        fieldLeftAlliance, 
        lhsAllianceAsFieldLeft, 
        rhsAllianceAsFieldLeft, 
        ipAddress);
      lock (_padlock)
      {
        _updatesRemaining -= 1;
      }
    }

    // Call this method to set up proper awaiting for receiving
    // all updates from the field pieces.
    public void PrepareForRandomizationUpdates()
    {
      _updatesRemaining = 0;
      if (RedSwitchSettings != null)
        _updatesRemaining += 1;
      if (BlueSwitchSettings != null)
        _updatesRemaining += 1;
      if (ScaleSettings != null)
        _updatesRemaining += 1;
    }

    public void WaitForAllUpdates()
    {
      SpinWait.SpinUntil(() => _updatesRemaining <= 0);
    }
  }
}