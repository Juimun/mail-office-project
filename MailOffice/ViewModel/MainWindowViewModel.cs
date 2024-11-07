using System.Windows;
using MailOffice.Infrastructure;
using MailOffice.View;
using MailOffice.View.Queries;
using MailOfficeControllers.Controllers;
using MailOfficeDataBase.DataBase;

namespace MailOffice.ViewModel;

public class MainWindowViewModel {

    // Контроллер для таблиц БД
    private DatabaseDisplayController _dataController;   

    public MainWindow HostWindow { get; set; }

    public MainWindowViewModel(
        MainWindow hostWindow, DatabaseDisplayController dataController)  
    { 
        (HostWindow, _dataController) = 
            (hostWindow, dataController);

        var data = new DatabaseQueries();
        HostWindow.DataGrid.ItemsSource = data.GetAllPeople();
    } // MainWindowViewModel

    #region MyRegion Команды

    public RelayCommand ExitCommand => new(
        obj => Application.Current.Shutdown(),
        obj => true
    );

    public RelayCommand SignAccountCommand => new(
        obj => SignAccount(),
        obj => true
    );

    public RelayCommand CloseAccountCommand => new(
        obj => Application.Current.Shutdown(),
        obj => true
    );

    public RelayCommand ChangeAccountCommand => new(
        obj => ChangeAccount(),
        obj => true
    );

    public RelayCommand LeftTabCommand => new(
        obj => LeftTab(),
        obj => true
    );

    public RelayCommand RightTabCommand => new(
        obj => RightTab(),
        obj => true
    );

    public RelayCommand Query1Command => new(
        obj => ShowQuery1(),
        obj => true
    );

    public RelayCommand Query2Command => new( 
        obj => ShowQuery2(),
        obj => true
    );

    public RelayCommand Query3Command => new(
        obj => ShowQuery3(),
        obj => true
    );

    public RelayCommand Query4Command => new(
        obj => ShowQuery4(),
        obj => true
    );

    public RelayCommand Query5Command => new(
        obj => ShowQuery5(),
        obj => true
    );

    public RelayCommand Query6Command => new(
        obj => ShowQuery6(),
        obj => true
    );
    #endregion

    // Вывод запроса № 1 
    private void ShowQuery1() {
        HostWindow.TbcMain.SelectedIndex = 1;
        HostWindow.TblQueries.Text = _dataController.ShowQuery1();
    } //ShowQuery1

    // Вывод запроса № 2 
    private void ShowQuery2() {
        var query2 = new Query2Window(); 
        query2.ShowDialog();

        if (query2.DialogResult == true) {
            HostWindow.TbcMain.SelectedIndex = 1;
            HostWindow.TblQueries.Text = _dataController.ShowQuery2(
                query2.StreetComboBox.Text,
                query2.HouseNumberComboBox.Text 
                );
        } //if
    } //ShowQuery2

    // Вывод запроса № 3 
    //Какие газеты выписывает гражданин с указанной фамилией, именем, отчеством?
    private void ShowQuery3() {
        var query3 = new Query3Window();
        query3.ShowDialog();

        if (query3.DialogResult == true) {
            // Делаем вкладку активной и выводим результат
            HostWindow.TbcMain.SelectedIndex = 1;
            HostWindow.TblQueries.Text = _dataController.ShowQuery3(
                query3.SecondNameTextBox.Text,
                query3.NameTextBox.Text,
                query3.PatronymicTextBox.Text
                );
        } //if
        
    } //ShowQuery3

    // Вывод запроса № 4 
    private void ShowQuery4()
    {
        HostWindow.TbcMain.SelectedIndex = 1;
        HostWindow.TblQueries.Text = _dataController.ShowQuery4();
    } //ShowQuery4

    // Вывод запроса № 5 
    private void ShowQuery5()
    {
        HostWindow.TbcMain.SelectedIndex = 1;
        HostWindow.TblQueries.Text = _dataController.ShowQuery5();
    } //ShowQuery5

    // Вывод запроса № 6 
    private void ShowQuery6() {
        HostWindow.TbcMain.SelectedIndex = 1;
        HostWindow.TblQueries.Text = _dataController.ShowQuery6();
    } //ShowQuery6

    private void LeftTab() {
        if (HostWindow.TbcMain.SelectedIndex > 0)
            HostWindow.TbcMain.SelectedIndex--;
        else
            HostWindow.TbcMain.SelectedIndex = HostWindow.TbcMain.Items.Count - 1;
    } //LeftTab

    private void RightTab() {
        if (HostWindow.TbcMain.SelectedIndex < HostWindow.TbcMain.Items.Count - 1)
            HostWindow.TbcMain.SelectedIndex++;
        else
            HostWindow.TbcMain.SelectedIndex = 0;
    } //RightTab

    // Вход в аккаунт
    public void SignAccount() {
        var auth = new AuthorizationWindow();
        auth.ShowDialog();
    } //SignAccount

    // Смена аккаунта
    public void ChangeAccount() {
        var change = new ChangeAccountWindow();
        change.ShowDialog();
    } //ChangeAccount

    // Выход из аккаунта
    public void CloseAccount() {
        
    } //CloseAccount

} //MainWindowViewModel
