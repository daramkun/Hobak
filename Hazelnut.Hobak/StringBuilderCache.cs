using System.Text;

namespace Hazelnut.Hobak;

internal static class StringBuilderCache
{
    private const int InitialCapacity = 64;
    
    private static readonly ConcurrentQueue<StringBuilder> Builders = new();

    public static StringBuilder Pop()
    {
        if (Builders.TryDequeue(out var result))
            return result;
        return new StringBuilder(InitialCapacity);
    }

    public static void Push(StringBuilder builder)
    {
        if (Builders.Contains(builder))
            return;
        builder.Clear();
        Builders.Enqueue(builder);
    }
}