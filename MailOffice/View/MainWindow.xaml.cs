using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using MailOffice.ViewModel;
using MailOfficeControllers.Controllers;
using MailOfficeDataBase.DataBase;
using Microsoft.Win32;

namespace MailOffice.View;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window {

    private DatabaseDisplayController _dataController;
    private DatabaseQueries _dataQueries;
    private MainWindowViewModel mainWindowViewModel;

    public MainWindow() {
        InitializeComponent();

        _dataController = new DatabaseDisplayController();
        _dataQueries = new DatabaseQueries();

        mainWindowViewModel = new MainWindowViewModel(this, _dataController, _dataQueries);
        DataContext = mainWindowViewModel;

    }

    private void MainProfileImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
        mainWindowViewModel.ChangeImage();
    } //MainProfileImage_MouseLeftButtonDown

} //MainWindow