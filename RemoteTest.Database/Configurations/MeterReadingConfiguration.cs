using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RemoteTest.Server.Entities;

namespace RemoteTest.Database.Configurations
{
    public class MeterReadingConfiguration : IEntityTypeConfiguration<MeterReading>
    {
        public void Configure(EntityTypeBuilder<MeterReading> builder)
        {
            // Configure the primary key
            builder.HasKey(mr => mr.Id);

            // Configure the foreign key relationship with Account
            builder.HasOne<Account>()
                .WithMany()
                .HasForeignKey(mr => mr.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            // Add a unique constraint on the AccountId column
            builder.HasIndex(mr => mr.AccountId).IsUnique();

            // Additional configurations can be added here if needed
        }
    }
}
