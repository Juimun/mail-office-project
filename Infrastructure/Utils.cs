using System.IO;
using Newtonsoft.Json;
using System.Text;
using MailOffice.Models.Entities.Accounts;
using MailOffice.Models.Reports;

namespace MailOffice.Infrastructure;

public static class Utils {

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
     
    public static void JsonSerialize(List<JsonSerialize> dataList, string fileName) =>
        File.WriteAllText(fileName, JsonConvert.SerializeObject(dataList, Formatting.Indented), Encoding.UTF8);

    public static List<JsonSerialize> JsonDeserialize(string fileName) =>   
        JsonConvert.DeserializeObject<List<JsonSerialize>>(File.ReadAllText(fileName, Encoding.UTF8))!;
  

} //Utils