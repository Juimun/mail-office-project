using MailOffice.Models.Category;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MailOffice.Models.Entities.Configurations;

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription> {

    public void Configure(EntityTypeBuilder<Subscription> builder) {

        builder.ToTable("Subscriptions");
        builder.Property(s => s.Id)
            .ValueGeneratedNever();

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

        // Начальная инициализация таблицы
        List<Subscription> subscriptions = [
            new() {Id = 1, StartDate = DateTime.Now,              Duration = SubscriptionPeriod.Day,      SubscriberId = 1, PublicationId = 1},
            new() {Id = 2, StartDate = DateTime.Now.AddDays(2),   Duration = SubscriptionPeriod.Day,      SubscriberId = 1, PublicationId = 2},
            new() {Id = 3, StartDate = DateTime.Now.AddDays(-2),  Duration = SubscriptionPeriod.Week,     SubscriberId = 2, PublicationId = 2},
            new() {Id = 4, StartDate = DateTime.Now,              Duration = SubscriptionPeriod.Month,    SubscriberId = 3, PublicationId = 3},
            new() {Id = 5, StartDate = DateTime.Now.AddDays(-45), Duration = SubscriptionPeriod.HalfYear, SubscriberId = 4, PublicationId = 4},

            new() {Id = 6, StartDate = DateTime.Now.AddDays(-20), Duration = SubscriptionPeriod.Year,     SubscriberId = 5, PublicationId = 5},
            new() {Id = 7, StartDate = DateTime.Now.AddDays(7),   Duration = SubscriptionPeriod.Month,    SubscriberId = 1, PublicationId = 5},
        ];
        builder.HasData(subscriptions);

    } //Configure

} //SubscriptionConfiguration