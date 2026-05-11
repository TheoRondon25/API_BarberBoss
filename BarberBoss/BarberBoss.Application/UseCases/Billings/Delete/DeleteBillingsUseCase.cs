using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Billings;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionBase;

namespace BarberBoss.Application.UseCases.Billings.Delete;
public class DeleteBillingsUseCase : IDeleteBillingsUseCase
{
    private readonly IBillingsWriteOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBillingsUseCase(IBillingsWriteOnlyRepository repository, IUnitOfWork unitOfWork)        
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(long id)
    {
        var result = await _repository.Delete(id);

        if (!result)
            throw new NotFoundException(ResourceErrorMessages.BILLING_NOT_FOUND);

        await _unitOfWork.Commit();
    }
}
