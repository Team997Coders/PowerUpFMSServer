using System;
using System.Net;

namespace Server.Domain
{
  public class ScaleSettings : ValueObject<ScaleSettings>
  {
    public Alliance FieldLeftAlliance { get; private set; }
    public Alliance FieldRightAlliance { get; private set; }
    private Alliance _lhsAllianceAsFieldLeft;
    private Alliance _rhsAllianceAsFieldLeft;
    public Alliance LHSAlliance(Alliance asAlliance) {
      if (asAlliance == FieldLeftAlliance)
        return _lhsAllianceAsFieldLeft;
      else
        return _rhsAllianceAsFieldLeft;
    }
    public Alliance RHSAlliance(Alliance asAlliance) {
      if (asAlliance == FieldLeftAlliance)
        return _rhsAllianceAsFieldLeft;
      else
        return _lhsAllianceAsFieldLeft;
    }
    public IPAddress IP { get; private set; }
     
    public ScaleSettings(
      string fieldLeftAlliance,
      string lhsAllianceAsFieldLeft,
      string rhsAllianceAsFieldLeft,
      string ipAddress
    )
    {
      _lhsAllianceAsFieldLeft = Alliance.Parse(lhsAllianceAsFieldLeft);
      _rhsAllianceAsFieldLeft = Alliance.Parse(rhsAllianceAsFieldLeft);
      if (_lhsAllianceAsFieldLeft == _rhsAllianceAsFieldLeft) {
        throw new ArgumentException("Left and right alliances must be different colors");
      }
      FieldLeftAlliance = Alliance.Parse(fieldLeftAlliance);
      FieldRightAlliance = FieldLeftAlliance.OtherAlliance();
      IP = IPAddress.Parse(ipAddress);
    }
  }
}