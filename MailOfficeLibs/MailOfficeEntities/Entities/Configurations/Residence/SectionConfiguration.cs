using MailOfficeEntities.Entities.Residence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MailOfficeEntities.Entities.Configurations.Residence;

public class SectionConfiguration : IEntityTypeConfiguration<Section>
{

    public void Configure(EntityTypeBuilder<Section> builder) {

        builder.ToTable("Sections");
        builder.Property(s => s.Id)
            .ValueGeneratedOnAdd();


        // Настройка наименования участка
        builder
            .Property(s => s.Name)
            .HasMaxLength(100)
            .IsUnicode()
            .IsRequired();

        builder.HasMany(s => s.Houses)
            .WithOne(h => h.Section)
            .HasForeignKey(h => h.SectionId);

    } //Configure

} //SectionConfiguration