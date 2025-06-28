using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PurchasingOrder.Infrastructure.Data.Configuration;

public class PurchaseItemConfiguration : IEntityTypeConfiguration<PurchaseItem>
{
  public void Configure(EntityTypeBuilder<PurchaseItem> builder)
  {
    builder.HasKey(po => po.Id);
    builder.Property(po => po.Id).HasConversion(po => po.Value,
                                                dbID => PurchaseItemId.Of(dbID));

    builder.HasOne<PurchaseGood>()
            .WithMany()
            .HasForeignKey(pi => pi.PurchaseGoodId);


    builder.Property(po => po.PurchaseItemSerialNumber).IsRequired();
    builder.Property(po => po.PurchaseItemSerialNumber).HasConversion(po => po.SerialNumber,
                                            dbID => PurchaseItemSerialNumber.Of(dbID));


    builder.Property(po => po.Price).IsRequired();
    builder.Property(po => po.Price).HasConversion(po => po.Amount,
                                            dbID => Money.Of(dbID));

  }
}
