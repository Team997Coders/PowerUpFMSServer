using Server.Assemblers;
using Server.DTO;
using Server.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Server.Controllers
{
  [Route("api/[controller]")]
  public class SwitchSettingsController : Controller
  {
    private readonly FieldSettings _fieldSettings;

    public SwitchSettingsController(FieldSettings fieldSettings)
    {
      _fieldSettings = fieldSettings;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
      List<DTO.SwitchSettings> dto_SwitchSettings = new List<DTO.SwitchSettings>();
      if (_fieldSettings.RedSwitchSettings != null)
      {
        dto_SwitchSettings.Add(SwitchSettingsAssembler.ToDTO(_fieldSettings.RedSwitchSettings));
      }
      if (_fieldSettings.BlueSwitchSettings != null)
      {
        dto_SwitchSettings.Add(SwitchSettingsAssembler.ToDTO(_fieldSettings.BlueSwitchSettings));
      }
      return Ok(dto_SwitchSettings.ToArray());
    }

    [HttpGet("{alliance}")]
    public async Task<IActionResult> Get(string alliance)
    {
      if (alliance.ToUpper() == "RED" || alliance.ToUpper() == "BLUE")
      {
        Domain.SwitchSettings domain_SwitchSettings;
        if (alliance.ToUpper() == "RED")
          domain_SwitchSettings = _fieldSettings.RedSwitchSettings;
        else
          domain_SwitchSettings = _fieldSettings.BlueSwitchSettings;
        if (domain_SwitchSettings == null)
        {
          return NotFound(alliance);
        }
        else
        {
          return Ok(SwitchSettingsAssembler.ToDTO(domain_SwitchSettings));
        }
      }
      else
      {
        return NotFound(alliance);
      }
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromForm]DTO.SwitchSettings switchSettings)
    {
      if (ModelState.IsValid)
      {
        _fieldSettings.UpdateSwitchSettings(switchSettings.Alliance, switchSettings.LHS, switchSettings.RHS, switchSettings.IP);
        return new NoContentResult();
      }
      return BadRequest(ModelState);
    }
  }
}
