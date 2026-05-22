using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Billings;
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

    public async Task<ResponseAllBillingsJson> Execute(RequestGetAllBillingsJson request)
    {
        var filters = new BillingsFilters
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            BarberName = request.BarberName,
            ClientName = request.ClientName,
            ServiceName = request.ServiceName,
            MinAmount = request.MinAmount,
            MaxAmount = request.MaxAmount,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            OrderBy = request.OrderBy,
            Descending = request.Descending
        };

        var (billings, totalCount) = await _repository.GetAll(filters);

        return new ResponseAllBillingsJson
        {
            Billings = _mapper.Map<List<ResponseShortBillingsJson>>(billings),
            CurrentPage = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
        };
    }
}
