using MailOffice.Infrastructure;
using MailOffice.View;

namespace MailOffice.ViewModel.Authentication;

public class ChangeAccountViewModel(ChangeAccountWindow hostWindow) {

    public RelayCommand AuthorizationCommand => new(
        obj => ShowAuthorization(),
        obj => true
    ); 

    public RelayCommand ExitCommand => new( 
        obj => hostWindow.Close(),
        obj => true
    );

    private void ShowAuthorization() {
        hostWindow.Close();

        var auth = new AuthorizationWindow();
        auth.ShowDialog();
    } //ShowAuthorization

} //ChangeAccountViewModel