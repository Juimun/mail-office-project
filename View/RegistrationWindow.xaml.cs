using MailOffice.ViewModel.Authentication;
using System.Windows;

namespace MailOffice.View;

/// <summary>
/// Логика взаимодействия для RegistrationWindow.xaml
/// </summary>
public partial class RegistrationWindow : Window {

    public RegistrationWindow() {
        InitializeComponent();

        DataContext = new RegistrationViewModel(this);
    }

} //RegistrationWindow