namespace Hazelnut.Hobak;

public partial class Configuration
{
    public override string ToString() => ToStringAsync().GetAwaiter().GetResult();
    public void ToStream(Stream stream, bool leaveOpen = true) =>
        ToStreamAsync(stream, leaveOpen).GetAwaiter().GetResult();
    public void ToWriter(TextWriter writer, bool leaveOpen = true) =>
        ToWriterAsync(writer, leaveOpen).GetAwaiter().GetResult();
}