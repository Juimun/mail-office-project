using MailOffice.View;

namespace MailOffice.ViewModel.Authentication;

public class RegistrationViewModel {

    public RegistrationWindow HostWindow { get; set; }

    public RegistrationViewModel(RegistrationWindow hostWindow) {
        HostWindow = hostWindow;
    }

} //RegistrationViewModel