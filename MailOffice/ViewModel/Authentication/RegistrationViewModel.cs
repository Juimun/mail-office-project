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
    private void EntryRegistration() {

        // Проверки для регистрации
        if (ValidateRegistrationData()) {
            var data = new DatabaseQueries();
            data.AddRegisteredUser(HostWindow.LoginTextBox.Text,
                    Utils.GetBytes(HostWindow.PasswordTextBox.Text));

            // !!! Исключительно для тестов приложения !!!
            Utils.SaveAsTxt(new DatabaseQueries().GetAllAccountAuthorization(), App.AccountsTxtPath,
                "   Логин аккаунта   |   Пароль аккаунта   |   Роль персонала\n");

            HostWindow.Close();
        } //if
    } //EntryRegistration

    // Виды проверок для регистрации
    private bool ValidateRegistrationData() {
       
        // Проверка на пустые строки и пробелы
        if (string.IsNullOrEmpty(HostWindow.LoginTextBox.Text) && string.IsNullOrEmpty(HostWindow.PasswordTextBox.Text)) {
            ShowErrorMessage("Введены некорректные данные!");
            return false;
        } //if

        // Проверка логина на повтор
        var data = new DatabaseQueries();
        if (data.LoginExist(HostWindow.LoginTextBox.Text)) {
            ShowErrorMessage("Логин уже существует!");
            return false;
        } //if

        // Логин должен быть НЕ МЕНЕЕ 5 и НЕ БОЛЕЕ 16 символов
        if (!Utils.IsValidStringLength(HostWindow.LoginTextBox.Text, 5, 16)) {
            ShowErrorMessage("Некорректный размер логина!");
            return false;
        } //if

        // Логин должен содержать в первом символе букву, а затем можно цифры и буквы английского языка
        if (!Utils.IsValidLogin(HostWindow.LoginTextBox.Text)) {
            ShowErrorMessage("Логин не должен содержать невалидные символы или начинаться не с буквы!");
            return false;
        }

        // Пароль должен быть НЕ МЕНЕЕ 8 символов
        if (!Utils.IsValidStringLength(HostWindow.PasswordTextBox.Text, 8, 30)) {
            ShowErrorMessage("Некорректный размер пароля!");
            return false;
        } //if

        // Пароль должен содержать только доступные символы, цифры и буквы английского языка
        if (!Utils.IsValidPassword(HostWindow.PasswordTextBox.Text)) {
            ShowErrorMessage("Пароль не должен содержать невалидные символы!");
            return false;
        } //if

        // Совпадение паролей
        if (HostWindow.PasswordTextBox.Text != HostWindow.PasswordRepeatTextBox.Text)  {
            ShowErrorMessage("Пароли не совпадают!");
            return false;
        } //if

        return true;
    } //ValidateRegistrationData

    private static void ShowErrorMessage(string message) =>
        MessageBox.Show(message, "Подсказка", MessageBoxButton.OK, MessageBoxImage.Warning);
    
} //RegistrationViewModel