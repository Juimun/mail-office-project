using System.Windows;
using MailOffice.Infrastructure;
using MailOffice.View;
using MailOfficeDataBase.DataBase;
using MailOfficeTool.Infrastructure;

namespace MailOffice.ViewModel.Authentication;

public class RegistrationViewModel(RegistrationWindow hostWindow) {

    private RegistrationWindow HostWindow { get; set; } = hostWindow;

    public RelayCommand CanselCommand => new(
        obj => HostWindow.Close(),
        obj => true
    ); 

    public RelayCommand AuthorizationCommand => new (
        obj => ShowAuthorization(),
        obj => true
    );

    public RelayCommand EntryRegistrationCommand => new( 
        obj => EntryRegistration(),
        obj => true
    );

    private void ShowAuthorization() { 
        HostWindow.Close();

        var authWindow = new AuthorizationWindow();
        authWindow.ShowDialog();
    } //ShowRegistration

    // Создание нового пользователя 
    //TODO: Пока хардкод - Переделать!
    private void EntryRegistration() {

        // Проверка на пустые строки и пробелы
        if (string.IsNullOrEmpty(HostWindow.LoginTextBox.Text) && string.IsNullOrEmpty(HostWindow.PasswordTextBox.Text)) {
            MessageBox.Show("Введены некорректные данные!", "Подсказка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        } //if

        // Проверка логина на повтор
        var data = new DatabaseQueries();
        if (data.LoginExist(HostWindow.LoginTextBox.Text)) { 
            MessageBox.Show("Неверный логин для регистрации!", "Подсказка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        } //if

        // Логин должен быть НЕ МЕНЕЕ 5 символов
        if (HostWindow.LoginTextBox.Text.Length < 5)
        {
            MessageBox.Show("Логин должен быть НЕ МЕНЕЕ 5 символов!", "Подсказка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        } //if

        // Пароль должен быть НЕ МЕНЕЕ 8 символов
        if (HostWindow.PasswordTextBox.Text.Length < 8) {
            MessageBox.Show("Пароль должен быть НЕ МЕНЕЕ 8 символов!", "Подсказка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        } //if

        if (HostWindow.PasswordTextBox.Text == HostWindow.PasswordRepeatTextBox.Text) {
            // Добавление новой записи в БД
            data.AddRegisteredUser(HostWindow.LoginTextBox.Text,
                Utils.GetBytes(HostWindow.PasswordTextBox.Text));

            HostWindow.Close();
        } //if
        else {
            MessageBox.Show("Пароли НЕ совпадают", "Подсказка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    } //EntryRegistration

} //RegistrationViewModel