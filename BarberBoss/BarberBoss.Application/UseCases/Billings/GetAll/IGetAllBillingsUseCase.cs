using BarberBoss.Communication.Responses;

namespace BarberBoss.Application.UseCases.Billings.GetAll;
public interface IGetAllBillingsUseCase
{
    Task<ResponseAllBillingsJson> Execute();
}
