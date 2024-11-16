using System.Text;

namespace MailOfficeControllers.Controllers;

//Контроллер для Формирования строкового представления запросов к БД
public partial class DatabaseDisplayController { 

    public string ShowQuery1() {  
        var sb = new StringBuilder();

        var result = data.Query01();
        result.ForEach(q => sb.AppendLine( 
            $"Наименование: {q.Name}, " +
            $"Количество: {q.Quintity} шт."  
            ));

        return sb.ToString();
    } //ShowQuery1
     
    public string ShowQuery2(string street, string houseNumber) { 
        var sb = new StringBuilder();

        var result = data.Query02(street, houseNumber);
        sb.AppendLine($"По заданному адресу: {street} дом {houseNumber}, работает почтальен с такой фамилией ->");
        result.ForEach(q => sb.AppendLine($"{q}, "));

        return sb.ToString();
    } //ShowQuery2

    public string ShowQuery3(string secondName, string firstName, string patronymic) {  
        var sb = new StringBuilder();

        var result = data.Query03(secondName, firstName, patronymic);
        sb.AppendLine($"С указанным ФИО: {secondName} {firstName[0]}.{patronymic[0]}, пользователь был подписан на ->");
        result.ForEach(q => sb.AppendLine($"{q}, "));

        return sb.ToString();
    } //ShowQuery3

    public string ShowQuery4() {
        var sb = new StringBuilder();

        var result = data.PostmansCount();
        sb.AppendLine($"Почтальонов в почтовом отделении: {result}");

        return sb.ToString();
    } //ShowQuery4

    public string ShowQuery5() { 
        var sb = new StringBuilder();

        var result = data.Query05();
        sb.AppendLine($"На участке {result?.SectionName} количество экземпляров подписных изданий максимально.\nЕго количество: {result?.Quintity}");

        return sb.ToString();
    } //ShowQuery5

    public string ShowQuery6() {
        var sb = new StringBuilder();

        var result = data.Query06();
        sb.AppendLine($"Средний срок подписки по каждому изданию:");
        result.ForEach(q => sb.AppendLine(
            $"Id публикации: {q.Id}, " +
            $"Средний срок публикации: {q.Avg}"
            ));

        return sb.ToString();
    } //ShowQuery4

} //QueryDisplayController
