using Server.Domain;
using Server.DTO;

namespace Server.Assemblers
{
  public class GameSettingsAssembler
  {
    public static DTO.GameSettings ToDTO(Domain.GameSettings domain_GameSettings)
    {
      DTO.GameSettings dto_GameSettings = new DTO.GameSettings
      {
        DriverCountdown = domain_GameSettings.DriverCountdown,
        AutonomousCountdown = domain_GameSettings.AutonomousCountdown,
        Autonomous = domain_GameSettings.Autonomous,
        Delay = domain_GameSettings.Delay,
        Teleoperated = domain_GameSettings.Teleoperated,
        EndGame = domain_GameSettings.EndGame
      };
      return dto_GameSettings;
    }    
  }
}