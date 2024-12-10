﻿using MailOffice.Infrastructure;
using MailOffice.View;
using MailOfficeDataBase.DataBase;
using MailOfficeEntities.Category;
using MailOfficeEntities.Entities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MailOffice.ViewModel;

public class BuySubscriptionWindowViewModel : INotifyPropertyChanged {

    public BuySubscriptionWindow HostWindow;
    private DatabaseQueries _databaseQueries;
    public MainWindowViewModel _mainWindowViewModel;

    public BuySubscriptionWindowViewModel(
       BuySubscriptionWindow hostWindow, DatabaseQueries databaseQueries, MainWindowViewModel mainWindowViewModel)
    {
        (HostWindow, _databaseQueries, _mainWindowViewModel) =
            (hostWindow, databaseQueries, mainWindowViewModel);

        // Список публикаций для оформления подписки 
        Publications = new ObservableCollection<Publication>(_databaseQueries.GetAllPublications());
        HostWindow.PublicationDataGrid.ItemsSource = Publications;
    } //SpecialMenuWindowViewModel

    public RelayCommand ConfirmSubscriptionCommand => new(
       obj => ConfirmSubscription(),
       obj => SelectedPublications != null
    );

    // Список публикаций 
    private ObservableCollection<Publication> _publications;  
    public ObservableCollection<Publication> Publications {
        get => _publications;
        set => SetField(ref _publications, value);
    }

    // Список выбранных публикаций
    private List<Publication> _selectedPublications; 
    public List<Publication> SelectedPublications 
    {
        get => _selectedPublications;
        set => SetField(ref _selectedPublications, value);
    }

    public SubscriptionPeriod SelectedSubscriptionPeriod;

    // Оформление подписки
    private void ConfirmSubscription() {

        // Защита от повторного нажатия
        HostWindow.ConfirmSubscription.IsEnabled

             // Отключаем DataGrid
             = HostWindow.PublicationDataGrid.IsEnabled = false;

        // Выбранные публикации 
        SelectedPublications = HostWindow
            .PublicationDataGrid
            .SelectedItems
            .OfType<Publication>()
            .ToList();

        // Выбранный срок
        SelectedSubscriptionPeriod = Enum
            .Parse<SubscriptionPeriod>(
            (string)HostWindow
                .SubscriptionPeriodComboBox
                .SelectedItem
            );

        var addInformation = new AdditionalInformationWindow(this);
        addInformation.ShowDialog();
    } //ConfirmSubscription

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
