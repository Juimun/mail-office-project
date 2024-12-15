using MailOfficeDataBase.Reports;
using MailOfficeEntities.Category;
using MailOfficeEntities.Entities;
using MailOfficeEntities.Entities.Accounts;
using MailOfficeEntities.Entities.Receipts;
using MailOfficeEntities.Entities.Residence;
using MailOfficeFactory.Factories;
using MailOfficeTool.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Utils = MailOfficeTool.Infrastructure.Utils;

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
        db.Add(new User() { Login = newLogin, Password = newPassword });
        db.SaveChanges();

        // Создаем сущность People и связываем с User
        db.Add(new Person() { UserId = db.Users.Max(u => u.Id)});   
        db.SaveChanges();
    } //AddNewUser

    //Создание сужностей для тестов
    public void AddTestEntities() {          
        //Всего тестовых пользователей 
        int users = 1_000, quantityPublication = 450, quantitySection = 100;  

        // Создаем таблицу Users
        db.AddRange(Factory.GetUsers(users, Factory.GetUser));
        db.SaveChanges();

        // Создаем таблицу Publication
        db.AddRange(Factory.GetPublications(quantityPublication, Factory.GetPublication));

        // Создаем таблицу Section
        db.AddRange(Factory.GetSections(quantitySection, Factory.GetSection));
        
        // Создаем таблицу People
        db.AddRange(Factory.GetPeople(GetAllUsers(), Factory.GetPerson));
        db.SaveChanges();

        // Создаем таблицу House
        db.AddRange(Factory.GetHouses(GetAllSections(), Factory.GetHouse));

        // Создаем таблицу Subscriber
        db.AddRange(Factory.GetSubscribers(GetAllPeople(), Factory.GetSubscriber));
        
        // Создаем таблицу Staff
        db.AddRange(Factory.GetStaff(GetAllPeople(), Factory.GetStaff));
        db.SaveChanges();
       
        // Создаем таблицу Subscription
        db.AddRange(Factory.GetSubscriptions(GetAllSubscribers(), Factory.GetSubscription));
        db.SaveChanges();
    } //AddTestEntities

    // Получение списка всех уникальных улиц
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
        .Include(u => u.Person.Staff!.Section)
        .Include(u => u.Person.Subscriber!.House)
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

    // Проверка на роль Administrator
    public bool IsAdmin(string newLogin, byte[] newPassword) => db 
        .Users
        .Include(u => u.Person)
            .ThenInclude(s => s.Staff)
        .Where(u => u.Login == newLogin && u.Password == newPassword)
        .Any(u => u.Person.Staff!.Role == StaffRole.Administrator);

    // Проверка на роль Director 
    public bool IsDirector(string newLogin, byte[] newPassword) => db 
        .Users
        .Include(u => u.Person)
            .ThenInclude(s => s.Staff)
        .Where(u => u.Login == newLogin && u.Password == newPassword)
        .Any(u => u.Person.Staff!.Role == StaffRole.Director);

    // Проверка на роль Operator
    public bool IsOperator(string newLogin, byte[] newPassword) => db
        .Users 
        .Include(u => u.Person)
            .ThenInclude(s => s.Staff)
        .Where(u => u.Login == newLogin && u.Password == newPassword)
        .Any(u => u.Person.Staff!.Role == StaffRole.Operator);

    // Проверка на роль Postman
    public bool IsPostman(string newLogin, byte[] newPassword) => db 
        .Users
        .Include(u => u.Person)
            .ThenInclude(s => s.Staff)
        .Where(u => u.Login == newLogin && u.Password == newPassword)
        .Any(u => u.Person.Staff!.Role == StaffRole.Postman);

    // Уволить почтальена
    public bool RemovePostman(int staffId) {
        var selectedPostman = db 
            .Staff
            .Where(s => s.Role == StaffRole.Postman)
            .FirstOrDefault(s => s.Id == staffId);

        if (selectedPostman == null) 
            return false;

        // Восстанавливаем старую роль
        selectedPostman.Person.Role = selectedPostman.Person.PreviousRole;

        db.Update(selectedPostman.Person);
        db.Remove(selectedPostman);

        db.SaveChanges();
        return true;
    } //RemovePostman

    // Добавить почтальена
    public bool AddPostman(int personId) { 
        // Находим пользователя
        var selectedPerson = db
            .People
            .Where(s => s.Role != PersonCategory.Staff)
            .FirstOrDefault(p => p.Id == personId);

        // Находим свободный участок
        var freeSection = db
            .Sections
            .FirstOrDefault(s => s.Staff.SectionId == null);

        if (selectedPerson == null || freeSection == null) 
            return false;

        // Меняем роль персоны и сохраняем старую
        selectedPerson.PreviousRole = selectedPerson.Role; 
        selectedPerson.Role = PersonCategory.Staff;
        
        db.Update(selectedPerson);

        // Добавляем новую запись Staff
        db.Staff.Add(new Staff() { PersonId = selectedPerson.Id, Role = StaffRole.Postman, SectionId = freeSection.Id });

        db.SaveChanges();
        return true;
    } //AddPostman
     
    // Для удобства тестов
    // Создание списка данных всех аккаунтов
    public List<string> GetAllAccountAuthorization() => db
        .Users
        .Include(u => u.Person.Staff)
        .Select(u => $" {u.Login,-18} | {Utils.GetString(u.Password),-19} |" +
        $" {(u.Person.Staff != null ? u.Person.Staff.Role.ToString() : null)}\n")
        .ToList();
    
         
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
    } //GetCurrentAccountFullName

    // Проверка аутентификации
    public bool IsAuthenticate(string newLogin, byte[] newPassword) => db
        .Users 
        .Any(u => u.Login == newLogin && u.Password == newPassword);

    // Получение уникальных имен подписчиков газет
    public List<string> GetNewspaperSubscriberFirstNames() => db
        .Subscriptions
        .Include(s => s.Publication)
        .Include(s => s.Subscriber)
            .ThenInclude(s => s.Person)
        .Where(s => s.Subscriber.Person.Role == PersonCategory.Subscriber 
                    && s.Publication.Type == PublicationType.Newspaper)
        .Select(s => s.Subscriber.Person.FirstName) 
        .Distinct()
        .ToList();

    // Получение уникальных фамилий подписчиков газет по имени
    public List<string> GetNewspaperSubscriberSecondNames(string? firstName) => db 
        .Subscriptions
        .Include(s => s.Publication)
        .Include(s => s.Subscriber)
            .ThenInclude(s => s.Person)
        .Where(s => s.Subscriber.Person.FirstName == firstName
                    && s.Subscriber.Person.Role == PersonCategory.Subscriber
                    && s.Publication.Type == PublicationType.Newspaper)
        .Select(s => s.Subscriber.Person.SecondName)
        .Distinct()
        .ToList();

    // Получение уникальных отчеств подписчиков газет по имени и фамилии
    public List<string> GetNewspaperSubscriberPatronymics(string? firstName, string? secondName) => db 
        .Subscriptions
        .Include(s => s.Publication)
        .Include(s => s.Subscriber)
            .ThenInclude(s => s.Person)
        .Where(s => s.Subscriber.Person.FirstName == firstName
                    && s.Subscriber.Person.SecondName == secondName
                    && s.Subscriber.Person.Role == PersonCategory.Subscriber
                    && s.Publication.Type == PublicationType.Newspaper)
        .Select(s => s.Subscriber.Person.Patronymic)
        .Distinct()
        .ToList();

    // Количество подписчиков в БД
    public int SubscribersCount() => db
         .Subscribers
         .Count();

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
    public async Task UpdateSubscriptionStatusAsync(Subscription subscription, SubscriptionStatus status, MailOfficeContext database)
    {
        subscription.SubscriptionStatus = status;
        database.Update(subscription);

        await database.SaveChangesAsync();
    } //UpdateSubscriptionStatus

    // Изменить статус подписного издания
    public void RejectSubscriptionStatusAsync(Subscription subscription, MailOfficeContext database)
    {
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
    public void GetNewReceipt(
        string login, byte[] password, List<Publication> publications,
        SubscriptionPeriod selectedDuration, string firstName, string secondName,
        string patronymic, string sectionName, string street, string houseNumber)
    {

        // Поиск пользователя 
        var user = db
            .Users
            .FirstOrDefault(u => u.Login == login && u.Password == password);

        // Если найден - создать новую квитанцию
        if (user == null) return;

        var startTime = DateTime.Now;
       
        // Сменить роль на подписчика
        var person = db
            .People
            .Where(p => p.User == user)
            .FirstOrDefault()!;

        person.PreviousRole = person.Role;
        person.Role = PersonCategory.Subscriber;
        person.FirstName = firstName;
        person.SecondName = secondName;
        person.Patronymic = patronymic;

        db.People.Update(person);
        db.SaveChanges();

        // Создание нового дома
        db.Houses
          .Add(new House() { 
              SectionId = db
                  .Sections
                  .Where(s => s.Name == sectionName)
                  .Select(s => s.Id)
                  .FirstOrDefault(), 
              Street = street, 
              HouseNumber = houseNumber });
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

        var receipt = new Receipt()
        {
            Issuance = startTime,
            PersonId = user.Person.Id,
            Price = publications.Sum(p => p.Price)
        };

        db.Receipts.Add(receipt);
        db.SaveChanges();

        var receiptMaxId = db.Receipts.Max(r => r.Id);
        publications.ForEach(p => db.ReceiptDetail.Add(
            new ReceiptDetail()
            {
                ReceiptId = receiptMaxId,
                Duration = selectedDuration,
                Name = p.Name
            }
            ));

        db.SaveChanges();
    } //CreateReceipt

    public void GetNewReceipt(
        string login, byte[] password, List<Publication> publications, SubscriptionPeriod selectedDuration)
    {

        // Поиск пользователя 
        var user = db
            .Users
            .FirstOrDefault(u => u.Login == login && u.Password == password);

        // Если найден - создать новую квитанцию
        if (user == null) return;

        var startTime = DateTime.Now;
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

        var receipt = new Receipt()
        {
            Issuance = startTime,
            PersonId = user.Person.Id,
            Price = publications.Sum(p => p.Price)
        };

        db.Receipts.Add(receipt);
        db.SaveChanges();

        var receiptMaxId = db.Receipts.Max(r => r.Id);
        publications.ForEach(p => db.ReceiptDetail.Add(
            new ReceiptDetail()
            {
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

    // Проверка на роль подписчика и выше
    public bool IsSubscriberOrStaff(string login, byte[] password) => db 
        .People
        .Any(p => p.User.Login == login && p.User.Password == password 
            && p.Role >= PersonCategory.Subscriber);

} //DatabaseQueries
