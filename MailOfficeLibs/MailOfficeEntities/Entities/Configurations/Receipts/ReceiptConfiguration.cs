using MailOfficeEntities.Entities.Receipts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MailOfficeEntities.Entities.Configurations.Receipts;

public class ReceiptConfiguration : IEntityTypeConfiguration<Receipt> {

    public void Configure(EntityTypeBuilder<Receipt> builder) {

        builder.ToTable("Receipts");
        builder.Property(r => r.Id)
            .ValueGeneratedOnAdd();

        builder
            .Property(r => r.Price)
            .IsRequired();

        builder
            .Property(r => r.Issuance)
            .IsRequired();

        builder
           .HasMany(r => r.ReceiptDetails)
           .WithOne(rd => rd.Receipt)  
           .HasForeignKey(rd => rd.ReceiptId);

    } //Configure

} //ReceiptConfiguration
