using System.Windows;
using System.Windows.Controls;
using System.Windows.Shell;
using MailOffice.ViewModel.Authentication;

namespace MailOffice.View;

/// <summary>
/// Логика взаимодействия для Authorization.xaml
/// </summary>
public partial class AuthorizationWindow : Window {

    public AuthorizationWindow() {
        InitializeComponent();

        DataContext = new AuthorizationViewModel(this);
    }

    private void TextBox_OnGotFocus(object sender, RoutedEventArgs e) {
        var textBox = (TextBox)sender;
        if (textBox.Text is "Введите логин" or "Введите пароль")
            textBox.Text = string.Empty;
    } //TextBox_OnGotFocus

    private void TextBox_OnLostFocus(object sender, RoutedEventArgs e) {
        var textBox = (TextBox)sender;
        if (string.IsNullOrWhiteSpace(textBox.Text)) 
            textBox.Text = textBox.Name switch {
                "PasswordTextBox" => "Введите пароль",
                "LoginTextBox" => "Введите логин",
                _ => textBox.Text
            };
    } //TextBox_OnLostFocus

} //AuthorizationWindow