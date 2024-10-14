using MailOffice.Models.Category;
using Microsoft.EntityFrameworkCore;
using MailOffice.Models.Entities.Configurations;
using MailOffice.Models.Entities.Accounts;

namespace MailOffice.Models.Entities;

// Класс для предоставления подписки
[EntityTypeConfiguration(typeof(SubscriptionConfiguration))]
public class Subscription {

    public int Id { get; set; }

    // Дата оформления подписка
    public DateTime StartDate { get; set; } 

    // Срок подписки (в днях)
    public SubscriptionPeriod Duration { get; set; } 

    #region Внешний ключ и связное свойстов для Publication
    public int PublicationId { get; set; }
    public virtual Publication Publication { get; set; }
    #endregion

    #region Внешний ключ и связное свойстов для Subscriber 
    public int SubscriberId { get; set; } 
    public virtual Subscriber Subscriber { get; set; }
    #endregion

    public DateTime EndDate => StartDate.AddDays((int)Duration);

} //Subscription