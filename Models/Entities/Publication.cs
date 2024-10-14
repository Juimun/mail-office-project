using MailOffice.Models.Category;
using MailOffice.Models.Entities.Configurations;
using MailOffice.Models.Entities.Residence;
using Microsoft.EntityFrameworkCore;

namespace MailOffice.Models.Entities;

// Класс для подписного издания
[EntityTypeConfiguration(typeof(PublicationConfiguration))]
public class Publication {

    public int Id { get; set; }

    // Наименование
    public string Name { get; set; }

    public PublicationType Type { get; set; }

    // Цена подписки
    public decimal Price { get; set; }

    // Связное свойство для таблицы Subscription, связь 1:M
    // Publication (одно издание) -> Subscription (несколько подписок на это издание)
    public virtual List<Subscription> Subscriptions { get; set; } = new();

} //Publication