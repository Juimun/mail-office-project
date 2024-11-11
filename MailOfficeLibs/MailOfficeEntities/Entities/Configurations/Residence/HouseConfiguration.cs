using MailOfficeEntities.Entities.Residence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MailOfficeEntities.Entities.Configurations.Residence;

public class HouseConfiguration() : IEntityTypeConfiguration<House>
{

    public void Configure(EntityTypeBuilder<House> builder) {

        builder.ToTable("Houses");
        builder.Property(h => h.Id)
            .ValueGeneratedOnAdd();

        // Настройка адреса дома
        builder
            .Property(h => h.Street)
            .HasMaxLength(150)
            .IsUnicode()
            .IsRequired();

        // Настройка адреса дома
        builder
            .Property(h => h.HouseNumber)
            .HasMaxLength(5)
            .IsUnicode()
            .IsRequired();

        // Отношение 1 : многие
        // Section -> House
        builder
            .HasOne(h => h.Section)
            .WithMany(s => s.Houses)
            .HasForeignKey(r => r.SectionId);

    } //Configure

} //HouseConfiguration