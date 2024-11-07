using MailOfficeEntities.Entities.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MailOfficeEntities.Entities.Configurations.Accounts;

public class SubscriberConfiguration : IEntityTypeConfiguration<Subscriber>
{

    public void Configure(EntityTypeBuilder<Subscriber> builder)
    {

        builder.ToTable("Subscribers");
        builder.Property(s => s.PersonId)
            .ValueGeneratedNever(); 

        builder
            .HasMany(s => s.Subscriptions)
            .WithOne(sub => sub.Subscriber)
            .HasForeignKey(sub => sub.SubscriberId);

        builder
            .HasOne(h => h.House)
            .WithOne(s => s.Subscriber)
            .HasForeignKey<Subscriber>(r => r.HouseId);

    } //Configure

} //SubscriberConfiguration