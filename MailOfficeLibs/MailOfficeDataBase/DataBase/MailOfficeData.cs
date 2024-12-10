using MailOfficeEntities.Entities;
using MailOfficeEntities.Entities.Accounts;
using MailOfficeEntities.Entities.Residence;
using MailOfficeEntities.Category;
using System.Collections.Generic;
using System.Collections;
using Microsoft.EntityFrameworkCore;
using MailOfficeFactory.Factories;
using MailOfficeTool.Entities;
using MailOfficeEntities.Entities.Receipts;

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

    // Список подписных изданий со статусом "В ожидании"
    public List<Subscription> GetAllAwaitingSubscription() => db 
        .Subscriptions
        .Where(s => s.SubscriptionStatus == SubscriptionStatus.Awaiting)
        .ToList();

    // Изменить статус подписного издания
    public async Task UpdateSubscriptionStatusAsync(Subscription subscription, SubscriptionStatus status, MailOfficeContext database) {
        subscription.SubscriptionStatus = status; 
        database.Update(subscription);

        await database.SaveChangesAsync();
    } //UpdateSubscriptionStatus

    // Изменить статус подписного издания
    public void RejectSubscriptionStatusAsync(Subscription subscription,  MailOfficeContext database) { 
        subscription.SubscriptionStatus = SubscriptionStatus.Rejected; 
        database.Update(subscription);

        // ТУТ удалить - если отклолить

        
        db.SaveChanges();
    } //UpdateSubscriptionStatus

    // Список пользователей, которые не являются персоналом
    public List<Person> GetAllPersonWithoutStaff() => db 
        .People
        .Where(p => p.Role != PersonCategory.Staff)
        .ToList();

    // Список активных подписных изданий пользователя
    public List<Subscription> GetAllActiveSubscription(string login, byte[] password) => db
            .Subscriptions
            .Where(s => s.Subscriber.Person.User.Login == login
                && s.Subscriber.Person.User.Password == password
                && s.SubscriptionStatus == SubscriptionStatus.Сonfirmed)
            .ToList()

            // Должно выполняться на клиенте
            .Where(s => s.EndDate >= DateTime.Now)
            .ToList();
     
    // Создание квитанции
    public void GetNewReceipt(string login, byte[] password, List<Publication> publications, SubscriptionPeriod selectedDuration, string firstName, string secondName, string patronymic, string sectionName, string street, string houseNumber) {  

        // Поиск пользователя 
        var user = db.Users.FirstOrDefault(u => u.Login == login && u.Password == password);

        // Если найден - создать новую квитанцию
        if (user == null) return;

        var startTime = DateTime.Now;
        if (user.Person.Role < PersonCategory.Subscriber) {

            // Сменить роль на подписчика
            var person = db.People.Where(p => p.User == user).FirstOrDefault()!;

            person.PreviousRole = person.Role;
            person.Role = PersonCategory.Subscriber;
            person.FirstName = firstName;
            person.SecondName = secondName;
            person.Patronymic = patronymic;

            db.People.Update(person);
            db.SaveChanges();

            // Создание нового дома
            db.Houses.Add(new House() { SectionId = db.Sections.Where(s => s.Name == sectionName).Select(s => s.Id).FirstOrDefault(), Street = street, HouseNumber = houseNumber });
            db.SaveChanges();

            db.Subscribers.Add(new Subscriber() { PersonId = person.Id, HouseId = db.Houses.Max(h => h.Id) });
            db.SaveChanges();

            var subscriberMaxId = db.Subscribers.Max(s => s.Id);

            // Создать подписное издание с выбранными публикациями
            publications.ForEach(p => {
                db.Subscriptions.Add(new Subscription()
                {
                    Duration = selectedDuration,
                    StartDate = startTime,
                    SubscriberId = subscriberMaxId,
                    PublicationId = p.Id,
                    SubscriptionStatus = SubscriptionStatus.Awaiting
                });
            });
        }

        var receipt = new Receipt() {
            Issuance = startTime,
            PersonId = user.Person.Id,
            Price = publications.Sum(p => p.Price)
        };

        db.Receipts.Add(receipt);
        db.SaveChanges();

        var receiptMaxId = db.Receipts.Max(r => r.Id); 
        publications.ForEach(p => db.ReceiptDetail.Add(
            new ReceiptDetail() {
                ReceiptId = receiptMaxId,
                Duration = selectedDuration,
                Name = p.Name
            }
            ));

        db.SaveChanges();
    } //CreateReceipt

    // Получение всех квитанций пользователя
    public List<Receipt> GetAllReceipts(string login, byte[] password) => db
        .Receipts
        .Where(r => r.Person.User.Login == login 
            && r.Person.User.Password == password)
        .ToList();

    // Создание списка для вывода в DataGrid
    public List<ReceiptWithDetail> GetAllReceiptsWithDetails(string login, byte[] password) => db
        .Receipts
        .Where(r => r.Person.User.Login == login
            && r.Person.User.Password == password)
        .Select(r => new ReceiptWithDetail(
            r.Price,
            r.Issuance,
            r.ReceiptDetails.ToList()
            ))
        .ToList();

    // Получить список наименований участков
    public List<string> GetAllSectionNames() => db
        .Sections
        .Select(s => s.Name)
        .ToList();
   
} //DatabaseQueries