using System.Text;
using System.Windows;
using MailOffice.Controllers;
using MailOffice.Models;
using MailOffice.Models.DataBase;
using MailOffice.ViewModel;

namespace MailOffice.View;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window {

    private DataDisplayController _dataController;
    private QueryDisplayController _queryController;

    public MainWindow() {
        InitializeComponent();

        (_dataController, _queryController) = (new(), new());

        DataContext = new MainWindowViewModel(this, _dataController, _queryController);
    } 

} //MainWindow