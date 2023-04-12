namespace Hazelnut.Hobak;

[Serializable]
public struct SectionValue
{
    private string _value;
    private char _splitter;

    internal SectionValue(string? value, char splitter = ',')
    {
        _value = value ?? string.Empty;
        _splitter = splitter;
    }

    public SectionValue ApplyArraySplitter(char splitter)
    {
        _splitter = splitter;
        return this;
    }

    private static T? ToValue<T>(string value)
    {
        var parseMethod = typeof(T).GetMethod("Parse", BindingFlags.Public | BindingFlags.Static);
        if (parseMethod == null)
            return (T?)Activator.CreateInstance(typeof(T), value);
        return ToValue(value, (Func<string, T?>)parseMethod.CreateDelegate(typeof(Func<string, T?>)));
    }
    public static T? ToValue<T>(string value, Func<string, T?> converter) => converter.Invoke(value);

    public T? ToValue<T>() => ToValue<T>(_value);
    public T? ToValue<T>(Func<string, T?> converter) => converter.Invoke(_value);
    public T?[] ToValueArray<T>() => _value.Split(_splitter).Select(ToValue<T>).ToArray();
    public T?[] ToValueArray<T>(Func<string, T?> converter) =>
        _value.Split(_splitter).Select(a => ToValue(a, converter)).ToArray();

    public static implicit operator SectionValue(string? value) => new(value);

    public static implicit operator string(SectionValue value) => value._value;
    public static implicit operator char[](SectionValue value) => value._value.ToArray();
    public static implicit operator char(SectionValue value) => char.Parse(value);
    public static implicit operator byte(SectionValue value) => byte.Parse(value);
    public static implicit operator sbyte(SectionValue value) => sbyte.Parse(value);
    public static implicit operator short(SectionValue value) => short.Parse(value);
    public static implicit operator ushort(SectionValue value) => ushort.Parse(value);
    public static implicit operator int(SectionValue value) => int.Parse(value);
    public static implicit operator uint(SectionValue value) => uint.Parse(value);
    public static implicit operator long(SectionValue value) => long.Parse(value);
    public static implicit operator ulong(SectionValue value) => ulong.Parse(value);
#if NET5_0_OR_GREATER
    public static implicit operator Half(SectionValue value) => Half.Parse(value);
#endif
    public static implicit operator float(SectionValue value) => float.Parse(value);
    public static implicit operator double(SectionValue value) => double.Parse(value);
    public static implicit operator decimal(SectionValue value) => decimal.Parse(value);
    public static implicit operator bool(SectionValue value) => bool.Parse(value);
#if NET5_0_OR_GREATER
    public static implicit operator IntPtr(SectionValue value) => IntPtr.Parse(value);
    public static implicit operator UIntPtr(SectionValue value) => UIntPtr.Parse(value);
#else
    public static implicit operator IntPtr(SectionValue value) => new((long)value);
    public static implicit operator UIntPtr(SectionValue value) => new((ulong)value);
#endif
    public static implicit operator TimeSpan(SectionValue value) => TimeSpan.Parse(value);
    public static implicit operator DateTime(SectionValue value) => DateTime.Parse(value);
    public static implicit operator Regex(SectionValue value) => new(value);

    public static implicit operator string[](SectionValue value) => value._value.Split(value._splitter);
    public static implicit operator SectionValue[](SectionValue value) => ((string[])value).Select(a => (SectionValue)a).ToArray();
    public static implicit operator byte[](SectionValue value) => ((SectionValue[])value).Select(a => (byte)a).ToArray();
    public static implicit operator sbyte[](SectionValue value) => ((SectionValue[])value).Select(a => (sbyte)a).ToArray();
    public static implicit operator short[](SectionValue value) => ((SectionValue[])value).Select(a => (short)a).ToArray();
    public static implicit operator ushort[](SectionValue value) => ((SectionValue[])value).Select(a => (ushort)a).ToArray();
    public static implicit operator int[](SectionValue value) => ((SectionValue[])value).Select(a => (int)a).ToArray();
    public static implicit operator uint[](SectionValue value) => ((SectionValue[])value).Select(a => (uint)a).ToArray();
    public static implicit operator long[](SectionValue value) => ((SectionValue[])value).Select(a => (long)a).ToArray();
    public static implicit operator ulong[](SectionValue value) => ((SectionValue[])value).Select(a => (ulong)a).ToArray();
#if NET5_0_OR_GREATER
    public static implicit operator Half[](SectionValue value) => ((SectionValue[])value).Select(a => (Half)a).ToArray();
#endif
    public static implicit operator float[](SectionValue value) => ((SectionValue[])value).Select(a => (float)a).ToArray();
    public static implicit operator double[](SectionValue value) => ((SectionValue[])value).Select(a => (double)a).ToArray();
    public static implicit operator decimal[](SectionValue value) => ((SectionValue[])value).Select(a => (decimal)a).ToArray();
    public static implicit operator bool[](SectionValue value) => ((SectionValue[])value).Select(a => (bool)a).ToArray();
    public static implicit operator IntPtr[](SectionValue value) => ((SectionValue[])value).Select(a => (IntPtr)a).ToArray();
    public static implicit operator UIntPtr[](SectionValue value) => ((SectionValue[])value).Select(a => (UIntPtr)a).ToArray();
    public static implicit operator TimeSpan[](SectionValue value) => ((SectionValue[])value).Select(a => (TimeSpan)a).ToArray();
    public static implicit operator DateTime[](SectionValue value) => ((SectionValue[])value).Select(a => (DateTime)a).ToArray();
    public static implicit operator Regex[](SectionValue value) => ((SectionValue[])value).Select(a => (Regex)a).ToArray();

#if NET6_0_OR_GREATER
    public static implicit operator Vector2(SectionValue value) => new((float[])value);
    public static implicit operator Vector3(SectionValue value) => new((float[])value);
    public static implicit operator Vector4(SectionValue value) => new((float[])value);
#endif

    public override string ToString() => _value;
}