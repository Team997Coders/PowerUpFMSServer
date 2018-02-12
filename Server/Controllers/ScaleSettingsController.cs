using Server.Assemblers;
using Server.DTO;
using Server.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
  [Route("api/[controller]")]
  public class ScaleSettingsController : Controller
  {
    private readonly FieldSettings _fieldSettings;

    public ScaleSettingsController(FieldSettings fieldSettings)
    {
      _fieldSettings = fieldSettings;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
      List<DTO.ScaleSettings> dto_ScaleSettings = new List<DTO.ScaleSettings>();
      if (_fieldSettings.ScaleSettings != null)
      {
        dto_ScaleSettings.Add(ScaleSettingsAssembler.ToDTO(_fieldSettings.ScaleSettings));
      }
      return Ok(dto_ScaleSettings.ToArray());
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromForm]DTO.ScaleSettings scaleSettings)
    {
      if (ModelState.IsValid)
      {
        _fieldSettings.UpdateScaleSettings(scaleSettings.FLAlliance, scaleSettings.LHSFL, scaleSettings.RHSFL, scaleSettings.IP);
        return new NoContentResult();
      }
      return BadRequest(ModelState);
    }
  }
}
