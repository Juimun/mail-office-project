using MailOffice.Infrastructure;
using MailOffice.View.Queries;

namespace MailOffice.ViewModel.Queries;

public class Query3ViewModel(Query3Window hostWindow) {

    public RelayCommand EntryCommand => new( 
        obj => hostWindow.DialogResult = true,
        obj => 
            !string.IsNullOrWhiteSpace(hostWindow.NameComboBox.Text)       && hostWindow.NameComboBox.Text != "Введите имя" &&
            !string.IsNullOrWhiteSpace(hostWindow.SecondNameComboBox.Text) && hostWindow.SecondNameComboBox.Text != "Введите фамилию" &&
            !string.IsNullOrWhiteSpace(hostWindow.PatronymicComboBox.Text) && hostWindow.PatronymicComboBox.Text != "Введите отчество"
    );

    public RelayCommand CanselCommand => new( 
        obj => hostWindow.Close(),
        obj => true
    );

} //Query3ViewModel