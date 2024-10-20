using System.IO;
using System.Windows;

namespace MailOffice;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application {

    public static string SourcePathFile = AppDomain.CurrentDomain.BaseDirectory;
    public static string FolderPath = Path.Combine(SourcePathFile, "Saves");
    public static string AccountsJsonPath = Path.Combine(FolderPath, "accounts.json");

    protected override void OnStartup(StartupEventArgs e) {
        base.OnStartup(e);

        if (!Directory.Exists(FolderPath))
            Directory.CreateDirectory(FolderPath);
    }
} //App