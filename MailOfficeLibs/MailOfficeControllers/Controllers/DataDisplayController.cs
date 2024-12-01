using System.Data;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MailOfficeDataBase.DataBase;
using MailOfficeEntities.Category;
using MailOfficeEntities.Entities;
using MailOfficeTool.Infrastructure;

namespace MailOfficeControllers.Controllers;

//Контроллер для Формирования строкового представления таблиц в БД
public partial class DatabaseDisplayController(DatabaseQueries data) {

    public DatabaseDisplayController() : this(new DatabaseQueries()) { }

    public string ShowAllTables() {   
        var sb = new StringBuilder();

        sb.AppendLine($" Таблица People:\n{ShowPeople()}")
          .AppendLine($" Таблица Staff:\n{ShowStaff()}")
          .AppendLine($" Таблица Users:\n{ShowUsers()}")
          .AppendLine($" Таблица Sections:\n{ShowSections()}")
          .AppendLine($" Таблица Houses:\n{ShowHouses()}")
          .AppendLine($" Таблица Subscriptions:\n{ShowSubscriptions()}")
          .AppendLine($" Таблица Publications:\n{ShowPublications()}"); 

        return sb.ToString();
    } //ShowAllTables

    public string ShowPeople() {
        var sb = new StringBuilder();

        var people = data.GetAllPeople();
        people.ForEach(p => sb.AppendLine(
            $"ID: {p.Id}, " +
            $"Имя: {p.FullName}, " +
            $"Тип: {p.Role}"
            ));

        return sb.ToString();
    } //ShowPeople

    public string ShowUsers() { 
        var sb = new StringBuilder();

        var users = data.GetAllUsers();
        users.ForEach(u => sb.AppendLine(
            $"ID: {u.Id}, " +
            $"Логин: {u.Login}, " +
            $"Пароль: {Utils.GetString(u.Password)}"
        ));

        return sb.ToString();
    } //ShowUsers

    public string ShowStaff() {  
        var sb = new StringBuilder();

        var users = data.GetAllStaff();
        users.ForEach(s => sb.AppendLine(
            $"ID: {s.Id}, " +
            $"Должность: {s.Role}" 
        ));

        return sb.ToString();
    } //ShowStaff

    public string ShowSections() {  
        var sb = new StringBuilder();

        var users = data.GetAllSections();
        users.ForEach(s => sb.AppendLine(
            $"ID: {s.Id}, " +
            $"Наименование участка: {s.Name}" 
        ));

        return sb.ToString();
    } //ShowSections

    public string ShowHouses() {
        var sb = new StringBuilder();

        var users = data.GetAllHouses();
        users.ForEach(h => sb.AppendLine(
            $"ID: {h.Id}, " +
            $"Адрес: {h.Address}"
        ));

        return sb.ToString();
    } //ShowHouses

    public string ShowSubscriptions() {
        var sb = new StringBuilder();

        var users = data.GetAllSubscriptions();
        users.ForEach(s => sb.AppendLine(
            $"ID: {s.Id}, " +
            $"Срок подписки: {s.Duration}, " +
            $"Дата начала подписки: {s.StartDate:dd.MM.yyyy}, " +
            $"Дата конца подписки: {s.EndDate:dd.MM.yyyy}"
        ));

        return sb.ToString();
    } //ShowSubscriptions

    public string ShowPublications() {  
        var sb = new StringBuilder();

        var users = data.GetAllPublications();
        users.ForEach(p => sb.AppendLine(
            $"ID: {p.Id}, " + 
            $"Наименование: {p.Name}, " +
            $"Тип издания: {p.Type}, " +
            $"Цена: {p.Price}" 
        ));

        return sb.ToString();
    } //ShowPublications

