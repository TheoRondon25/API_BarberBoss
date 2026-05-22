using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Billings;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionBase;

namespace BarberBoss.Application.UseCases.Billings.Update;
public class UpdateBillingsUseCase : IUpdateBillingsUseCase
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBillingsUpdateOnlyRepository _repository;

    public UpdateBillingsUseCase(IMapper mapper, IUnitOfWork unitOfWork, IBillingsUpdateOnlyRepository repository)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _repository = repository;
    }

    public async Task<ResponseShortBillingsJson> Execute(long id, RequestBillingsJson request)
    {
        Validate(request);

        var billing = await _repository.GetById(id);

        if(billing is null)
        {
            throw new NotFoundException(ResourceErrorMessages.BILLING_NOT_FOUND);
        }

        _mapper.Map(request, billing);

        billing.UpdatedAt = DateTime.UtcNow;

        _repository.Update(billing);

        await _unitOfWork.Commit();

        return _mapper.Map<ResponseShortBillingsJson>(billing);
    }

    public void Validate(RequestBillingsJson request)
    {
        var validator = new BillingsValidator();

        var result = validator.Validate(request);

        if(result.IsValid is false)
        {
            var errorMessage = result.Errors.Select(f => f.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessage);
        }
    }
}
