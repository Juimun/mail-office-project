using MailOfficeEntities.Entities;
using MailOfficeEntities.Entities.Accounts;
using MailOfficeEntities.Entities.Residence;
using MailOfficeEntities.Category;
using System.Collections.Generic;
using System.Collections;

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

} //DatabaseQueries