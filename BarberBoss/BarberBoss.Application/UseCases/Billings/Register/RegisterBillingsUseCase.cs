using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;

namespace BarberBoss.Application.UseCases.Billings.Register;
public class RegisterBillingsUseCase : IRegisterBillingsUseCase
{
    // TODO: com banco de dados -> Criar interfaces para repositórios, criar entidades, criar mapeamento entre entidades e json, implementar repositórios, implementar use case
    public async Task<ResponseRegisteredBillingsJson> Execute(RequestBillingsJson request)
    {
        return new ResponseRegisteredBillingsJson
        {
            BarberName = request.BarberName,
            ClientName = request.ClientName,
            ServiceName = request.ServiceName
        };
    }
}
