using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarberBoss.Application.UseCases.Billings.Delete;
public interface IDeleteBillingsUseCase
{
    public Task Execute(long id);
}
