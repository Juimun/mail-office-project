﻿using MailOfficeDataBase.Reports;
using MailOfficeEntities.Category;
using MailOfficeEntities.Entities;
using MailOfficeEntities.Entities.Accounts;
using MailOfficeFactory.Factories;
using MailOfficeTool.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
    public int PostmansCount() => db  
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
    public UserJson GetUserJson(string login, byte[] password) => db  
        .Users
        .Where(u => u.Login == login && u.Password == password)
        .Select(u => new UserJson(u.Login, u.Password))
        .First();

    // Регистрация нового пользователя 
    public void AddRegisteredUser(string newLogin, byte[] newPassword) {

        // Создаем сущность User 
        var newUser = new User() { Id = db.Users.Max(u => u.Id) + 1, Login = newLogin, Password = newPassword };
        db.Add(newUser);

        // Создаем сущность People и связываем с User
        db.Add(new Person() { Id = db.People.Max(u => u.Id) + 1, UserId = newUser.Id});
        
        db.SaveChanges();
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
    public bool LoginExist(string newLogin) => db
        .Users
        .Any(u => u.Login == newLogin);

    // Получение данных введенного аккаунта 
    public AccountProfile? GetAccount(string currentLogin, byte[] currentPassword) => db 
        .Users 
        .Include(u => u.Person)
            .ThenInclude(p => p.Staff)
            .ThenInclude(s => s!.Section)
        .Include(u => u.Person.Subscriber)
            .ThenInclude(s => s!.House)
        .Where(u => u.Login == currentLogin && u.Password == currentPassword)
        .Select(u => new AccountProfile( 
            u.Id,
            u.Login,
            u.Person.FirstName,
            u.Person.SecondName,
            u.Person.Patronymic,
            u.Person.Role,
            u.Person.Staff!.Role,
            u.Person.Subscriber!.House.Street,
            u.Person.Subscriber!.House.HouseNumber,
            u.Person.Staff!.Section!.Id,
            u.Person.Staff!.Section!.Name
            ))
        .FirstOrDefault();

    // Получение данных о всех профилях
    public List<AccountProfile> GetAccounts() => db  
        .Users 
        .Include(u => u.Person)
            .ThenInclude(p => p.Staff)
            .ThenInclude(s => s!.Section)
        .Include(u => u.Person.Subscriber)
            .ThenInclude(s => s!.House)
        .Select(u => new AccountProfile(
            u.Id,
            u.Login,
            u.Person.FirstName,
            u.Person.SecondName,
            u.Person.Patronymic,
            u.Person.Role,
            u.Person.Staff!.Role,
            u.Person.Subscriber!.House.Street,
            u.Person.Subscriber!.House.HouseNumber,
            u.Person.Staff!.Section!.Id,
            u.Person.Staff!.Section!.Name
            ))
        .ToList();

    // Получение списка подписных изданий введенного аккаунта
    public Subscription? GetCurrentSubscriptions(string currentLogin, byte[] currentPassword) => db
        .Subscriptions
        .Include(s => s.Subscriber)
            .ThenInclude(sub => sub.Person)
            .ThenInclude(p => p.User)
        .Where(s => s.Subscriber.Person.User.Login == currentLogin &&
                    s.Subscriber.Person.User.Password == currentPassword)
        .Select(s => s)
        .FirstOrDefault();

    // Проверка на роль Director или Administrator
    public bool IsDirector(string newLogin, byte[] newPassword) => db 
        .Users
        .Include(u => u.Person)
            .ThenInclude(s => s.Staff)
        .Where(u => u.Login == newLogin && u.Password == newPassword)
        .Any(u => u.Person.Staff!.Role == StaffRole.Director || u.Person.Staff.Role == StaffRole.Administrator);

    // Количество обслуживаемых участков
    public int ServesSectionCount() => db
        .Staff
        .Include(s => s.Section)
        .Where(s => s.Role == StaffRole.Postman)
        .Select(s => s.Section)
        .Count();

    public int DeliveredPublicationCount(PublicationType type) => db
        .Subscriptions
        .Include(s => s.Publication)
        .Include(s => s.Subscriber)
        .Where(s => s.Publication.Type == type && s.Subscriber != null)
        .Select (s => s.Publication)
        .Distinct()
        .Count();

    public string? GetCurrentAccountFullName(string currentLogin, byte[] currentPassword) {
        var user = db
            .Users
            .Include(s => s.Person)
            .FirstOrDefault(u => u.Login == currentLogin && u.Password == currentPassword);

        if (user == null) return null;
        return user.Person.FullName;
    }

    // Проверка аутентификации
    public bool IsAuthenticate(string newLogin, byte[] newPassword) => db
        .Users 
        .Any(u => u.Login == newLogin && u.Password == newPassword);
} //DatabaseQueries