using Microsoft.EntityFrameworkCore;
using BarberBoss.Domain.Repositories.Billings;
using BarberBoss.Domain.Entities;

namespace BarberBoss.Infrastructure.DataAccess.Repositories;
internal class BillingsRepository : IBillingsWriteOnlyRepository, IBillingsReadOnlyRepository //, IBillingsUpdateOnlyRepository
{
    private readonly BarberBossDbContext _dbContext;

    public BillingsRepository(BarberBossDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(Billing billing)
    {
        await _dbContext.Billings.AddAsync(billing);    
    }

    public async Task<bool> Delete(long id)
    {
        var result = await _dbContext.Billings.FirstOrDefaultAsync(billing => billing.Id == id);

        if (result is null)
        {
            return false;
        }

        _dbContext.Billings.Remove(result);

        return true;
    }

    public async Task<List<Billing>> GetAll()
    {
        return await _dbContext.Billings.AsNoTracking().ToListAsync();
    }
}
