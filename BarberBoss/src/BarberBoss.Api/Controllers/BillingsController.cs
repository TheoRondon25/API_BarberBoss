using Microsoft.AspNetCore.Mvc;
using BarberBoss.Communication.Responses;
using BarberBoss.Communication.Requests;
using BarberBoss.Application.UseCases.Billings.Register;
using BarberBoss.Application.UseCases.Billings.GetAll;
using BarberBoss.Application.UseCases.Billings.GetById;
using BarberBoss.Application.UseCases.Billings.Update;
using BarberBoss.Application.UseCases.Billings.Delete;

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

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseBillingsJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromServices] IGetBillingsByIdUseCase useCase, [FromRoute] long id)
    {
        var response = await useCase.Execute(id);
        
        return Ok(response);        
    }

    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseShortBillingsJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromServices] IUpdateBillingsUseCase useCase, [FromRoute] long id, [FromBody] RequestBillingsJson request)
    {
        var response = await useCase.Execute(id, request);

        return Ok(response);
    }

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]    
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromServices] IDeleteBillingsUseCase useCase, [FromRoute] long id)
    {
        await useCase.Execute(id);

        return NoContent();
    }
}
