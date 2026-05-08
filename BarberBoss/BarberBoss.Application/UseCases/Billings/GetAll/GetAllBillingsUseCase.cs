using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Repositories.Billings;

namespace BarberBoss.Application.UseCases.Billings.GetAll;
public class GetAllBillingsUseCase : IGetAllBillingsUseCase
{
    private readonly IBillingsReadOnlyRepository _repository;
    private readonly IMapper _mapper;

    public GetAllBillingsUseCase(IBillingsReadOnlyRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResponseAllBillingsJson> Execute()
    {
        var result = await _repository.GetAll();

        return new ResponseAllBillingsJson
        {
            Billings = _mapper.Map<List<ResponseShortBillingsJson>>(result)
        };
    }
}
