using MailOffice.Controllers;
using MailOffice.Infrastructure;
using MailOffice.View;
using System.Text;
using System.Windows;

namespace MailOffice.ViewModel;

public class MainWindowViewModel {

    // Контроллеры для визуализации
    private readonly DataDisplayController _dataController;
    private readonly QueryDisplayController _queryController;  

    public MainWindow HostWindow { get; set; }

    public MainWindowViewModel(
        MainWindow hostWindow, DataDisplayController dataController, QueryDisplayController queryController) 
    { 
        (HostWindow, _dataController, _queryController) = 
            (hostWindow, dataController, queryController);

        var sb = new StringBuilder("\n");
        sb.AppendLine($"{_dataController.ShowAllTables()}\n")
          .AppendLine($"Запрос №1\nОпределить наименование и количество экземпляров всех изданий\n{_queryController.ShowQuery1()}\n\n")
          .AppendLine($"Запрос №2\nПо заданному адресу определить фамилию почтальона, обслуживающего подписчика\n{_queryController.ShowQuery2("ул.Артема")}\n\n")

          .AppendLine($"Запрос №4\nСколько почтальонов работает в почтовом отделении\n{_queryController.ShowQuery4()}\n\n")

          .AppendLine($"Запрос №6\nСколько почтальонов работает в почтовом отделении\n{_queryController.ShowQuery6()}\n\n"); ;

        HostWindow.TblTables.Text = sb.ToString();
    } // MainWindowViewModel

    public RelayCommand ExitCommand => new(
        obj => Application.Current.Shutdown(),
        obj => true
    );

} //MainWindowViewModel
