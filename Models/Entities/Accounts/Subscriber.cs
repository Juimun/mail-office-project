using MailOffice.Models.Entities.Configurations.Accounts;
using MailOffice.Models.Entities.Residence;
using Microsoft.EntityFrameworkCore;

namespace MailOffice.Models.Entities.Accounts;

// Класс для подписчика почтового отделения
[EntityTypeConfiguration(typeof(SubscriberConfiguration))]
public class Subscriber {

    public int Id { get; set; }

    // Внешняя связь с Person
    public int PersonId { get; set; }
    public virtual Person Person { get; set; }

    // Внешняя связь с House
    public int HouseId { get; set; } 
    public virtual House House { get; set; }

    // Связное свойство для таблицы Subscription, связь 1:M
    // Subscriber (один подписчик) -> Subscription (несколько подписок) 
    public virtual List<Subscription> Subscriptions { get; set; } = new();

} //Subscriber