    // Создание строкового представления изданий с ограничением
    public string ShowPublications(List<Publication> publications) {
        var sb = new StringBuilder();

        int сnt = 0, maxLength = 40; 
        publications.ForEach(s => sb.AppendLine(
            $"{сnt++}.   {(s.Name.Length > maxLength ? s.Name[..(maxLength - 3)] + "..." : s.Name)}   {s.Type}   {s.Price:N0}р.\n"
            ));

        return sb.ToString();
    } //ShowPublications

    // Создание профиля введенного аккаунта
    public string ShowCurrentProfile(string currentLogin, byte[] currentPassword) {  
        var sb = new StringBuilder();
        
        var currentAccount = data.GetAccount(currentLogin, currentPassword);

        sb.AppendLine("\n\n\t\t\tМой профиль:")
        
          // Основная информация
          .AppendLine(
              $"\tID: {currentAccount!.UserId}\n" +
              $"\tИмя пользователя: {currentAccount.Login}\n\n" +
          
              $"\tФамилия: {currentAccount.SecondName}\n" +
              $"\tИмя: {currentAccount.FirstName}\n" +
              $"\tОтчество: {currentAccount.Patronymic}\n")
              .AppendLine(
              currentAccount.PersonRole == PersonCategory.Staff
              ? $"\tРоль сотрудника: {currentAccount.StaffRole}"
              : $"\tРоль: {currentAccount.PersonRole}")
              .AppendLine("\n\n");
          
          // Участок - для Postman
          if (currentAccount.StaffRole == StaffRole.Postman) {
              sb.AppendLine(
                  $"\t\tУчасток:\n" +
                  $"\tЗакреплен за: {currentAccount.SectionId}\n" +
                  $"\tНаименование: {currentAccount.SectionName}\n"
                  );
          } //if
          
          // Адрес - для Subscriber
          if (currentAccount.PersonRole == PersonCategory.Subscriber) {
              sb.AppendLine(
                  $"\t\tАдрес:\n" +
                  $"\tУлица: {currentAccount.Street}\n" +
                  $"\tНомер дома: {currentAccount.HouseNumber}\n"
                  );
          } //if
       
        return sb.ToString();
    } //ShowCurrentProfile
     
    public string ShowSubscriptionsCurrentUser(string currentLogin, byte[] currentPassword) {
        var sb = new StringBuilder();

        var currentAccount = data.GetCurrentSubscriptions(currentLogin, currentPassword);
        if (currentAccount == null) return sb.ToString();

        sb.AppendLine("\n\n\t\tМои подписные издания:")
          .AppendLine(
            $"\tID: {currentAccount!.Id}\n" +
            $"\tСрок подписки: {currentAccount.Duration}\n" +
            $"\tДата начала подписки: {currentAccount.StartDate:dd.MM.yyyy}\n" +
            $"\tДата конца подписки: {currentAccount.EndDate:dd.MM.yyyy}\n"
        );

        return sb.ToString();
    } //ShowSubscriptionCurrentUser

