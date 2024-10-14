using MailOffice.Models.Category;
using MailOffice.Models.Entities;
using MailOffice.Models.Reports;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MailOffice.Models.DataBase;

// Класс для запросов к БД
public class DatabaseQueries(MailOfficeContext context) {

    public DatabaseQueries() : this(new MailOfficeContext()) { }

    //Определить наименование и количество экземпляров всех изданий
    public List<ResultQuery01> Query01() => context
        .Publications
        .GroupBy(p => p.Name)
        .Select(g => new ResultQuery01(
            g.Key, 
            g.Count()))
        .ToList();

    //По заданному адресу определить фамилию почтальона, обслуживающего подписчика
    public List<string> Query02(string address) => context
        .Houses
        .Include(h => h.Section)
        .ThenInclude(s => s.Staff)
        .ThenInclude(s => s.Person)
        .Where(h => h.Street == address)
        .Select(g => g.Section.Staff.Person.SecondName)
        .ToList();

    //Какие газеты выписывает гражданин с указанной фамилией, именем, отчеством
    //public List<string> Query03(string address) => context

    //Сколько почтальонов работает в почтовом отделении
    public int Query04() => context
        .Staff
        .Where(s => s.Role == StaffRole.Postman)
        .Count();

    //На каком участке количество экземпляров подписных изданий максимально
    //public List<ResultQuery05> Query05() => context

    //Каков средний срок подписки по каждому изданию
    public List<ResultQuery06> Query06() => context
        .Subscriptions 
        .GroupBy(s => s.PublicationId)
        .Select(g => new ResultQuery06(
            g.Key, 
            g.Average(s => (int)s.Duration
        )))
        .ToList();

} //DatabaseQueries
