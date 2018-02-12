using Server.Services;
using System;
using System.Threading.Tasks;

namespace Server.Domain
{
  public class Field
  {
    public Switch RedSwitch { get; private set; }
    public Switch BlueSwitch { get; private set; }
    public Scale Scale { get; private set; }

    public Field(Switch redSwitch, Switch blueSwitch, Scale scale)
    {
      RedSwitch = redSwitch;
      BlueSwitch = blueSwitch;
      Scale = scale;
    }

    public Field(FieldSettings fieldSettings)
    {
      if (fieldSettings.RedSwitchSettings != null)
        RedSwitch = new Switch(fieldSettings.RedSwitchSettings);
      if (fieldSettings.BlueSwitchSettings != null)
        BlueSwitch = new Switch(fieldSettings.BlueSwitchSettings);
      if (fieldSettings.ScaleSettings != null)
        Scale = new Scale(fieldSettings.ScaleSettings);
    }

    public async Task Safe()
    {
      if (RedSwitch != null)
        await RedSwitch.Safe();
      if (BlueSwitch != null)
        await BlueSwitch.Safe();
      if (Scale != null)
        await Scale.Safe();
    }

    public async Task StaffSafe()
    {
      if (RedSwitch != null)
        await RedSwitch.StaffSafe();      
      if (BlueSwitch != null)
        await BlueSwitch.StaffSafe();
      if (Scale != null)
        await Scale.StaffSafe();
    }

    public async Task Randomize(FieldSettings fieldSettings)
    {
      // Pass in field settings in order to wait for settings
      // to get updated by the field piece firmware
      fieldSettings.PrepareForRandomizationUpdates();
      if (RedSwitch != null)
        await RedSwitch.Randomize();
      if (BlueSwitch != null)
        await BlueSwitch.Randomize();
      if (Scale != null)
        await Scale.Randomize();
      fieldSettings.WaitForAllUpdates();
    }

    public async Task Off()
    {
      if (RedSwitch != null)
        await RedSwitch.Off();
      if (BlueSwitch != null)
        await BlueSwitch.Off();
      if (Scale != null)
        await Scale.Off();
    }

    public async Task Play()
    {
      if (RedSwitch != null)
        await RedSwitch.Play();
      if (BlueSwitch != null)
        await BlueSwitch.Play();
      if (Scale != null)
        await Scale.Play();
    }
  }
}