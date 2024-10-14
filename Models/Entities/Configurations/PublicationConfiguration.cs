using MailOffice.Models.Category;
using MailOffice.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MailOffice.Models.Entities.Configurations;

public class PublicationConfiguration : IEntityTypeConfiguration<Publication> {
    public void Configure(EntityTypeBuilder<Publication> builder) {

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

        // Начальная инициализация таблицы
        List<Publication> publication = [
            new() {Id = 1, Name = "Война и мир",           Type= PublicationType.Book,      Price = Utils.GetRandom(100, 300)},
            new() {Id = 2, Name = "Огонек",                Type= PublicationType.Journal,   Price = Utils.GetRandom(250, 500) }, 
            new() {Id = 3, Name = "Популярная Механика",   Type= PublicationType.Journal,   Price = Utils.GetRandom(750, 900) },  
            new() {Id = 4, Name = "Иностранная Литература",Type= PublicationType.Newspaper, Price = Utils.GetRandom(500, 1_000) }, 
            new() {Id = 5, Name = "Знание - сила",         Type= PublicationType.Newspaper, Price = Utils.GetRandom(1_500, 3_000) }
        ];
        builder.HasData(publication);

    } //Configure

} //PublicationConfiguration