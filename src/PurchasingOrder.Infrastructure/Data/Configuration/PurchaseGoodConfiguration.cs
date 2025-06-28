using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PurchasingOrder.Infrastructure.Data.Configuration;

public class PurchaseGoodConfiguration : IEntityTypeConfiguration<PurchaseGood>
{
  public void Configure(EntityTypeBuilder<PurchaseGood> builder)
  {

    builder.HasKey(pg => pg.Id);
    builder.Property(pg => pg.Id).HasConversion(pg => pg.Value,
                                                dbID => PurchaseGoodId.Of(dbID));


    builder.ComplexProperty(
                        pg => pg.Code, pgBuilder =>
                        {
                          pgBuilder.Property(p => p.Code)
                                      .HasColumnName("Code")
                                      .HasMaxLength(100)
                                      .IsRequired();
                        });


    builder.Property(pg => pg.Name).HasMaxLength(250).IsRequired();
  }
}
