using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarberBoss.Domain.Repositories;

namespace BarberBoss.Infrastructure.DataAccess;
internal class UnitOfWork : IUnitOfWork
{
    private readonly BarberBossDbContext _dbContext;

    public UnitOfWork(BarberBossDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Commit() => await _dbContext.SaveChangesAsync();
}
