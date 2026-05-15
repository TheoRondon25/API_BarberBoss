using BarberBoss.Domain.Billings;
using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories.Billings;
public interface IBillingsReadOnlyRepository
{
    Task<(List<Billing> billings, int totalCount)> GetAll(BillingsFilters filters);
    Task<Billing?> GetById(long id);
    Task<List<Billing>> FilterByWeek(DateOnly date);
}
