using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MailOfficeEntities.Entities.Configurations;

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription> {

    public void Configure(EntityTypeBuilder<Subscription> builder) {

        builder.ToTable("Subscriptions");
        builder.Property(s => s.Id)
            .ValueGeneratedOnAdd();

        builder
            .Property(s => s.StartDate)
            .IsRequired();

        builder
            .ToTable(t => t.HasCheckConstraint("Duration", "Duration > 0"))
            .Property(s => s.Duration)
            .IsRequired();

        builder.HasOne(s => s.Publication)
            .WithMany(p => p.Subscriptions)
            .HasForeignKey(s => s.PublicationId);

        builder.HasOne(s => s.Subscriber)
            .WithMany(sub => sub.Subscriptions)
            .HasForeignKey(s => s.SubscriberId);

    } //Configure

} //SubscriptionConfiguration