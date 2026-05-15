using System.Net.Mime;
using BarberBoss.Application.UseCases.Billings.Reports.Excel;
using BarberBoss.Application.UseCases.Billings.Reports.Pdf;
using Microsoft.AspNetCore.Mvc;

namespace BarberBoss.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ReportController : ControllerBase
{
    [HttpGet("excel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetExcel([FromServices] IGenerateBillingsReportExcelUseCase useCase, [FromHeader] DateOnly month)
    {
        byte[] file = await useCase.Execute(month);

        if (file.Length > 0)
            return File(file, MediaTypeNames.Application.Octet, "barber_boss.xlsx");

        return NoContent();
    }

    [HttpGet("pdf")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetPdf([FromServices] IGenerateBillingsReportPdfUseCase useCase, [FromHeader] DateOnly date)
    {
        byte[] file = await useCase.Execute(date);

        if (file.Length > 0)
            return File(file, MediaTypeNames.Application.Pdf, "barber_boss.pdf");

        return NoContent();
    }
}
