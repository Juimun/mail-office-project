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

        builder
            .HasOne(h => h.House)
            .WithOne(s => s.Subscriber)
            .HasForeignKey<Subscriber>(r => r.HouseId);

        // Начальное заполнение таблицы БД
        List<Subscriber> subscribers = [
            new() { Id = 1,  PersonId = 11, HouseId = 1},
            new() { Id = 2,  PersonId = 12, HouseId = 2},
            new() { Id = 3,  PersonId = 13, HouseId = 3},
            new() { Id = 4,  PersonId = 14, HouseId = 4},
            new() { Id = 5,  PersonId = 15, HouseId = 5},
                             
            new() { Id = 6,  PersonId = 1,  HouseId = 6},
            new() { Id = 7,  PersonId = 2,  HouseId = 7},
            new() { Id = 8,  PersonId = 3,  HouseId = 8},
            new() { Id = 9,  PersonId = 4,  HouseId = 9},
            new() { Id = 10, PersonId = 5,  HouseId = 10},

            new() { Id = 11, PersonId = 18, HouseId = 11},
            new() { Id = 12, PersonId = 19, HouseId = 12},
        ];
        builder.HasData(subscribers);

    } //Configure

} //SubscriberConfiguration