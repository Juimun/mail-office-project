using MailOffice.View;
using MailOfficeDataBase.DataBase;
using MailOfficeEntities.Entities;
using MailOfficeEntities.Entities.Receipts;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MailOffice.ViewModel;

// Оформление подписки связано с выдачей клиенту квитанции,
//  в которой указывается общая стоимость подписки, что выписано, и на какой срок
class ReceiptWindowViewModel : INotifyPropertyChanged {

    private ReceiptWindow HostWindow;
    private DatabaseQueries DatabaseQueries;
    private MainWindowViewModel MainWindowViewModel;

    public ReceiptWindowViewModel(
        ReceiptWindow hostWindow, DatabaseQueries databaseQueries, MainWindowViewModel mainWindowViewModel) 
    {
        (HostWindow, DatabaseQueries, MainWindowViewModel) =
            (hostWindow, databaseQueries, mainWindowViewModel);

        // Список активных подписных изданий пользователя
        Subscriptions = new ObservableCollection<Subscription>(DatabaseQueries.GetAllActiveSubscription(
            MainWindowViewModel.CurrentAccount!.Login, MainWindowViewModel.CurrentAccount.Password));
        HostWindow.AllActiveSubscription.ItemsSource = Subscriptions;

        // Список квитанций пользователя
        Receipts = new ObservableCollection<ReceiptWithDetail>(DatabaseQueries.GetAllReceiptsWithDetails(
            MainWindowViewModel.CurrentAccount!.Login, MainWindowViewModel.CurrentAccount.Password));
        HostWindow.AllСonfirmedSubscription.ItemsSource = Receipts;
    } //SpecialMenuWindowViewModel

    // Список активных подписок пользователя
    private ObservableCollection<Subscription> _subscriptions;  
    public ObservableCollection<Subscription> Subscriptions { 
        get => _subscriptions;
        set => SetField(ref _subscriptions, value);
    }

    // Список квитанций пользователя
    private ObservableCollection<ReceiptWithDetail> _receipts; 
    public ObservableCollection<ReceiptWithDetail> Receipts {
        get => _receipts;
        set => SetField(ref _receipts, value);
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

} //ReceiptWindowViewModel

