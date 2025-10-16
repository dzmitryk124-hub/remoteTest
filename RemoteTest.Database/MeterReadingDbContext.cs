using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RemoteTest.Database.Configurations;
using RemoteTest.Server.Entities;
using System.Globalization;
using System.Reflection;

namespace RemoteTest.Database
{
    public class MeterReadingDbContext : DbContext
    {
        private readonly IConfiguration configuration;

        public MeterReadingDbContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<MeterReading> MeterReadings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            optionsBuilder.UseSeeding((context, _) => SeedData(context));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply the MeterReading configuration
            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new MeterReadingConfiguration());

            // You can apply other entity configurations here
        }

        private static void SeedData(DbContext context)
        {

            if (context.Set<Account>().Any()) 
            {
                return;
            }
            Assembly assembly = Assembly.GetExecutingAssembly();
            const string resourceName = "RemoteTest.Database.Test_Accounts.csv"; // Adjust resource name

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException($"Embedded resource '{resourceName}' not found.");
                }
                using (StreamReader reader = new StreamReader(stream))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Accounts ON");
                    var records = csv.GetRecords<Account>().ToList();
                    context.Set<Account>().AddRange(records);
                    context.SaveChanges();
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Accounts OFF");
                }
            }
        }
    }
}