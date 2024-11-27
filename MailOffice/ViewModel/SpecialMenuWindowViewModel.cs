using MailOffice.Infrastructure;
using MailOffice.View;
using MailOfficeDataBase.DataBase;
using MailOfficeEntities.Category;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace MailOffice.ViewModel;

public class SpecialMenuWindowViewModel(SpecialMenuWindow hostWindow, DatabaseQueries databaseQueries, MainWindowViewModel mainWindowViewModel) {

    private SpecialMenuWindow HostWindow { get; set; } = hostWindow;
    private DatabaseQueries DatabaseQueries { get; set; } = databaseQueries;
    private MainWindowViewModel MainWindowViewModel { get; set; } = mainWindowViewModel; 

    public RelayCommand ExitCommand => new(
       obj => HostWindow.Close(),
       obj => true
    );

    public RelayCommand AddCommand => new( 
       obj => AddPostman(),
       obj => true
    );

    public RelayCommand RemoveCommand => new(
       obj => RemovePostman(),
       obj => true
    );

    public RelayCommand RegenerateDbCommand => new(
       obj => RegenerateDadabase(),
       obj => true
    );

    // Принять на работу Почтальена
    // TODO: Переделать хардкод!
    private void AddPostman() {
        if (int.TryParse(HostWindow.PersonIdTextBox.Text, out int personId)) {
            if (DatabaseQueries.AddPostman(personId)) {
                MessageBox.Show($"Пользователь с Id {personId}, был повышен до: {StaffRole.Postman}!", 
                    "Удачно", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            } 
            else {
                MessageBox.Show("Пользователь уже назначен на должность!", 
                    "Подсказка", MessageBoxButton.OK, MessageBoxImage.Warning);
                HostWindow.PersonIdTextBox.Focus();
            } //if
        } 
        else {
            MessageBox.Show("Некорректный Id пользователя! Введите целое число.", 
                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            HostWindow.PersonIdTextBox.Focus();
        } //if
    } //AddPostman

    // Уволить почтальона
    // TODO: Переделать хардкод!
    private void RemovePostman() {
        if (int.TryParse(HostWindow.PostmanIdTextBox.Text, out int postmanId)) {
            if (DatabaseQueries.RemovePostman(postmanId)) { 
                MessageBox.Show($"Почтальен с Id {postmanId}, был уволен!",
                    "Удачно", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else {
                MessageBox.Show($"Почтальен с Id {postmanId}, не был уволен!",
                    "Подсказка", MessageBoxButton.OK, MessageBoxImage.Warning);
                HostWindow.PostmanIdTextBox.Focus();
            } //if
        }
        else {
            MessageBox.Show("Некорректный Id почтальена! Введите целое число.",
                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            HostWindow.PostmanIdTextBox.Focus();
        } //if
    } //RemovePostman

    private void RegenerateDadabase() { 
        var messageBoxResult = MessageBox.Show("Вы уверены, что действительно хотите перегенерировать тестовые данные?",
                "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning); 

        // При подтверждении - перегенерация
        if (messageBoxResult == MessageBoxResult.OK) {
            MainWindowViewModel.GenerateTextEntities();
        } //if
    } //RegenerateDbCommand

} //SpecialMenuWindowViewModel

