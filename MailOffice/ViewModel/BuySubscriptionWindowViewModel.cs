using MailOffice.View;
using MailOfficeControllers.Controllers;
using MailOfficeDataBase.DataBase;
using MailOfficeEntities.Entities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Xml;
using System.Text;

namespace MailOffice.ViewModel;

public class BuySubscriptionWindowViewModel : INotifyPropertyChanged {

    private BuySubscriptionWindow HostWindow;
    private DatabaseQueries _databaseQueries; 

    public BuySubscriptionWindowViewModel(
       BuySubscriptionWindow hostWindow, DatabaseQueries databaseQueries)
    {
        (HostWindow, _databaseQueries) =
            (hostWindow, databaseQueries);

        // Список публикаций для оформления подписки 
        Publications = new ObservableCollection<Publication>(_databaseQueries.GetAllPublications());
        HostWindow.PublicationDataGrid.ItemsSource = Publications;
    } //SpecialMenuWindowViewModel

    // Список публикаций 
    private ObservableCollection<Publication> _publications;  
    public ObservableCollection<Publication> Publications
    {
        get => _publications;
        set => SetField(ref _publications, value);
    }

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

} //BuySubscriptionWindowViewModel
