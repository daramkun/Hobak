namespace Hazelnut.Hobak;

[Serializable]
public partial class Configuration : IEnumerable<KeyValuePair<string, Section>>, IEnumerable<Section>
{
    public const string GlobalSectionName = "$Global$";

    private readonly ConcurrentDictionary<string, Section> _sections = new();

    public int Count => _sections.Count;

    public Section this[string sectionName]
    {
        get
        {
            if (_sections.TryGetValue(sectionName, out var section))
                return section;
            section = new Section(sectionName);
            return _sections.TryAdd(sectionName, section)
                ? section
                : _sections[sectionName];
        }
    }

    public Configuration()
    {
        _sections.TryAdd(GlobalSectionName, new Section(GlobalSectionName));
    }

    IEnumerator<KeyValuePair<string, Section>> IEnumerable<KeyValuePair<string, Section>>.GetEnumerator() =>
        _sections.GetEnumerator();

    IEnumerator<Section> IEnumerable<Section>.GetEnumerator() => _sections.Values.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _sections.GetEnumerator();
}