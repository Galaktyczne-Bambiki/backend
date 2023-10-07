using BambikiBackend.Api.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace BambikiBackend.Api.Database;

public class BambikiDatabaseContext : DbContext
{
    public BambikiDatabaseContext(DbContextOptions<BambikiDatabaseContext> contextOptions) : base(contextOptions)
    {

    }

    public DbSet<FireReportsEntity> FireReports { get; set; }
    public DbSet<TemperatureReportsEntity> TemperatureReports { get; set; }
}