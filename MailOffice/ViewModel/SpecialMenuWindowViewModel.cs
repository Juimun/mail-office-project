using MailOffice.Infrastructure;
using MailOffice.View;

namespace MailOffice.ViewModel;

public class SpecialMenuWindowViewModel(SpecialMenuWindow hostWindow) {

    private SpecialMenuWindow HostWindow { get; set; } = hostWindow;

    public RelayCommand ExitCommand => new(
       obj => HostWindow.Close(),
       obj => true
   );

} //SpecialMenuWindowViewModel

