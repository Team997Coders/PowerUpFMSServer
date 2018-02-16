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
      if (_fieldSettings.ScaleSettings == null)
      {
        return NotFound();
      }
      else
      {
        return Ok(ScaleSettingsAssembler.ToDTO(_fieldSettings.ScaleSettings));
      }
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
