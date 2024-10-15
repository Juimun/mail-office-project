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
        .People
        .Include(p => p.Subscribers) 
        .ThenInclude(s => s.Subscriptions)
        .ThenInclude(s => s.Publication)
        .Where(p => p.SecondName == secondName && p.FirstName == firstName && p.Patronymic == patronymic) 
        .SelectMany(p => p.Subscribers
            .SelectMany(s => s.Subscriptions
                .Select(sub => sub.Publication.Name)))
        .Distinct()
        .ToList();

    //Сколько почтальонов работает в почтовом отделении
    public int Query04() => db
        .Staff
        .Count(s => s.Role == StaffRole.Postman);

    //На каком участке количество экземпляров подписных изданий максимально
    //public List<ResultQuery05> Query05() => db 
    
    //Каков средний срок подписки по каждому изданию
    public List<ResultQuery06> Query06() => db
        .Subscriptions 
        .GroupBy(s => s.PublicationId)
        .Select(g => new ResultQuery06(
            g.Key, 
            g.Average(s => (int)s.Duration
        )))
        .ToList();

} //DatabaseQueries
