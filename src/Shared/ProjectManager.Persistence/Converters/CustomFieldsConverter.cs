using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace ProjectManager.Persistence.Converters;

public sealed class CustomFieldsConverter : ValueConverter<Dictionary<string, object>, string>
{
    public CustomFieldsConverter() : base(
        v => JsonSerializer.Serialize(v, new JsonSerializerOptions { WriteIndented = false }),
        v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, new JsonSerializerOptions())
    )
    { }
}
