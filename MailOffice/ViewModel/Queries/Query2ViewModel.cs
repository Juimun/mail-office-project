using MailOffice.Infrastructure;
using MailOffice.View.Queries;

namespace MailOffice.ViewModel.Queries;

public class Query2ViewModel(Query2Window hostWindow) 
{
    public RelayCommand EntryCommand => new(
        obj => hostWindow.DialogResult = true,
        obj => 
            hostWindow.StreetComboBox.SelectedItem != null && 
            hostWindow.HouseNumberComboBox.SelectedItem != null
    );

    public RelayCommand CanselCommand => new(
        obj => hostWindow.Close(),
        obj => true
    );
}