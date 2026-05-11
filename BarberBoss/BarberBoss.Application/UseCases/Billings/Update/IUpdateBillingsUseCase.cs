using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;

namespace BarberBoss.Application.UseCases.Billings.Update;
public interface IUpdateBillingsUseCase
{
    public Task<ResponseShortBillingsJson> Execute(long id, RequestBillingsJson request);
}
