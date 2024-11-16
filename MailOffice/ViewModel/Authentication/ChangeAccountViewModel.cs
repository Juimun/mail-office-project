using MailOffice.Infrastructure;
using MailOffice.View;
using MailOfficeTool.Infrastructure;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MailOfficeTool.Entities;

namespace MailOffice.ViewModel.Authentication;

public class ChangeAccountViewModel(ChangeAccountWindow hostWindow) : INotifyPropertyChanged {

    private ObservableCollection<UserJson> _savedAccounts;

    public ObservableCollection<UserJson> SavedAccounts { 
        get => _savedAccounts;
        set => SetField(ref _savedAccounts, value);
    }

    public RelayCommand AuthorizationCommand => new(
        obj => ShowAuthorization(),
        obj => true
    ); 

    public RelayCommand ExitCommand => new( 
        obj => hostWindow.Close(),
        obj => true
    ); 

    public RelayCommand LoadAccountCommand => new( 
        obj => SavedAccounts = new ObservableCollection<UserJson>(Utils.JsonDeserialize(App.AccountsJsonPath)),
        obj => true
    );

    private void ShowAuthorization() {
        hostWindow.Close();

        var auth = new AuthorizationWindow();
        auth.ShowDialog();
    } //ShowAuthorization

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

} //ChangeAccountViewModel