    /*
     * Требуется формирование отчета о доставке почтой газет и журналов. 
     * Отчет должен быть упорядочен по участкам.
     * Для каждого участка указывается фамилия и инициалы почтальона, обслуживающего участок, и перечень доставляемых изданий 
     *  (индекс и название издания, адрес доставки, срок подписки). 
     * По каждому изданию указывается средний срок подписки и количество экземпляров, 
     *  а по участку – количество различных подписных изданий. 
     *
     * В отчете должно быть указано сколько почтальонов работает в почтовом отделении, 
     *  сколько всего участков оно обслуживает, сколько различных изданий доставляет подписчикам.
    */
    public List<Paragraph> ShowReport(string currentLogin, byte[] currentPassword) {
        var paragraphs = new List<Paragraph> {
             
            // Заголовок отчета
            new("Отчет о доставке почтой газет и журналов:\n\n", Utils.GetArialFont(20, Font.BOLD)) {
                IndentationLeft = 80
            } 
        }; //paragraphs

        int cnt = 1;
        data.GetAllPostmans().ForEach(p => {

            // Отчет должен быть упорядочен по участкам
            paragraphs.Add(new Paragraph([
                new Chunk($"{cnt++}. ", Utils.GetArialFont()),
                new Chunk(p.Section!.Name, Utils.GetArialFont(style: Font.ITALIC)),
                new Chunk(", ID участка: ", Utils.GetArialFont(Font.DEFAULTSIZE)),
                new Chunk($"{p.Section.Id}", Utils.GetArialFont(style: Font.ITALIC)),
                new Chunk($", ФИО почтальена: ", Utils.GetArialFont(Font.DEFAULTSIZE)),
                new Chunk($"{p.Person.FullName}\n", Utils.GetArialFont(style: Font.ITALIC)),

                new Chunk("Перечень доставленных изданий:", Utils.GetArialFont(style: Font.ITALIC))
            ]) {
                IndentationLeft = 24,
            }); 

            var subscriptionsForPostman = data.GetDeliveredSubscriptions(p);
            subscriptionsForPostman.ForEach(s => {

                // Перечень доставляемых изданий
                paragraphs.Add(new Paragraph([
                    new Chunk("Индекс: ", Utils.GetArialFont(Font.DEFAULTSIZE)),
                    new Chunk($"{ s.Publication.Id }\n", Utils.GetArialFont(style: Font.ITALIC)),

                    new Chunk("Название: ",Utils.GetArialFont(Font.DEFAULTSIZE)),
                    new Chunk($"{s.Publication.Name}\n", Utils.GetArialFont(style: Font.ITALIC)),

                    new Chunk("Адрес доставки: ", Utils.GetArialFont(Font.DEFAULTSIZE)),
                    new Chunk($"{s.Subscriber.House.Address}\n", Utils.GetArialFont(style: Font.ITALIC)),

                    new Chunk("Срок подписки:\n", Utils.GetArialFont(Font.DEFAULTSIZE)),
                    new Chunk($"{s.StartDate}", Utils.GetArialFont(style: Font.ITALIC)),
                    new Chunk("   =>  ", Utils.GetArialFont(Font.DEFAULTSIZE)),
                    new Chunk($"{s.EndDate}\n", Utils.GetArialFont(style: Font.ITALIC)),

                    new Chunk("Количество дней: ", Utils.GetArialFont(Font.DEFAULTSIZE)),
                    new Chunk($"{s.Duration}\n\n", Utils.GetArialFont(style: Font.ITALIC))
                ]) {
                    IndentationLeft = 12 
                }); 
            }); //ForEach

            paragraphs.Add(new Paragraph([
                new Chunk("Средний срок подписки: ", Utils.GetArialFont(Font.DEFAULTSIZE)),
                new Chunk($"{data.GetAverageSubscription(subscriptionsForPostman):F0}\n", Utils.GetArialFont(style: Font.ITALIC)),

                new Chunk("Количество экземпляров: ", Utils.GetArialFont(Font.DEFAULTSIZE)),
                new Chunk($"{subscriptionsForPostman.Count}\n", Utils.GetArialFont(style: Font.ITALIC)),

                new Chunk("Количество различных подписных изданий: ",Utils.GetArialFont(Font.DEFAULTSIZE)),
                new Chunk($"{data.GetPublicationCountsForSection(subscriptionsForPostman)}\n\n", Utils.GetArialFont(style: Font.ITALIC)),
            ]) {
                IndentationLeft = 12
            });
        }); //ForEach

        cnt = 1;
        paragraphs.Add(new Paragraph("Общая информация:", Utils.GetArialFont(20, Font.BOLD)) {
            IndentationLeft = 24
        });
        paragraphs.Add(new Paragraph([
             new Chunk("Количество почтальонов: ", Utils.GetArialFont()),
             new Chunk($"{data.PostmansCount()}\n", Utils.GetArialFont(18, Font.ITALIC)),

             new Chunk("Обслуживаемых участков: ", Utils.GetArialFont()),
             new Chunk($"{data.ServesSectionCount()}", Utils.GetArialFont(18, Font.ITALIC)),
             new Chunk(" / ", Utils.GetArialFont(Font.DEFAULTSIZE)),
             new Chunk($"{data.GetAllSections().Count}\n", Utils.GetArialFont(18, Font.ITALIC)),

             new Chunk("Количество доставляемых изданий:\n", Utils.GetArialFont()),
             new Chunk($"{cnt++}. ", Utils.GetArialFont()),
             new Chunk("Журналов", Utils.GetArialFont()),
             new Chunk("  =>   ", Utils.GetArialFont(Font.DEFAULTSIZE)),
             new Chunk($"{data.DeliveredPublicationCount(PublicationType.Journal)}", Utils.GetArialFont(18, Font.ITALIC)),
             new Chunk(" шт.\n", Utils.GetArialFont()),

             new Chunk($"{cnt++}. ", Utils.GetArialFont()),
             new Chunk("Газет   ", Utils.GetArialFont()),
             new Chunk("  =>   ", Utils.GetArialFont(Font.DEFAULTSIZE)),
             new Chunk($"{data.DeliveredPublicationCount(PublicationType.Newspaper)}", Utils.GetArialFont(18, Font.ITALIC)),
             new Chunk(" шт.\n", Utils.GetArialFont()),

             new Chunk($"{cnt++}. ", Utils.GetArialFont()),
             new Chunk("Книг    ", Utils.GetArialFont()),
             new Chunk("  =>   ", Utils.GetArialFont(size: Font.DEFAULTSIZE)),
             new Chunk($"{data.DeliveredPublicationCount(PublicationType.Book)}", Utils.GetArialFont(18, Font.ITALIC)),
             new Chunk(" шт.\n\n\n", Utils.GetArialFont()),

             new Chunk($"{data.GetCurrentAccountFullName(currentLogin, currentPassword)}             {DateTime.Now:f}", Utils.GetArialFont(style: Font.BOLD)),
        ]) { 
            IndentationLeft = 12
        });

        return paragraphs;
    } //ShowReport

