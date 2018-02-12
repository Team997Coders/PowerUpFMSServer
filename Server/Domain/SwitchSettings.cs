using System.Net;

namespace Server.Domain
{
  public class SwitchSettings : ValueObject<SwitchSettings>
  {
    public Alliance LHSAlliance { get; private set; }
    public Alliance RHSAlliance { get; private set; }
    public Alliance Alliance { get; private set; }
    public IPAddress IP { get; private set; }
     
    public SwitchSettings(
      string lhsAlliance,
      string rhsAlliance,
      string alliance,
      string ipAddress
    )
    {
      LHSAlliance = Alliance.Parse(lhsAlliance);
      RHSAlliance = Alliance.Parse(rhsAlliance);
      Alliance = Alliance.Parse(alliance);
      IP = IPAddress.Parse(ipAddress);
    }
  }
}