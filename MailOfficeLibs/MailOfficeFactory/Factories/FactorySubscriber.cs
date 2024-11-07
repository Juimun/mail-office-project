using MailOfficeEntities.Category;
using MailOfficeEntities.Entities.Accounts;

namespace MailOfficeFactory.Factories;

public static partial class Factory {

    // Создание сущности Subscriber
    public static Subscriber GetSubscriber(int personId, int houseId) => new()     
    { 
        PersonId = personId,
        HouseId = houseId,
    };

    // Создание списка сущностей Staff 
    public static List<Subscriber> GetSubscribers(List<Person> people, Func<int, int, Subscriber> getSubscriber) => people
            .Where(p => p.Role == PersonCategory.Subscriber)
            .Select((p, index) => getSubscriber(p.Id, index + 1))
            .ToList();

} //FactorySubscriber