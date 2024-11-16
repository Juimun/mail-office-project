using MailOfficeEntities.Entities;
using MailOfficeEntities.Entities.Accounts;
using MailOfficeEntities.Entities.Residence;
using MailOfficeEntities.Category;

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

} //DatabaseQueries