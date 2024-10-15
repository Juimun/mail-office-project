using MailOffice.Models.Entities;
using MailOffice.Models.Entities.Accounts;
using MailOffice.Models.Entities.Residence;

namespace MailOffice.Models.DataBase;

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