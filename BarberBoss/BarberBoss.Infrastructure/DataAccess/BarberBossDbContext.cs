using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarberBoss.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BarberBoss.Infrastructure.DataAccess;
public class BarberBossDbContext : DbContext
{
    public BarberBossDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Billing> Billings { get; set; }
}
