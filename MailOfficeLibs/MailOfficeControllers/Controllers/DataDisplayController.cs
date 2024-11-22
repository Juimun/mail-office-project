using System.Text;
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
    public string ShowReport(string currentLogin, byte[] currentPassword) {
        var sb = new StringBuilder();

        var cnt = 1;
        sb.AppendLine("\n\n\t\tОтчет о доставке почтой газет и журналов:\n\n");
        data.GetAllPostmans().ForEach(p => {

            // Отчет должен быть упорядочен по участкам
            sb.AppendLine(
                $"\t{cnt++}. " +
                $"{p.Section!.Name}, " +
                $"ID участка: {p.Section.Id}:\n" +

                // Для каждого участка указывается фамилия и инициалы почтальона, обслуживающего участок
                $"\tФИО почтальена: {p.Person.FullName}\n\n" +

                $"\t\t\tПеречень доставленных изданий:\n"
                );

            // Перечень доставляемых изданий 
            //  (индекс и название издания, адрес доставки, срок подписки).
            data.GetDeliveredSubscriptions(p).ForEach(s => {
                sb.AppendLine(
                    $"\tИндекс: {s.Publication.Id}\n" +
                    $"\tНазвание: {s.Publication.Name}\n" +
                    $"\tАдрес доставки: {s.Subscriber.House.Address}\n" +
                    $"\t\tСрок подписки:\n\t{s.StartDate} => {s.EndDate}\n" +
                    $"\tКоличество дней: {s.Duration}\n\n"
                    );
            });
        });

        // В отчете должно быть указано сколько почтальонов работает в почтовом отделении, 
        //  сколько всего участков оно обслуживает, сколько различных изданий доставляет подписчикам.
        cnt = 1;
        sb.AppendLine(
            $"\t\t\tОбщая информация:\n" +
            $"\tКоличество почтальонов: {data.PostmansCount()}\n" +
            $"\tОбслуживаемых участков: {data.ServesSectionCount()}/{data.GetAllSections().Count}\n\n" +

            $"\t\tКоличество доставляемых изданий:\n" +
            $"\t{cnt++}.Журналов => {data.DeliveredPublicationCount(PublicationType.Journal)} шт.\n" +
            $"\t{cnt++}.Газет    => {data.DeliveredPublicationCount(PublicationType.Newspaper)} шт.\n" +
            $"\t{cnt++}.Книг     => {data.DeliveredPublicationCount(PublicationType.Book)} шт." +

            $"\t\t{data.GetCurrentAccountFullName(currentLogin, currentPassword)}\t\t\t{DateTime.Now:f}"
            );

        return sb.ToString();
    } //ShowReport

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

