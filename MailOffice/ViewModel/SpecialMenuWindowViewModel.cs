using MailOffice.Infrastructure;
using MailOffice.View;
using MailOfficeDataBase.DataBase;
using MailOfficeEntities.Category;
using MailOfficeEntities.Entities;
using MailOfficeEntities.Entities.Accounts;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;

namespace MailOffice.ViewModel;

public class SpecialMenuWindowViewModel : INotifyPropertyChanged {

    private SpecialMenuWindow HostWindow { get; set; } 
    private DatabaseQueries DatabaseQueries { get; set; } 
    private MainWindowViewModel MainWindowViewModel { get; set; } 

    public SpecialMenuWindowViewModel(
        SpecialMenuWindow hostWindow, DatabaseQueries databaseQueries, MainWindowViewModel mainWindowViewModel) 
    {
        (HostWindow, DatabaseQueries, MainWindowViewModel) =
            (hostWindow, databaseQueries, mainWindowViewModel);

        // Список подписных изданий на ожидании 
        Entities = new ObservableCollection<Subscription>(DatabaseQueries.GetAllAwaitingSubscription());
        HostWindow.SelectedAwaitingDataGrid.ItemsSource = Entities;

        // Список почтальенов
        Postmans = new ObservableCollection<Staff>(DatabaseQueries.GetAllPostmans());
        HostWindow.PostmansDataGrid.ItemsSource = Postmans;

        // Список пользователей, которых можно повысить до Postman
        People = new ObservableCollection<Person>(DatabaseQueries.GetAllPersonWithoutStaff());
        HostWindow.PeopleDataGrid.ItemsSource = People;
    } //SpecialMenuWindowViewModel

    #region Команды
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

    public RelayCommand ConfirmSubscriptionCommand => new( 
       obj => ConfirmSubscription(),
       obj => true
    ); 

    public RelayCommand RejectSubscriptionCommand => new( 
       obj => RejectSubscription(),
       obj => true
    );
    #endregion 

    #region Director
    // Список пользователей, которых можно повысить до Postman
    private ObservableCollection<Person> _people;
    public ObservableCollection<Person> People { 
        get => _people;
        set => SetField(ref _people, value);
    }

    // Список почтальенов
    private ObservableCollection<Staff> _postmans; 
    public ObservableCollection<Staff> Postmans {  
        get => _postmans;
        set => SetField(ref _postmans, value);
    }

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
        } //if
    } //RemovePostman
    #endregion

    #region Admin
    // Перегенерация БД
    private void RegenerateDadabase() { 
        var messageBoxResult = MessageBox.Show("Вы уверены, что действительно хотите перегенерировать тестовые данные?",
                "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning); 

        // При подтверждении - перегенерация
        if (messageBoxResult == MessageBoxResult.OK) {
            MainWindowViewModel.GenerateTextEntities();
            HostWindow.Close();
        } //if
    } //RegenerateDbCommand
    #endregion

    #region Operator
    // Список подписных изданий на ожидании
    private ObservableCollection<Subscription> _entities;
    public ObservableCollection<Subscription> Entities {
        get => _entities;
        set => SetField(ref _entities, value);
    }

    // Подтвердить подписку
    private async void ConfirmSubscription() {

        // Список выделенных сущностей
        var selectedSubscription = HostWindow
            .SelectedAwaitingDataGrid
            .SelectedItems
            .OfType<Subscription>()
            .ToList(); 

        if (selectedSubscription.Count > 0) {

            // Защита от повторного нажатия
            HostWindow.ConfirmSubscription.IsEnabled 
                = HostWindow.RejectSubscription.IsEnabled

            // Отключаем DataGrid
                 = HostWindow.SelectedAwaitingDataGrid.IsEnabled = false;

            // Очищаем выделение
            HostWindow.SelectedAwaitingDataGrid.SelectedItems.Clear();

            // Создание списка задач 
            var tasks = selectedSubscription.Select(async subscription => {
                using (var db = new MailOfficeContext()) {

                    // Обновление статуса в БД
                    await DatabaseQueries.UpdateSubscriptionStatusAsync(subscription, SubscriptionStatus.Сonfirmed, db);
                }
            });

            // Ожидаем завершения
            await Task.WhenAll(tasks);

            // Удаляем сущности с измененнным статусом
            // DispatcherPriority запланирует удаление в фоновом потоке, чтобы избежать блокировки UI
            await HostWindow.Dispatcher.BeginInvoke(DispatcherPriority.Background, () => {
                selectedSubscription.ForEach(s => Entities.Remove(s));
            });

            // Включаем все обратно
            HostWindow.ConfirmSubscription.IsEnabled
                 = HostWindow.RejectSubscription.IsEnabled
                 = HostWindow.SelectedAwaitingDataGrid.IsEnabled = true;
        } //if      
    } //ConfirmSubscription

    // Отклонить подписку
    private async void RejectSubscription() {

        // Список выделенных сущностей
        var selectedSubscription = HostWindow
            .SelectedAwaitingDataGrid
            .SelectedItems
            .OfType<Subscription>()
            .ToList();

        if (selectedSubscription.Count > 0) {

            // Защита от повторного нажатия
            HostWindow.ConfirmSubscription.IsEnabled
                 = HostWindow.RejectSubscription.IsEnabled 

                 // Отключаем DataGrid
                 = HostWindow.SelectedAwaitingDataGrid.IsEnabled = false;

            // Очищаем выделение
            HostWindow.SelectedAwaitingDataGrid.SelectedItems.Clear();

            // Создание списка задач 
            var tasks = selectedSubscription.Select(async subscription => {
                using (var db = new MailOfficeContext()) {

                    // Обновление статуса в БД
                    await DatabaseQueries.UpdateSubscriptionStatusAsync(subscription, SubscriptionStatus.Rejected, db);
                }
            });

            // Ожидаем завершения
            await Task.WhenAll(tasks);

            // Удаляем сущности с измененнным статусом
            // DispatcherPriority запланирует удаление в фоновом потоке, чтобы избежать блокировки UI
            await HostWindow.Dispatcher.BeginInvoke(DispatcherPriority.Background, () => {
                selectedSubscription.ForEach(s => Entities.Remove(s));
            });

            // Включаем все обратно
            HostWindow.ConfirmSubscription.IsEnabled
                 = HostWindow.RejectSubscription.IsEnabled 
                 = HostWindow.SelectedAwaitingDataGrid.IsEnabled = true;
        } //if      
    } //RejectSubscription
    #endregion

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
    #endregion

} //SpecialMenuWindowViewModel

