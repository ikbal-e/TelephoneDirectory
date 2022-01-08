using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Report.API.Services;

namespace Report.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ReportsController : ControllerBase
{
    private readonly ILocationService _locationService;

    public ReportsController(ILocationService locationService)
    {
        _locationService = locationService;
    }

    [HttpPost()]
    public async Task<ActionResult> CreateReportAsync()
    {
        var reportId = await _locationService.GenerateReportAsync();
        return Created("", reportId);
    }
}
