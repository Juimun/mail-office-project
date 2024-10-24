using MailOffice.Infrastructure;
using MailOffice.Models.Category;
using MailOffice.Models.Reports;
using Microsoft.EntityFrameworkCore;

namespace MailOffice.Models.DataBase;

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

    #region Максимальный Id в БД
    //Какой Id в таблице User максимальный
    private Task<int> MaxUserIdQueryAsync() => db 
        .Users
        .Select(g => g.Id)
        .MaxAsync();

    //Какой Id в таблице Person максимальный 
    private Task<int> MaxPersonIdQueryAsync() => db
        .People
        .Select(g => g.Id)
        .MaxAsync();
    #endregion

    #region Добавление сущности в БД 
    // Асинхронный запрос для создания случайной сущности User
    public async void AddUser() { 
        // Асинхронное получение максимальных ID
        var maxUserId = await MaxUserIdQueryAsync() + 1;
        var maxPersonId = await MaxPersonIdQueryAsync() + 1;

        db
            .Users
            .Add(Factory.GetUser(maxUserId));

        db
            .People
            .Add(Factory.GetPerson(maxPersonId, maxUserId));

        await db.SaveChangesAsync();
    } // AddUser


    #endregion

} //DatabaseQueries
