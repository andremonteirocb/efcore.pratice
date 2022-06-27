using CursoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fundamentos.EFCore.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(user => user.Id);

            builder.Property(user => user.Username)
                   .HasColumnName("Username");

            // Without Custom Column Name
            // builder.OwnsOne(user => user.Address);

            builder.OwnsOne(user => user.Address,
                            navigationBuilder =>
                            {
                                navigationBuilder.Property(address => address.Country)
                                                 .HasColumnName("Country");
                                navigationBuilder.Property(address => address.City)
                                                 .HasColumnName("City");
                            });
        }
    }
}
