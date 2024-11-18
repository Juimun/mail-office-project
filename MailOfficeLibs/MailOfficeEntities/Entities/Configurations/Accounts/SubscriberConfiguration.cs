using MailOfficeEntities.Entities.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MailOfficeEntities.Entities.Configurations.Accounts;

public class SubscriberConfiguration : IEntityTypeConfiguration<Subscriber>
{

    public void Configure(EntityTypeBuilder<Subscriber> builder)
    {

        builder.ToTable("Subscribers");
        builder.Property(s => s.Id)
            .ValueGeneratedOnAdd();

        builder
            .HasMany(s => s.Subscriptions)
            .WithOne(sub => sub.Subscriber)
            .HasForeignKey(sub => sub.SubscriberId);

        builder
            .HasOne(h => h.House)
            .WithOne(s => s.Subscriber)
            .HasForeignKey<Subscriber>(r => r.HouseId);

        builder
            .HasOne(s => s.Person)
            .WithOne(p => p.Subscriber)
            .HasForeignKey<Subscriber>(s => s.PersonId);

    } //Configure

} //SubscriberConfiguration