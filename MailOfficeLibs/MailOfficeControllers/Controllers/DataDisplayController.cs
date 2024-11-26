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
        string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
        BaseFont bf = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        var font = new Font(bf, 12);
        var boldFont = new Font(bf, 12, Font.BOLD);
        var boldHeaderFont = new Font(bf, 18, Font.BOLD); 

        var paragraphs = new List<Paragraph> {
             
            // Параграф заголовка
            new("Отчет о доставке почтой газет и журналов:\n\n", new Font(bf, 20, Font.BOLD)) {
                IndentationLeft = 80
            } 
        }; //paragraphs

        int cnt = 1;
        data.GetAllPostmans().ForEach(p => {

            // Отчет должен быть упорядочен по участкам
            paragraphs.Add(new Paragraph([
                new Chunk($"{cnt++}. ", new Font(bf, 15)),
                new Chunk(p.Section!.Name, new Font(bf, 15, Font.ITALIC)),
                new Chunk(", ID участка: ", new Font(bf, Font.DEFAULTSIZE)),
                new Chunk($"{p.Section.Id}", new Font(bf, 15, Font.ITALIC)),
                new Chunk($", ФИО почтальена: ", new Font(bf, Font.DEFAULTSIZE)),
                new Chunk($"{p.Person.FullName}\n", new Font(bf, 15, Font.ITALIC)),

                new Chunk("Перечень доставленных изданий:", new Font(bf, 15))
            ]) {
                IndentationLeft = 24,
            }); 

            var subscriptionsForPostman = data.GetDeliveredSubscriptions(p);
            subscriptionsForPostman.ForEach(s => {

                // Перечень доставляемых изданий
                paragraphs.Add(new Paragraph([
                    new Chunk("Индекс: ", new Font(bf, 12)),
                    new Chunk($"{ s.Publication.Id }\n", new Font(bf, 15, Font.ITALIC)),

                    new Chunk("Название: ", new Font(bf, 12)),
                    new Chunk($"{s.Publication.Name}\n", new Font(bf, 15, Font.ITALIC)),

                    new Chunk("Адрес доставки: ", new Font(bf, 12)),
                    new Chunk($"{s.Subscriber.House.Address}\n", new Font(bf, 15, Font.ITALIC)),

                    new Chunk("Срок подписки:\n", new Font(bf, 12)),
                    new Chunk($"{s.StartDate}", new Font(bf, 15, Font.ITALIC)),
                    new Chunk("   =>  ", new Font(bf, Font.DEFAULTSIZE)),
                    new Chunk($"{s.EndDate}\n", new Font(bf, 15, Font.ITALIC)),

                    new Chunk("Количество дней: ", new Font(bf, 12)),
                    new Chunk($"{s.Duration}\n\n", new Font(bf, 15, Font.ITALIC))
                ]) {
                    IndentationLeft = 12 
                }); 
            }); //ForEach

            paragraphs.Add(new Paragraph([
                new Chunk("Средний срок подписки: ", new Font(bf, 12)),
                new Chunk($"{data.GetAverageSubscription(subscriptionsForPostman):F0}\n", new Font(bf, 15, Font.ITALIC)),

                new Chunk("Количество экземпляров: ", new Font(bf, 12)),
                new Chunk($"{subscriptionsForPostman.Count}\n", new Font(bf, 15, Font.ITALIC)),

                new Chunk("Количество различных подписных изданий: ", new Font(bf, 12)),
                new Chunk($"{data.GetPublicationCountsForSection(subscriptionsForPostman)}\n\n", new Font(bf, 15, Font.ITALIC)),
            ]) {
                IndentationLeft = 12
            });
        }); //ForEach

        cnt = 1;
        paragraphs.Add(new Paragraph("Общая информация:", new Font(bf, 20)) {
            IndentationLeft = 24
        });
        paragraphs.Add(new Paragraph([
             new Chunk("Количество почтальонов: ", new Font(bf, 15)),
             new Chunk($"{data.PostmansCount()}\n", new Font(bf, 20, Font.ITALIC)),

             new Chunk("Обслуживаемых участков: ", new Font(bf, 15)),
             new Chunk($"{data.ServesSectionCount()}", new Font(bf, 18, Font.ITALIC)),
             new Chunk(" / ", new Font(bf, Font.DEFAULTSIZE)),
             new Chunk($"{data.GetAllSections().Count}\n", new Font(bf, 18, Font.ITALIC)),

             new Chunk("Количество доставляемых изданий:\n", new Font(bf, 15)),
             new Chunk($"{cnt++}. ", new Font(bf, 15)),
             new Chunk("Журналов", new Font(bf, 15)),
             new Chunk("  =>   ", new Font(bf, Font.DEFAULTSIZE)),
             new Chunk($"{data.DeliveredPublicationCount(PublicationType.Journal)}", new Font(bf, 18, Font.ITALIC)),
             new Chunk(" шт.\n", new Font(bf, 15)),

             new Chunk($"{cnt++}. ", new Font(bf, 15)),
             new Chunk("Газет   ", new Font(bf, 15)),
             new Chunk("  =>   ", new Font(bf, Font.DEFAULTSIZE)),
             new Chunk($"{data.DeliveredPublicationCount(PublicationType.Newspaper)}", new Font(bf, 18, Font.ITALIC)),
             new Chunk(" шт.\n", new Font(bf, 15)),

             new Chunk($"{cnt++}. ", new Font(bf, 15)),
             new Chunk("Книг    ", new Font(bf, 15)),
             new Chunk("  =>   ", new Font(bf, Font.DEFAULTSIZE)),
             new Chunk($"{data.DeliveredPublicationCount(PublicationType.Book)}", new Font(bf, 18, Font.ITALIC)),
             new Chunk(" шт.\n\n\n", new Font(bf, 15)),

             new Chunk($"{data.GetCurrentAccountFullName(currentLogin, currentPassword)}             {DateTime.Now:f}", new Font(bf, 15, Font.BOLD)),
        ]) { 
            IndentationLeft = 12
        });

        return paragraphs;
    } //ShowReport

    /*
     * Нужна справка о количестве подписчиков, 
     *  количестве газет и количестве журналов, выписанных на текущий момент подписчиками.
     */
    public string ShowSubscribersStatement() {
        var sb = new StringBuilder();

        sb.AppendLine(
           $"\n\n\t\tCправка о\n" +
           $"\tколичестве подписчиков: {data.SubscribersCount()}\n" +
           $"\tколичестве газет:       {data.DeliveredPublicationCount(PublicationType.Newspaper)}\n" +
           $"\tколичестве журналов:    {data.DeliveredPublicationCount(PublicationType.Journal)}\n" +
           $"\tколичестве книг:        { data.DeliveredPublicationCount(PublicationType.Book)}\n\n" +
           $"\tCправка выписана в {DateTime.Now:f}."
           );
    
        return sb.ToString();
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

