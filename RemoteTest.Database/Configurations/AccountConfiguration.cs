using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RemoteTest.Server.Entities;

namespace RemoteTest.Database.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            // Configure the primary key
            builder.HasKey(a => a.AccountId);

            // Configure properties
            builder.Property(a => a.FirstName)
                .IsRequired()
                .HasMaxLength(100); // Example constraint

            builder.Property(a => a.LastName)
                .IsRequired()
                .HasMaxLength(100); // Example constraint

            // Additional configurations can be added here if needed
        }
    }
}
