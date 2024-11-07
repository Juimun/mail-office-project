using MailOfficeEntities.Entities.Residence;
using MailOfficeTool.Infrastructure;

namespace MailOfficeFactory.Factories;

public static partial class Factory {

    // Создание сущности House
    public static House GetHouse(int houseId, int sectionId) => new()
    {
        Id = houseId,
        Street = GetRandomStreet(),
        HouseNumber = GetRandomHouseNumber(),
        SectionId = Utils.GetRandom(1, sectionId)
    };

    // Создание списка сущностей House
    public static List<House> GetHouses(int count, Func<int, int, House> getHouse) => Enumerable     
        .Range(1, count)
        .Select(p => getHouse(p, p))
        .ToList();

    // Генерация "случаных" номеров домов
    private const int MinHouseNumber = 1, MaxHouseNumber = 300;
    private static string GetRandomHouseNumber() =>  
        $"{Utils.GetRandom(MinHouseNumber, MaxHouseNumber)}";

    // Генерация "случаных" названий улиц
    private static string GetRandomStreet() => 
        $"{Prefixes[Utils.GetRandom(0, Prefixes.Length - 1)]}.{Streets[Utils.GetRandom(0, Streets.Length - 1)]}";

    #region Данные улиц для тестов
    private static readonly string[] Prefixes = ["ул", "пр-кт", "б-р", "пл", "пер"];  
    private static readonly string[] Streets = [  
        "Лесная", "Солнечная", "Центральная", "Тихая", "Зеленая", 
        "Березовая", "Вишневая", "Яблоневая", "Садовая", "Мостовая",
        "Речная", "Озерная", "Горная", "Долинная", "Школьная", 
        "Больничная", "Спортивная", "Театральная", "Парковая", "Промышленная", 
        "Заводская", "Строительная", "Транспортная", "Комсомольская", "Советская", 
        "Октябрьская", "Мира", "Победы", "Дружбы", "Рабочая", 
        "Красная", "Новая", "Старая", "Молодежная", "Студенческая",
        "Кинотеатральная", "Культурная", "Научная", "Техническая", "Финансовая",
        "Торговая", "Хозяйственная", "Почтовая", "Телефонная", "Свободы",
        "Любви", "Счастья", "Надежда"
    ];
    #endregion
} //FactoryHouse