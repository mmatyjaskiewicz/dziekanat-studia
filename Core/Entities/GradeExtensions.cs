namespace Core.Entities;

// Klasa rozszerzeń dla typu wyliczeniowego GradeValue.
// Udostępnia konwersje na wartość liczbową, parsowanie łańcucha znaków
// oraz mapowanie na polską nazwę oceny.
public static class GradeExtensions
{
    public static double Value(this GradeValue gradeType)
    {
        return (int)gradeType / 10.0;
    }

    public static string PolishName(this GradeValue gradeType)
    {
        return gradeType switch
        {
            GradeValue.Grade20 => "niedostateczny",
            GradeValue.Grade30 => "dostateczny",
            GradeValue.Grade35 => "dostateczny plus",
            GradeValue.Grade40 => "dobry",
            GradeValue.Grade45 => "dobry plus",
            GradeValue.Grade50 => "bardzo dobry",
            _ => string.Empty
        };
    }

    public static GradeValue Parse(string gradeString)
    {
        return gradeString switch
        {
            "2.0" => GradeValue.Grade20,
            "3.0" => GradeValue.Grade30,
            "3.5" => GradeValue.Grade35,
            "4.0" => GradeValue.Grade40,
            "4.5" => GradeValue.Grade45,
            "5.0" => GradeValue.Grade50,
            _ => throw new ArgumentException($"Invalid grade: {gradeString}")
        };
    }

    public static List<string> GradeValues()
    {
        return Enum.GetValues<GradeValue>().Select(g => g.Value().ToString("N1")).ToList();
    }
}