    /*
     * Нужна справка о количестве подписчиков, 
     *  количестве газет и количестве журналов, выписанных на текущий момент подписчиками.
     */
    public List<Paragraph> ShowSubscribersStatement() {
        return [
            
            // Заголовок справки
            new($"Справка на {DateTime.Now:f}:\n\n", Utils.GetArialFont(20, Font.BOLD))
            {
                IndentationLeft = 80
            },
            
            // Данные справки
            new([
            new Chunk("Количестве подписчиков: ", Utils.GetArialFont()),
            new Chunk($"{data.SubscribersCount()}\n", Utils.GetArialFont(18, Font.ITALIC)),
            new Chunk("Количестве газет: ", Utils.GetArialFont()),
            new Chunk($"{data.DeliveredPublicationCount(PublicationType.Newspaper)}\n", Utils.GetArialFont(18, Font.ITALIC)),
            new Chunk("количестве журналов: ", Utils.GetArialFont()),
            new Chunk($"{data.DeliveredPublicationCount(PublicationType.Journal)}\n", Utils.GetArialFont(18, Font.ITALIC)),
            new Chunk("количестве книг: ", Utils.GetArialFont()),
            new Chunk($"{data.DeliveredPublicationCount(PublicationType.Book)}\n", Utils.GetArialFont(18, Font.ITALIC)),
        ])
            {
                IndentationLeft = 12
            }
        ];
    } //ShowSubscribersStatement

    // Строковое представление "страницы" списка
    public string ShowPagePublication(List<Publication> publications) {
        var sb = new StringBuilder();

        publications.ForEach(p => sb.AppendLine(
            $"ID: {p.Id}, " +
            $"Наименование: {p.Name}, " +
            $"Тип издания: {p.Type}, " +
            $"Цена: {p.Price}"
        ));

        return sb.ToString();
    } //ShowPagePublication
} //DataDisplayController

