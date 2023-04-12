namespace Hazelnut.Hobak;

public partial class Configuration
{
    public async Task<string> ToStringAsync()
    {
#if NETCOREAPP3_0_OR_GREATER
        await using var writer = new StringWriter();
        await ToWriterAsync(writer);
        return writer.ToString();
#else
        using var writer = new StringWriter();
        await ToWriterAsync(writer);
        return writer.ToString();
#endif
    }
    
    public Task ToStreamAsync(Stream stream, bool leaveOpen = true) =>
        ToWriterAsync(new StreamWriter(stream, Encoding.UTF8, 4096, leaveOpen), leaveOpen: false);
    
    public async Task ToWriterAsync(TextWriter writer, bool leaveOpen = true)
    {
        await __WriteToWriterAsync(writer, this[GlobalSectionName]);
        foreach (var keySection in _sections.Where(section => section.Key != GlobalSectionName))
            await __WriteToWriterAsync(writer, keySection.Value);

        if (!leaveOpen)
        {
            await writer.FlushAsync();
#if NETCOREAPP3_0_OR_GREATER
            await writer.DisposeAsync();
#else
            writer.Dispose();
#endif
        }
    }

    private static async Task __WriteToWriterAsync(TextWriter writer, Section section)
    {
        var sb = StringBuilderCache.Pop();

        try
        {
            if (section.SectionName != GlobalSectionName)
                sb.AppendFormat("[{0}]", section.SectionName).AppendLine();
            foreach (var keyValue in section)
                sb.AppendFormat("{0} = {1}", keyValue.Key, keyValue.Value).AppendLine();

            await writer.WriteAsync(sb.ToString());
        }
        finally
        {
            StringBuilderCache.Push(sb);
        }
    }
}