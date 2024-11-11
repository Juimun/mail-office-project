using System.Diagnostics;
using System.IO;
using System.Windows;

namespace MailOffice;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application {

    public static string SourceFilePath = AppDomain.CurrentDomain.BaseDirectory;
    public static string FolderPath = Path.Combine(SourceFilePath, "Saves");
    public static string AccountsJsonPath = Path.Combine(FolderPath, "accounts.json");

    protected override void OnStartup(StartupEventArgs e) {
        base.OnStartup(e);

        if (!Directory.Exists(FolderPath))
            Directory.CreateDirectory(FolderPath);

        // Запуск и ожидание работы MailOfiiceDataSeeder
        Process.Start(DataSeederPath()).WaitForExit();
    }

    // Создание пути к файлу MailOfiiceDataSeeder.exe
    private string DataSeederPath() {
        var dataSeederPath = SourceFilePath;
        for (var cnt = 0; cnt < 5; cnt++)
            dataSeederPath = Path.GetDirectoryName(dataSeederPath);

        return Path.Combine(dataSeederPath!, @"MailOfiiceDataSeeder\bin\Debug\net8.0\MailOfiiceDataSeeder.exe");
    } //DataSeederPath
} //App