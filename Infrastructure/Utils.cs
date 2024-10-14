using System.Text;

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

} //Utils