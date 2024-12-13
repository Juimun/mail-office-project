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
        set => SetField(ref _rememberMe, value);
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

    public void EntryAuthorization() {
        var data = new DatabaseQueries();

        UserJson savedUser;
        try
        {
            savedUser = data.GetUserJson(HostWindow.LoginTextBox.Text, HostWindow.EnteredPassword);
        }
        catch (Exception)
        {
            MessageBox.Show($"Аккаунт не найден! Попробуйте еще раз...", "Подсказка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (RememberMe)
        {
            Utils.JsonSerialize(savedUser, App.AccountsJsonPath);
        }
            
        
        HostWindow.DialogResult = true;
        HostWindow.Close();
    } //EntryAuthorization

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