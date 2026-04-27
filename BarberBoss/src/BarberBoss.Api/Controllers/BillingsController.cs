using Microsoft.AspNetCore.Mvc;
using BarberBoss.Communication.Responses;
using BarberBoss.Communication.Requests;
using BarberBoss.Application.UseCases.Billings.Register;

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
        // TODO: criar toda a questao de injeção dependencia, banco de dados e etc
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }
}
