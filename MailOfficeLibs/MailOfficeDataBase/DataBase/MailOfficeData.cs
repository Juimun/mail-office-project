using MailOfficeEntities.Entities;
using MailOfficeEntities.Entities.Accounts;
using MailOfficeEntities.Entities.Residence;
using MailOfficeEntities.Category;
using System.Collections.Generic;
using System.Collections;
using Microsoft.EntityFrameworkCore;

namespace MailOfficeDataBase.DataBase;

//Класс для получение всех записей для всех таблиц
public partial class DatabaseQueries(MailOfficeContext db) { 

    public DatabaseQueries() : this(new MailOfficeContext()) { }
     
    public List<User> GetAllUsers() => db 
        .Users
        .ToList();

    public List<Person> GetAllPeople() => db  
        .People
        .ToList();

    public List<Staff> GetAllStaff() => db
        .Staff
        .ToList();

    public List<Staff> GetAllPostmans() => db 
        .Staff
        .Where(s => s.Role == StaffRole.Postman)
        .ToList();

    public List<Subscriber> GetAllSubscribers() => db 
        .Subscribers
        .ToList();

    public List<Publication> GetAllPublications() => db
        .Publications
        .ToList();

    public List<Subscription> GetAllSubscriptions() => db
        .Subscriptions
        .ToList();

    public List<House> GetAllHouses() => db
        .Houses
        .ToList();

    public List<Section> GetAllSections() => db
         .Sections
         .ToList();

    // Получить список "страницы" публикаций
    //  offset - сколько записей нужно пропустить 
    //  pageSize - размер одной страницы
    public List<Publication> GetSelectPagePublication(int offset, int pageSize) => db 
        .Publications
        .Skip(offset)
        .Take(pageSize)
        .ToList();

    // Сколько всего записей в таблице Publications
    public int GetTotalPublicationRecords() => db
        .Publications
        .Count();

    // Получить список Subscription, которые связаны с Postman
    public List<Subscription> GetDeliveredSubscriptions(Staff postman) => db
        .Subscriptions
        .Include(s => s.Subscriber.House)
        .Where(s => s.Subscriber.House.SectionId == postman.SectionId)
        .Select(s => s)
        .ToList();

    // Средний срок подписки для изданий почтальена 
    public double GetAverageSubscription(List<Subscription> selectedSubcription) => selectedSubcription
        .Average(s => (s.EndDate - s.StartDate).TotalDays);

    // Количество различных подписных изданий
    public int GetPublicationCountsForSection(List<Subscription> selectedSubcription) => selectedSubcription
        .Distinct()
        .Count();

    public StaffRole? GetRoleCurrentAccount(string login, byte[] password) => db
        .Users
        .Include(u => u.Person.Staff)
        .Where(u => u.Login == login && u.Password == password && u.Person.Staff != null)
        .Select(u => u.Person.Staff!.Role)
        .FirstOrDefault();

    public bool IsSavedUser(string login, byte[] password) => db
        .Users
        .Any(u => u.Login == login && u.Password == password);
        
} //DatabaseQueries