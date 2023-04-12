namespace Hazelnut.Hobak;

public partial class Configuration
{
    public static Task<Configuration> FromStringAsync(string text) => FromReaderAsync(new StringReader(text));

    public static async Task<Configuration> FromFileAsync(string filename)
    {
        if (filename.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
            filename.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            var response = await new HttpClient().GetAsync(filename);
            return await FromStreamAsync(await response.Content.ReadAsStreamAsync(), false);
        }

        if (Path.IsPathRooted(filename))
        {
            if (File.Exists(filename))
                return await FromStreamAsync(new FileStream(filename, FileMode.Open), false);
            throw new FileNotFoundException("File is not found.", filename);
        }

        var fromWorkingDir = Path.Combine(Environment.CurrentDirectory, filename);
        if (File.Exists(fromWorkingDir))
            return await FromStreamAsync(new FileStream(fromWorkingDir, FileMode.Open), false);

        var fromAppPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, filename);
        if (File.Exists(fromAppPath))
            return await FromStreamAsync(new FileStream(fromAppPath, FileMode.Open), false);
        
        throw new FileNotFoundException("File is not found.", filename);
    }

    public static Task<Configuration> FromStreamAsync(Stream stream, bool leaveOpen = true) =>
        FromReaderAsync(new StreamReader(stream, Encoding.UTF8, true, 4096, leaveOpen), leaveOpen: false);

    public static async Task<Configuration> FromReaderAsync(TextReader reader, bool leaveOpen = true)
    {
        var result = new Configuration();
        var currentSection = result[GlobalSectionName];

        while (true)
        {
            var i = 0;
            var line = await reader.ReadLineAsync();
            if (line == null)
                break;
            if (line.Length == 0)
                continue;
            for (; i < line.Length; ++i)
            {
                var ch = line[i];
                if (ch != ' ' && ch != '\t' && ch != '\a' && ch != '\r')
                    break;
            }

            switch (line[i])
            {
                case ';':
                    continue;
                
                case '[':
                    currentSection = result[__IniGetSectionTitle(line, i + 1)];
                    break;
                
                default:
                    var key = __IniGetKey(line, ref i);
                    var value = __IniGetValue(line, i);
                    currentSection[key] = value;
                    break;
            }
        }

        if (!leaveOpen)
            reader.Dispose();

        return result;
    }

    private static string __IniGetSectionTitle(string line, int startIndex)
    {
        var sb = StringBuilderCache.Pop();
        try
        {
            for (; startIndex < line.Length && line[startIndex] != ']'; ++startIndex)
                sb.Append(line[startIndex]);
            return sb.ToString();
        }
        finally
        {
            StringBuilderCache.Push(sb);
        }
    }

    private static string __IniGetKey(string line, ref int startIndex)
    {
        var sb = StringBuilderCache.Pop();
        try
        {
            for (; startIndex < line.Length && line[startIndex] != '='; ++startIndex)
                sb.Append(line[startIndex]);
            ++startIndex;
            return sb.ToString().Trim();
        }
        finally
        {
            StringBuilderCache.Push(sb);
        }
    }

    private static string __IniGetValue(string line, int startIndex)
    {
        if (line.Length == startIndex) return string.Empty;

        var sb = StringBuilderCache.Pop();
        try
        {
            for (; startIndex < line.Length; ++startIndex)
            {
                var ch = line[startIndex];
                if (ch != ' ' && ch != '\t' && ch != '\r')
                    break;
            }

            if (line[startIndex] == '"')
            {
                ++startIndex;
                for (; startIndex < line.Length && line[startIndex] != '"'; ++startIndex)
                    sb.Append(line[startIndex]);
            }
            else
            {
                for (; startIndex < line.Length && (line[startIndex] != '\n' && line[startIndex] != ';'); ++startIndex)
                    sb.Append(line[startIndex]);
            }

            return sb.ToString().Trim();
        }
        finally
        {
            StringBuilderCache.Push(sb);
        }
    }
}