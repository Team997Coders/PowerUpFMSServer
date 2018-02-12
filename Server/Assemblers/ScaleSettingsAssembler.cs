using Server.Domain;
using Server.DTO;

namespace Server.Assemblers
{
  public class ScaleSettingsAssembler
  {
    public static DTO.ScaleSettings ToDTO(Domain.ScaleSettings domain_ScaleSettings)
    {
      DTO.ScaleSettings dto_ScaleSettings = new DTO.ScaleSettings
      {
        FLAlliance = domain_ScaleSettings.FieldLeftAlliance.ToString(),
        LHSFL = domain_ScaleSettings.LHSAlliance(domain_ScaleSettings.FieldLeftAlliance).ToString(),
        RHSFL = domain_ScaleSettings.RHSAlliance(domain_ScaleSettings.FieldLeftAlliance).ToString(),
        IP = domain_ScaleSettings.IP.ToString()
      };
      return dto_ScaleSettings;
    }    
  }
}