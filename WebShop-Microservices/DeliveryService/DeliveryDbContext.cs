using DeliveryService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace DeliveryService
{
    public class DeliveryDbContext : DbContext
    {
        public DbSet<Delivery> Deliveries { get; set; }
        public DeliveryDbContext(DbContextOptions<DeliveryDbContext> dbContextOptions) : base(dbContextOptions)
        {
            try
            {
                var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                if (databaseCreator != null)
                {
                    //Create Database if cannot connect
                    if (!databaseCreator.CanConnect()) databaseCreator.Create();
                    // Create tables if no tables exist
                    if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
