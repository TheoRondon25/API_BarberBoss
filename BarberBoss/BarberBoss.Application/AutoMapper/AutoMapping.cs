using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Entities;

namespace BarberBoss.Application.AutoMapper;
public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToEntity();
        EntityToResponse();
    }

    private void RequestToEntity()
    {
        CreateMap<RequestBillingsJson, Billing>();
    }

    private void EntityToResponse()
    {
        CreateMap<Billing, ResponseRegisteredBillingsJson>();
        CreateMap<Billing, ResponseShortBillingsJson>();
        CreateMap<Billing, ResponseBillingsJson>();
    }
}
