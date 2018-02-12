using Server.Domain;
using Server.DTO;

namespace Server.Assemblers
{
  public class SwitchSettingsAssembler
  {
    public static DTO.SwitchSettings ToDTO(Domain.SwitchSettings domain_SwitchSettings)
    {
      DTO.SwitchSettings dto_SwitchSettings = new DTO.SwitchSettings
      {
        Alliance = domain_SwitchSettings.Alliance.ToString(),
        LHS = domain_SwitchSettings.LHSAlliance.ToString(),
        RHS = domain_SwitchSettings.RHSAlliance.ToString(),
        IP = domain_SwitchSettings.IP.ToString()
      };
      return dto_SwitchSettings;
    }    
  }
}