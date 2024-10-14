using MailOffice.Models.DataBase;
using System.Text;

namespace MailOffice.Controllers;

//Контроллер для Формирования строкового представления запросов к БД
public class QueryDisplayController(DatabaseQueries query) {

    public QueryDisplayController() : this(new DatabaseQueries()) { } 

    public string ShowQuery1() { 
        var sb = new StringBuilder();

        var result = query.Query01();
        result.ForEach(q => sb.AppendLine( 
            $"Наименование: {q.Name}, " +
            $"Количество: {q.Quintity} шт."  
            ));

        return sb.ToString();
    } //ShowQuery1

    public string ShowQuery2(string address) { 
        var sb = new StringBuilder();

        var result = query.Query02(address);
        sb.AppendLine($"По заданному адресу: {address}, работает почтальен с такой фамилией ->");
        result.ForEach(q => sb.AppendLine($"{q}\n"));

        return sb.ToString();
    } //ShowQuery2

    public string ShowQuery4() {
        var sb = new StringBuilder();

        var result = query.Query04();
        sb.AppendLine($"Почтальонов в почтовом отделении: {result}");

        return sb.ToString();
    } //ShowQuery4

    public string ShowQuery6() {
        var sb = new StringBuilder();

        var result = query.Query06();
        sb.AppendLine($"Средний срок подписки по каждому изданию:");
        result.ForEach(q => sb.AppendLine(
            $"Id публикации: {q.Id}, " +
            $"Средний срок публикации: {q.Avg}"
            ));

        return sb.ToString();
    } //ShowQuery4

} //QueryDisplayController
