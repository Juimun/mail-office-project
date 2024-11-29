using MailOffice.ViewModel;
using MailOfficeControllers.Controllers;
using MailOfficeDataBase.DataBase;
using System.Windows;

namespace MailOffice.View;

/// <summary>
/// Логика взаимодействия для ReceiptWindow.xaml
/// </summary>
public partial class ReceiptWindow : Window {
    public ReceiptWindow(MainWindowViewModel mainWindowViewModel) {
        InitializeComponent();

        DataContext = new ReceiptWindowViewModel(this, new DatabaseQueries(), mainWindowViewModel);
    }
} //ReceiptWindow
