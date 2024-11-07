using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using MailOffice.Infrastructure;
using MailOffice.View;
using MailOfficeDataBase.DataBase;
using MailOfficeTool.Entities;
using MailOfficeTool.Infrastructure;

namespace MailOffice.ViewModel.Authentication;

public class AuthorizationViewModel(AuthorizationWindow hostWindow) : INotifyPropertyChanged {

    public AuthorizationWindow HostWindow { get; set; } = hostWindow;

    private bool _rememberMe;
    public bool RememberMe {
        get => _rememberMe;
        set { 
            _rememberMe = value; 
            OnPropertyChanged();
        }
    }

    #region Команды
    public RelayCommand CanselCommand => new( 
        obj => HostWindow.Close(),
        obj => true
    );
    
    public RelayCommand EntryCommand => new(
    obj => EntryAuthorization(),
        obj => true
    );

    public RelayCommand RememberMeCommand => new(
        obj => RememberMe = (bool)obj,
        obj => true
    );

    public RelayCommand RegistrationCommand => new(
        obj => ShowRegistration(),
        obj => true
    );
    #endregion

    private void ShowRegistration() {
        HostWindow.Close();

        var regWindow = new RegistrationWindow();
        regWindow.ShowDialog();
    } //ShowRegistration

    private List<UserJson> _savedUsers = new();

    public void EntryAuthorization() { 
        if (RememberMe == true) {
            var data = new DatabaseQueries();

            UserJson savedUser;
            try {
                savedUser = data
                    .Query07(HostWindow.LoginTextBox.Text, HostWindow.PasswordTextBox.Text);
            }
            catch {
                MessageBox.Show("Аккаунт не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SaveUser(savedUser);
        }

        HostWindow.Close();
    } //EntryAuthorization

    private void SaveUser(UserJson savedUser) {
        // Перезапись в JSON для добавления нескольких аккаунтов
        if (File.Exists(App.AccountsJsonPath))
        {
            _savedUsers = Utils.JsonDeserialize(App.AccountsJsonPath);
        }

        // Проверка на дубликаты
        if (_savedUsers.Any(u => u.Login == savedUser.Login))
        {
            MessageBox.Show("Данный аккаунт уже сохранен!", "Подсказка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        // Проверка на ограничение количества 
        if (_savedUsers.Count >= 4)
        {
            MessageBox.Show("Вы сохранили максимальное количество аккаунтов!", "Подсказка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        _savedUsers.Add(savedUser);
        Utils.JsonSerialize(_savedUsers, App.AccountsJsonPath);
    } //SaveUser

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null) {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
    #endregion

} //AuthorizationViewModel