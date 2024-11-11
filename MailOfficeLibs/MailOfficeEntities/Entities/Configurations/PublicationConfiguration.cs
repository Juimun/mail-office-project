using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MailOfficeEntities.Entities.Configurations;

public class PublicationConfiguration : IEntityTypeConfiguration<Publication> {
    public void Configure(EntityTypeBuilder<Publication> builder) {

        builder.ToTable("Publications");
        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd();

        builder
            .Property(p => p.Name)
            .HasMaxLength(100)
            .IsUnicode()
            .IsUnicode();

        builder
            .ToTable(t => t.HasCheckConstraint("Price", "Price > 0"))
            .Property(p => p.Price)
            .IsRequired();

        builder
            .HasMany(p => p.Subscriptions)
            .WithOne(s => s.Publication)
            .HasForeignKey(s => s.PublicationId);

    } //Configure

} //PublicationConfiguration