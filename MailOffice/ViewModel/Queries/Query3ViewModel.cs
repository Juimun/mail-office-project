using MailOffice.Infrastructure;
using MailOffice.View.Queries;

namespace MailOffice.ViewModel.Queries;

public class Query3ViewModel(Query3Window hostWindow) {

    public RelayCommand EntryCommand => new( 
        obj => hostWindow.DialogResult = true,
        obj => 
            !string.IsNullOrWhiteSpace(hostWindow.NameTextBox.Text)       && hostWindow.NameTextBox.Text != "Введите имя" &&
            !string.IsNullOrWhiteSpace(hostWindow.SecondNameTextBox.Text) && hostWindow.SecondNameTextBox.Text != "Введите фамилию" &&
            !string.IsNullOrWhiteSpace(hostWindow.PatronymicTextBox.Text) && hostWindow.PatronymicTextBox.Text != "Введите отчество"
    );

    public RelayCommand CanselCommand => new( 
        obj => hostWindow.Close(),
        obj => true
    );

} //Query3ViewModel