using MailOfficeDataBase.Reports;
using MailOfficeEntities.Category;
using MailOfficeEntities.Entities.Accounts;
using MailOfficeFactory.Factories;
using MailOfficeTool.Entities;
using Microsoft.EntityFrameworkCore;

namespace MailOfficeDataBase.DataBase;

// Класс для запросов к БД
public partial class DatabaseQueries {

    //Определить наименование и количество экземпляров всех изданий
    public List<ResultQuery01> Query01() => db
        .Publications
        .GroupBy(p => p.Name)
        .Select(g => new ResultQuery01(
            g.Key, 
            g.Count()))
        .ToList();

    //По заданному адресу определить фамилию почтальона, обслуживающего подписчика
    public List<string> Query02(string street, string houseNumber) => db
        .Houses
        .Include(h => h.Section)
            .ThenInclude(s => s.Staff) 
            .ThenInclude(s => s.Person)
        .Where(h => h.Street == street && h.HouseNumber == houseNumber)
        .Select(g => g.Section.Staff.Person.SecondName)
        .ToList();

    //Какие газеты выписывает гражданин с указанной фамилией, именем, отчеством
    public List<string> Query03(string secondName, string firstName, string patronymic) => db
        .Subscriptions 
        .Include(s => s.Publication) 
        .Include(s => s.Subscriber)
            .ThenInclude(s => s.Person) 
        .Where(s => s.Subscriber.Person.SecondName == secondName
                    && s.Subscriber.Person.FirstName == firstName
                    && (s.Subscriber.Person.Patronymic == patronymic || 
                        string.IsNullOrWhiteSpace(s.Subscriber.Person.Patronymic))
                    && s.Publication.Type == PublicationType.Newspaper)
        .Select(s => s.Publication.Name) 
        .Distinct() 
        .ToList();

    //Сколько почтальонов работает в почтовом отделении
    public int Query04() => db
        .Staff
        .Count(s => s.Role == StaffRole.Postman);

    //На каком участке количество экземпляров подписных изданий максимально
    public ResultQuery05? Query05() => db
        .Subscriptions
        .Include(s => s.Subscriber)
            .ThenInclude(h => h.House)
            .ThenInclude(s => s.Section)
        .GroupBy(p => p.Subscriber.House.Section.Name)
        .OrderByDescending(g => g.Count())
        .Select(g => new ResultQuery05(
            g.Key,
            g.Count()
            ))
        .FirstOrDefault();
    
    //Каков средний срок подписки по каждому изданию
    public List<ResultQuery06> Query06() => db
        .Subscriptions 
        .GroupBy(s => s.PublicationId)
        .Select(g => new ResultQuery06(
            g.Key, 
            g.Average(s => (int)s.Duration
        )))
        .ToList();

    //Запрос к БД Users
    // Создание списка для авторизации
    // (Для использования класса User в JSON)
    public UserJson Query07(string login, string password) => db
        .Users
        .AsEnumerable()
        .Where(u => u.Authenticate(login, password))
        .Select(u => new UserJson(u.Login, u.Password))
        .First();

    // Регистрация нового пользователя 
    public async Task AddRegisteredUserAsync(string newLogin, byte[] newPassword) {  
        await db.AddAsync(new User() { Login = newLogin, Password = newPassword });

        // Создаем таблицу People
        await db.AddAsync(new Person());
        
        await db.SaveChangesAsync();
    } //AddNewUser

    //Создание сужностей для тестов
    public void AddTestEntities() {          
        //Всего тестовых пользователей 
        int users = 1_000, quantityPublication = 450, quantitySection = 100;  

        // Создаем таблицу Users
        db.AddRange(Factory.GetUsers(users, Factory.GetUser));

        // Создаем таблицу Publication
        db.AddRange(Factory.GetPublications(quantityPublication, Factory.GetPublication));

        // Создаем таблицу Section
        db.AddRange(Factory.GetSections(quantitySection, Factory.GetSection));

        // Создаем таблицу People
        var people = Factory.GetPeople(users, Factory.GetPerson);
        db.AddRange(people);

        // Создаем таблицу Staff
        db.AddRange(Factory.GetStaff(people, Factory.GetStaff));

        // Создаем таблицу House
        db.AddRange(Factory.GetHouses(quantitySection, Factory.GetHouse));

        // Создаем таблицу Subscriber
        var subscribers = Factory.GetSubscribers(people, Factory.GetSubscriber);
        db.AddRange(subscribers);

        db.SaveChanges();

        //TODO: должен быть вариант получше - переделать!
        // Создаем таблицу Subscription
        db.AddRange(Factory.GetSubscriptions(GetAllSubscribers(), Factory.GetSubscription));
        
        db.SaveChanges();
    } //AddTestEntities

    // Получение списка всех улиц
    public List<string> GetAllStreets() => db
        .Houses
        .Select(h => h.Street)
        .Distinct()
        .ToList();

    // Получение списка номеров домов по выбранной улице
    public List<string> GetHouseNumbersByStreet(string? selectStreet) => db 
        .Houses
        .Where(h => h.Street == selectStreet)
        .Select(h => h.HouseNumber)
        .ToList();

    // Проверка на совпадение логинов
    public async Task<bool> LoginExistAsync(string newLogin) =>
        await db.Users.AnyAsync(u => u.Login == newLogin);
    
} //DatabaseQueries
