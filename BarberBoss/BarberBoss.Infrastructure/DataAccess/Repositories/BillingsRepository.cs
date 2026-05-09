using Microsoft.EntityFrameworkCore;
using BarberBoss.Domain.Repositories.Billings;
using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Billings;

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

    public async Task<(List<Billing> billings, int totalCount)> GetAll(BillingsFilters filters)
    {
        var query = _dbContext.Billings.AsNoTracking().AsQueryable();

        // filtros
        if (!string.IsNullOrWhiteSpace(filters.BarberName))
            query = query.Where(b => b.BarberName.Contains(filters.BarberName));

        if (!string.IsNullOrWhiteSpace(filters.ClientName))
            query = query.Where(b => b.ClientName.Contains(filters.ClientName));

        if (!string.IsNullOrWhiteSpace(filters.ServiceName))
            query = query.Where(b => b.ServiceName.Contains(filters.ServiceName));

        if (filters.MinAmount.HasValue)
            query = query.Where(b => b.Amount >= filters.MinAmount.Value);

        if (filters.MaxAmount.HasValue)
            query = query.Where(b => b.Amount <= filters.MaxAmount.Value);

        if (filters.StartDate.HasValue)
            query = query.Where(b => b.Date >= filters.StartDate.Value);

        if (filters.EndDate.HasValue)
            query = query.Where(b => b.Date <= filters.EndDate.Value);

        // total antes da paginaçao
        var totalCount = await query.CountAsync();

        // ordenaçao
        query = filters.OrderBy.ToLower() switch
        {
            "barbername" => filters.Descending ? query.OrderByDescending(b => b.BarberName) : query.OrderBy(b => b.BarberName),
            "clientname" => filters.Descending ? query.OrderByDescending(b => b.ClientName) : query.OrderBy(b => b.ClientName),
            "servicename" => filters.Descending ? query.OrderByDescending(b => b.ServiceName) : query.OrderBy(b => b.ServiceName),
            "amount" => filters.Descending ? query.OrderByDescending(b => b.Amount) : query.OrderBy(b => b.Amount),
            _ => filters.Descending ? query.OrderByDescending(b => b.Date) : query.OrderBy(b => b.Date)
        };

        // paginaçao
        var billings = await query
            .Skip((filters.PageNumber - 1) * filters.PageSize)
            .Take(filters.PageSize)
            .ToListAsync();

        return (billings, totalCount);

        //return await _dbContext.Billings.AsNoTracking().ToListAsync();
    }
}
