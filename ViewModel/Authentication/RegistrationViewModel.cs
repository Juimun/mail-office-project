using MailOffice.Infrastructure;
using MailOffice.View;


namespace MailOffice.ViewModel.Authentication;

public class RegistrationViewModel(RegistrationWindow hostWindow) {

    public RegistrationWindow HostWindow { get; set; } = hostWindow;

    public RelayCommand CanselCommand => new(
        obj => HostWindow.Close(),
        obj => true
    ); 

    public RelayCommand AuthorizationCommand => new (
        obj => ShowAuthorization(),
        obj => true
    );

    private void ShowAuthorization() { 
        HostWindow.Close();
         
        var authWindow = new AuthorizationWindow();
        authWindow.ShowDialog();
    } //ShowRegistration

    private void AddUser() {
        
    } //AddUser
} //RegistrationViewModel