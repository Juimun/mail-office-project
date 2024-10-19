using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using MailOffice.Infrastructure;
using MailOffice.Models.DataBase;
using MailOffice.Models.Reports;
using MailOffice.View;

namespace MailOffice.ViewModel.Authentication;

public class AuthorizationViewModel : INotifyPropertyChanged {

    public static string SourcePathFile = AppDomain.CurrentDomain.BaseDirectory;
    public static string FolderPath = Path.Combine(SourcePathFile, "Saves");
    public static string AccountsJsonPath = Path.Combine(FolderPath, "accounts.json"); 

    public AuthorizationWindow HostWindow { get; set; } 

    public AuthorizationViewModel(AuthorizationWindow hostWindow) {
        HostWindow = hostWindow;

        if (!Directory.Exists(FolderPath))
            Directory.CreateDirectory(FolderPath);

       
    }

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
    #endregion

    public void EntryAuthorization() { 
        var db = new DatabaseQueries();
        var users = db.GetAllUsers();

        var firstUser = users
            .Where(u => u.Authenticate(HostWindow.LoginTextBox.Text, HostWindow.PasswordTextBox.Text))
            .Select(u => new JsonSerialize(u.Login, u.Password))
            .ToList(); 
        

        if (!File.Exists(AccountsJsonPath)) {
            Utils.JsonSerialize(firstUser, AccountsJsonPath);
        }
        else
            Utils.JsonDeserialize(AccountsJsonPath);


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