namespace Hazelnut.Hobak;

public partial class Configuration
{
    public static Configuration FromString(string text) =>
        FromStringAsync(text).GetAwaiter().GetResult();
    public static Configuration FromFile(string filename) =>
        FromFileAsync(filename).GetAwaiter().GetResult();
    public static Configuration FromStream(Stream stream, bool leaveOpen = true) =>
        FromStreamAsync(stream, leaveOpen).GetAwaiter().GetResult();
    public static Configuration FromReader(TextReader reader, bool leaveOpen = true) =>
        FromReaderAsync(reader, leaveOpen).GetAwaiter().GetResult();
}