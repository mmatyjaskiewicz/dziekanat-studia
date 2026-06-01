using Core.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructre.EntityFramework.Converters;

public class PeselToStringConverter : ValueConverter<Pesel, string>
{
    public PeselToStringConverter() : base(v => v.Value, v => new Pesel(v))
    {
    }
}
