using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PurchasingOrder.Infrastructure.Data.Configuration;

public class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
{
  public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
  {

    builder.Ignore(orders => orders.DomainEvents);
    builder.Ignore(po => po.TotalPrice);

    builder.HasKey(po => po.Id);
    builder.Property(po => po.Id).HasConversion(po => po.Value,
                                                dbID => PurchaseOrderId.Of(dbID));

    builder.HasMany(po => po.PurchaseItems)
      .WithOne()
      .HasForeignKey(po => po.PurchaseOrderId);


    builder.ComplexProperty(
                po => po.PONumber, POBuilder =>
                {
                  POBuilder.Property(p => p.Value)
                  .HasColumnName("PONumber")
                  .HasMaxLength(200)
                  .IsRequired();
                });

    builder.Property(po => po.DocumentState)
                    .HasConversion<string>()
                    .HasMaxLength(30);

    builder.Property(po => po.DocumentStatus)
                    .HasConversion<string>()
                    .HasMaxLength(30);

    builder.Property(po => po.IssuedDate);

  }
}
