using System.Text.RegularExpressions;

namespace Core.ValueObjects;

public class Pesel : IEquatable<Pesel>
{
    private static readonly Regex PeselRegex = new(@"^\d{11}$", RegexOptions.Compiled);

    public string Value { get; }

    public Pesel(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("PESEL nie może być pusty.", nameof(value));
        if (!PeselRegex.IsMatch(value))
            throw new ArgumentException("PESEL musi składać się z 11 cyfr.", nameof(value));
        if (!IsChecksumValid(value))
            throw new ArgumentException("Niepoprawna suma kontrolna PESEL.", nameof(value));

        Value = value;
    }

    public DateTime BirthDate
    {
        get
        {
            var year = int.Parse(Value.AsSpan(0, 2));
            var month = int.Parse(Value.AsSpan(2, 2));
            var day = int.Parse(Value.AsSpan(4, 2));
            var century = month switch
            {
                > 80 => 1800,
                > 60 => 2200,
                > 40 => 2100,
                > 20 => 2000,
                _ => 1900
            };
            return new DateTime(century + year, month % 20, day);
        }
    }

    public Gender Gender => int.Parse(Value.AsSpan(9, 2)) % 2 == 0 ? Gender.Female : Gender.Male;

    private static bool IsChecksumValid(string pesel)
    {
        var weights = new[] { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };
        var sum = 0;
        for (var i = 0; i < 10; i++)
            sum += int.Parse(pesel[i].ToString()) * weights[i];

        var checksum = (10 - sum % 10) % 10;
        return checksum == int.Parse(pesel[10].ToString());
    }

    public bool Equals(Pesel? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is Pesel p && Equals(p);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;
    public static implicit operator string(Pesel p) => p.Value;
    public static explicit operator Pesel(string v) => new(v);

    public static bool IsValid(string value) =>
        !string.IsNullOrWhiteSpace(value)
        && PeselRegex.IsMatch(value)
        && IsChecksumValid(value);
}

public enum Gender
{
    Male,
    Female
}
