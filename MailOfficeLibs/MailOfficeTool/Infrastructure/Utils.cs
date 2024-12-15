using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text;
using MailOfficeTool.Entities;
using Newtonsoft.Json;

namespace MailOfficeTool.Infrastructure;

public static partial class Utils
{

    public static int GetRandom(int valueMin, int valueMax) => Random
        .Shared
        .Next(valueMin, valueMax + 1);

    public static double GetRandom(double valueMin, double valueMax) => Random
        .Shared
        .NextDouble() * (valueMin + (valueMax - valueMin));

    public static string GetString(byte[] arr) =>
        Encoding.UTF8.GetString(arr);

    public static byte[] GetBytes(string arr) =>
    Encoding.UTF8.GetBytes(arr);

    public static void JsonSerialize(UserJson dataList, string fileName) =>
        File.WriteAllText(fileName, JsonConvert.SerializeObject(dataList, Formatting.Indented), Encoding.UTF8);

    public static UserJson JsonDeserialize(string fileName) =>
        JsonConvert.DeserializeObject<UserJson>(File.ReadAllText(fileName, Encoding.UTF8))!;

    // Создание файла .pdf
    // Нужен NuGet пакет iTextSharp 
    public static void SaveAsPdf(List<Paragraph> text, string filePath) {
        if (string.IsNullOrEmpty(filePath) || text.Count == 0) return;

        using (var document = new Document()) {
            using (var writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create))) {
                writer.SetPdfVersion(PdfWriter.PDF_VERSION_1_7);

                document.Open();                
                text.ForEach(p => document.Add(p));
                document.Close();
            }
        }
    } //SaveStringAsPdf

    // Создание файла .txt
    public static void SaveAsTxt(List<string> text, string filePath, string title) {
        using (var writer = new StreamWriter(filePath)) {
            writer.Write(title); 
            text.ForEach(writer.Write);
        }
    } //SaveAsTxt

    // Создание типизированного шрифта 
    private static string ArialFontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
    private static readonly BaseFont BaseFont = BaseFont.CreateFont(ArialFontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
    public static Font GetArialFont(float size = 15, int style = Font.NORMAL) => new(BaseFont, size, style);

} //Utils