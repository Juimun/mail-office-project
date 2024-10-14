using MailOffice.Models.Entities.Accounts;
using MailOffice.Models.Entities.Residence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace MailOffice.Models.Entities.Configurations.Residence;

public class SectionConfiguration : IEntityTypeConfiguration<Section>
{

    public void Configure(EntityTypeBuilder<Section> builder) {

        builder.ToTable("Sections");
        builder.HasKey(sec => sec.Id);

        // Настройка наименования участка
        builder
            .Property(s => s.Name)
            .HasMaxLength(100)
            .IsUnicode()
            .IsRequired();

        builder.HasMany(s => s.Houses)
            .WithOne(h => h.Section)
            .HasForeignKey(h => h.SectionId);

        // Начальная инициализация таблицы
        List<Section> sections = [
            new() {Id = 1, Name = "Участок №1"},
            new() {Id = 2, Name = "Участок №2"},
            new() {Id = 3, Name = "Участок №3"},
            new() {Id = 4, Name = "Участок №4"},
            new() {Id = 5, Name = "Участок №5"}
        ];
        builder.HasData(sections);
    } //Configure

} //SectionConfiguration