using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Report.API.Models;
using Report.API.Services;

namespace Report.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ReportsController : ControllerBase
{
    private readonly ILocationService _locationService;
    private readonly IDocumentService _documentService;

    public ReportsController(ILocationService locationService, IDocumentService documentService)
    {
        _locationService = locationService;
        _documentService = documentService;
    }


    [HttpGet()]
    public async Task<ActionResult<IEnumerable<Entities.Report>>> GetReportsAsync()
    {
        var reports = await _documentService.GetLocationReportsAsync();
        return Ok(reports);
    }

    [HttpGet("{id}/Detail")]
    public async Task<ActionResult> GetReportAsync([FromRoute] string id)
    {
        var report = await _documentService.GetGeportFileAsync(id);
        return File(report, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    [HttpPost()]
    public async Task<ActionResult> CreateReportAsync()
    {
        var reportId = await _locationService.GenerateReportAsync();
        return Created("", reportId);
    }
}
