using MailOffice.Models.Entities.Residence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MailOffice.Models.Entities.Configurations.Residence;

public class HouseConfiguration() : IEntityTypeConfiguration<House>
{

    public void Configure(EntityTypeBuilder<House> builder) {
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

        // Начальная инициализация таблицы
        List<House> houses = [
            new () {Id = 1, Street = "ул.Артема", HouseNumber = "140", SectionId = 3},
            new () {Id = 2, Street = "ул.Старая", HouseNumber = "87", SectionId = 2},
            new () {Id = 3, Street = "пр.Мира", HouseNumber = "14", SectionId = 1},
            new () {Id = 4, Street = "ул.Советская", HouseNumber = "44", SectionId = 2},
            new () {Id = 5, Street = "ул.Ленина", HouseNumber = "25", SectionId = 3},
                       
            new () {Id = 6, Street = "ул.Депутатская", HouseNumber = "157", SectionId = 1},
            new () {Id = 7, Street = "ул.Солнечная", HouseNumber = "15", SectionId = 4},
            new () {Id = 8, Street = "пр.Дубовый", HouseNumber = "33", SectionId = 4},
            new () {Id = 9, Street = "пр.Березовый", HouseNumber = "7", SectionId = 5},
            new () {Id = 10, Street = "ул.Яблоневая", HouseNumber = "101", SectionId = 5},
                       
            new () {Id = 11, Street = "пр.Сирени", HouseNumber = "24", SectionId = 4},
            new () {Id = 12, Street = "ул.Липовая", HouseNumber = "2", SectionId = 4},
        ];
        builder.HasData(houses);

    } //Configure

} //HouseConfiguration