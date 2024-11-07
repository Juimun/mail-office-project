using System.Text;
using MailOfficeDataBase.DataBase;
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

    public async Task<string> ShowSubscriptions() {
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

} //DataDisplayController

