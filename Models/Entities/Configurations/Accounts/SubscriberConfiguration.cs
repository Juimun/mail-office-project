using MailOffice.Models.Entities.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MailOffice.Models.Entities.Configurations.Accounts;

public class SubscriberConfiguration : IEntityTypeConfiguration<Subscriber>
{

    public void Configure(EntityTypeBuilder<Subscriber> builder)
    {

        builder.ToTable("Subscribers");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.PersonId);

        builder
            .HasMany(s => s.Subscriptions)
            .WithOne(sub => sub.Subscriber)
            .HasForeignKey(sub => sub.SubscriberId);

        // Начальное заполнение таблицы БД
        List<Subscriber> subscribers = [
            new() { Id = 1, PersonId = 1},
            new() { Id = 2, PersonId = 2},
            new() { Id = 3, PersonId = 3},
            new() { Id = 4, PersonId = 6},
            new() { Id = 5, PersonId = 10},

            new() { Id = 6, PersonId = 11},
            new() { Id = 7, PersonId = 12},
            new() { Id = 8, PersonId = 13},
            new() { Id = 9, PersonId = 14},
            new() { Id = 10, PersonId = 15},
        ];
        builder.HasData(subscribers);

    } //Configure

} //SubscriberConfiguration