namespace MailOfficeDataBase.Reports;

public record SubscriberStatement(
    // количестве газет 
    int NewspapersCount,

    // количестве журналов
    int JournalsCount,

    // количестве подписчиков
    int SubscribersCount 
    );

