using MailOfficeEntities.Category;
using MailOfficeEntities.Entities;
using MailOfficeEntities.Entities.Accounts;
using MailOfficeTool.Infrastructure;

namespace MailOfficeFactory.Factories;

public static partial class Factory { 

    // Создание сущности Subscription
    public static Subscription GetSubscription(int subscriberId, int publicationId) => new()
    {  
        StartDate = GetRandomDate(),
        Duration = GetRandomDuration(),
        SubscriberId = subscriberId,
        PublicationId = publicationId
    };

    // Создание списка сущностей Subscription
    public static List<Subscription> GetSubscriptions(List<Subscriber> subscribers, Func<int, int, Subscription> getSubscription) => subscribers
        .Select((s, index) => getSubscription(s.Id, index + 1))
        .ToList();

    // Генератор случайной роли SubscriptionPeriod
    private static SubscriptionPeriod GetRandomDuration() =>
        Utils.GetRandom(1, 5) switch
        {
            1 => SubscriptionPeriod.Day,
            2 => SubscriptionPeriod.Week,
            3 => SubscriptionPeriod.Month,
            4 => SubscriptionPeriod.HalfYear,
            _ => SubscriptionPeriod.Year,
        };

    // Генератор "случайного" начала подписки
    private const int MinDay = 1, MaxDay = 10;   
    private static DateTime GetRandomDate() => 
        Utils.GetRandom(1, 3) switch
    {
        1 => DateTime.Now.AddDays(Utils.GetRandom(MinDay, MaxDay)),
        2 => DateTime.Now,
        _ => DateTime.Now.AddDays(-Utils.GetRandom(MinDay, MaxDay)),
    };

} //FactorySubscription