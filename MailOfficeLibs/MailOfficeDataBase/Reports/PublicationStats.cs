namespace MailOfficeDataBase.Reports;

// По каждому изданию указывается средний срок подписки и количество экземпляров
public record PublicationStats(
    int SubscriptionAvg,
    int Quantity
    );
    

