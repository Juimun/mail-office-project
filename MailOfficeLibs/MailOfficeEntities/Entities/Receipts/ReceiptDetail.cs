using MailOfficeEntities.Category;

namespace MailOfficeEntities.Entities.Receipts;

// Вспомогательный класс для Receipt 
public class ReceiptDetail {

    public int Id { get; set; }

    // Наименование публикации
    public string Name { get; set; }

    // Период подписки
    public SubscriptionPeriod Duration { get; set; }

    // Внешний ключ и связное свойстов для Receipt
    public int ReceiptId { get; set; } 
    public virtual Receipt Receipt { get; set; }
} //ReceiptDetail
