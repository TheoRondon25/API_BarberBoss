using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarberBoss.Communication.Responses;

namespace BarberBoss.Application.UseCases.Billings.GetById;
public interface IGetBillingsByIdUseCase
{
    Task<ResponseBillingsJson> Execute(long id);
}
