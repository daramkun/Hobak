namespace Hazelnut.Hobak;

[Serializable]
public class Section : IEnumerable<KeyValuePair<string, string>>
{
    private ConcurrentDictionary<string, string> _keyValues = new();

    public string SectionName { get; }

    public int Count => _keyValues.Count;

    internal Section(string sectionName)
    {
        SectionName = sectionName;
    }

    public SectionValue this[string key]
    {
        get => _keyValues.TryGetValue(key, out var value) ? value : string.Empty;
        set
        {
            if (_keyValues.TryGetValue(key, out var originalValue))
                _keyValues.TryUpdate(key, value, originalValue);
            else
                _keyValues.TryAdd(key, value);
        }
    }

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => _keyValues.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _keyValues.GetEnumerator();
}