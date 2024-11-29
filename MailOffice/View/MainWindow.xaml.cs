using System.Windows;
using System.Windows.Controls;
using MailOffice.ViewModel;
using MailOfficeControllers.Controllers;
using MailOfficeDataBase.DataBase;

namespace MailOffice.View;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window {

    private DatabaseDisplayController _dataController;
    private DatabaseQueries _dataQueries; 

    public MainWindow() {
        InitializeComponent();

        _dataController = new DatabaseDisplayController();
        _dataQueries = new DatabaseQueries();

        DataContext = new MainWindowViewModel(this, _dataController, _dataQueries);

    }

    
} //MainWindow