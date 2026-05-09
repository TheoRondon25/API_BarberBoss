using Microsoft.AspNetCore.Mvc;
using BarberBoss.Communication.Responses;
using BarberBoss.Communication.Requests;
using BarberBoss.Application.UseCases.Billings.Register;
using BarberBoss.Application.UseCases.Billings.GetAll;

namespace BarberBoss.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BillingsController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredBillingsJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromServices] IRegisterBillingsUseCase useCase, [FromBody] RequestBillingsJson request)
    {        
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseBillingsJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAllBillings([FromServices] IGetAllBillingsUseCase useCase, [FromQuery] RequestGetAllBillingsJson request)
    {
        var response = await useCase.Execute(request);

        if(response.Billings.Count != 0)
            return Ok(response);

        return NoContent();
    }
}
