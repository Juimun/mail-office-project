using MailOfficeEntities.Entities.Receipts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MailOfficeEntities.Entities.Configurations.Receipts;

public class ReceiptDetailConfiguration : IEntityTypeConfiguration<ReceiptDetail> {

    public void Configure(EntityTypeBuilder<ReceiptDetail> builder) {

        builder.ToTable("ReceiptDetails");
        builder.Property(r => r.Id)
            .ValueGeneratedOnAdd();

        builder
            .Property(p => p.Name)
            .HasMaxLength(100)
            .IsUnicode()
            .IsUnicode();

        builder
            .ToTable(t => t.HasCheckConstraint("Duration", "Duration > 0"))
            .Property(s => s.Duration)
            .IsRequired();

    } //Configure

} //ReceiptDetailConfiguration
