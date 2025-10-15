using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Domain.Entities;

namespace Order.Infra
{
    public class OrderConfiguration : IEntityTypeConfiguration<Orders>
    {
        public void Configure(EntityTypeBuilder<Orders> i)
        {
            i.HasKey(x => x.OrderId);
            i.Property(x => x.CustomerName).HasMaxLength(100).IsRequired();
            i.Property(x => x.CreatedAt).IsRequired();

            i.OwnsMany(o => o.Items, builder =>
            {
                builder.WithOwner().HasForeignKey("OrderId");
                builder.ToTable("OrderItems");

                builder.Property<int>("Id").UseIdentityColumn(); // primary key for OrderItems
                builder.HasKey("Id");

                builder.Property<Guid>("ProductId").IsRequired();
                builder.Property<int>("Quantity").IsRequired();
            });

            i.Navigation(o => o.Items)
                .HasField("_items")
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            i.ToTable("Orders");
        }
    }
}
