using System.Reflection;

namespace ThePlant.Services.Services;

public static class FilesTemplateHelper
{
    public static string? ReadEmailTemplate(string nameOfTemplate)
    {
        var type = typeof(HtmlTemplates);
        var property = type.GetProperty(nameOfTemplate, BindingFlags.Static | BindingFlags.Public);

        return property == null ? null : property.GetValue(null)?.ToString();
    }
}