using MailOfficeDataBase.DataBase;
using MailOfficeTool.Infrastructure;
using System.IO;
using System.Windows;

namespace MailOffice;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application {

    public static string SourceFilePath = AppDomain.CurrentDomain.BaseDirectory;

    // Пути к папкам
    public static string SavesFolderPath = Path.Combine(SourceFilePath, "Saves");
    public static string UserFolderPath = Path.Combine(SavesFolderPath, "User");
    public static string DocumentsFolderPath = Path.Combine(SavesFolderPath, "Documents");
    public static string ReportsFolderPath = Path.Combine(DocumentsFolderPath, "Reports");
    public static string StatementsFolderPath = Path.Combine(DocumentsFolderPath, "Statements");

    // Пути к файлам
    public static string AccountsJsonPath = Path.Combine(UserFolderPath, "accounts.json");
    public static string AccountsTxtPath = Path.Combine(SavesFolderPath, "accounts.txt");

    // Пути к MailOfficeDataSeeder.exe
    public static string DataSeederPath = GetDataSeederPath(); 

    // Стандартные пути к аватаркам
    public static string AvatarsImagePath = GetSelectedImagePath($"Assets\\Menu\\Avatars\\{Utils.GetRandom(1, 15)}.png");
    public static string DeafaultAvatarImagePath = GetSelectedImagePath($"Assets\\User\\user.png");  

    protected override void OnStartup(StartupEventArgs e) {
        base.OnStartup(e);

        // Создание папки для сохранений
        if (!Directory.Exists(SavesFolderPath)) 
            Directory.CreateDirectory(SavesFolderPath);

        // Создание папки для пользователя
        if (!Directory.Exists(UserFolderPath))
            Directory.CreateDirectory(UserFolderPath);

        // Создание подпапки для документов
        if (!Directory.Exists(DocumentsFolderPath))
            Directory.CreateDirectory(DocumentsFolderPath);

        // Создание подпапки для отчетов  
        if (!Directory.Exists(ReportsFolderPath))
            Directory.CreateDirectory(ReportsFolderPath);

        // Создание подпапки для справок
        if (!Directory.Exists(StatementsFolderPath))
            Directory.CreateDirectory(StatementsFolderPath);

        // !!! Исключительно для тестов приложения !!!
        Utils.SaveAsTxt(new DatabaseQueries().GetAllAccountAuthorization(), AccountsTxtPath,
            "   Логин аккаунта   |   Пароль аккаунта   |   Роль персонала\n");
        MessageBox.Show(AvatarsImagePath);
    } //OnStartup

    // Создание пути к файлу MailOfiiceDataSeeder.exe
    private static string GetDataSeederPath() {
        var dataSeederPath = SourceFilePath;
        for (var cnt = 0; cnt < 5; cnt++)
            dataSeederPath = Path.GetDirectoryName(dataSeederPath);

        return Path.Combine(dataSeederPath!, @"MailOfiiceDataSeeder\bin\Debug\net8.0\MailOfiiceDataSeeder.exe");
    } //DataSeederPath

    // Создание пути к нужной папке MailOffice
    private static string GetSelectedImagePath(string path) { 
        var avatarsImagePath = SourceFilePath;
        for (var cnt = 0; cnt < 4; cnt++)
            avatarsImagePath = Path.GetDirectoryName(avatarsImagePath);

        return Path.Combine(avatarsImagePath!, path);
    } //AvatarsImagePath
} //App