using MailOffice.Controllers;
using MailOffice.Infrastructure;
using MailOffice.View;
using System.Text;
using System.Windows;

namespace MailOffice.ViewModel;

public class MainWindowViewModel {

    // Контроллер для визуализации
    private readonly DatabaseDisplayController _data; 

    public MainWindow HostWindow { get; set; }

    public MainWindowViewModel(
        MainWindow hostWindow, DatabaseDisplayController dataController) 
    { 
        (HostWindow, _data) = 
            (hostWindow, dataController);

        var sb = new StringBuilder("\n");
        sb.AppendLine($"{_data.ShowAllTables()}\n")
          .AppendLine($"Запрос №1\nОпределить наименование и количество экземпляров всех изданий\n{_data.ShowQuery1()}\n\n")
          .AppendLine($"Запрос №2\nПо заданному адресу определить фамилию почтальона, обслуживающего подписчика\n{_data.ShowQuery2("ул.Артема", "140")}\n\n")
          .AppendLine($"Запрос №3\nКакие газеты выписывает гражданин с указанной ФИО\n{_data.ShowQuery3("Кушнарева", "Анна", "Сергеевна")}\n\n")
          .AppendLine($"Запрос №4\nСколько почтальонов работает в почтовом отделении\n{_data.ShowQuery4()}\n\n")
          .AppendLine($"Запрос №5\nНа каком участке количество экземпляров подписных изданий максимально\n{_data.ShowQuery5()}\n\n")
          .AppendLine($"Запрос №6\nКакой средний срок подписки по каждому изданию\n{_data.ShowQuery6()}\n\n");
        
        HostWindow.TblTables.Text = sb.ToString();
    } // MainWindowViewModel

    public RelayCommand ExitCommand => new(
        obj => Application.Current.Shutdown(),
        obj => true
    );

    public RelayCommand SignAccountCommand => new (  
    obj => SignAccount(),
    obj => true
    );

    public RelayCommand CloseAccountCommand => new(
        obj => Application.Current.Shutdown(),
        obj => true
    );
    
    public RelayCommand ChangeAccountCommand => new( 
        obj => Application.Current.Shutdown(),
        obj => true
    );

    public void SignAccount() {
        var auth = new AuthorizationWindow();

        //TODO: Изменить после проверок!
        auth.Show();
    } //SignAccount

} //MainWindowViewModel
