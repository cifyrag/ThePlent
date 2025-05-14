using System.Reflection;

namespace ThePlant.Services.Services;

public static class TemplateHelper
{
    public static async Task<Dictionary<string, string>> ToTemplateData<T>(T model)
    {
        var templateData = new Dictionary<string, string>();

        foreach (PropertyInfo property in typeof(T).GetProperties())
        {
            var value = property.GetValue(model);

            var formattedValue = value switch
            {
                DateTime dateTimeValue => dateTimeValue.ToString("yyyy-MM-dd"),
                decimal decimalValue => decimalValue.ToString("F2"),
                _ => value?.ToString() ?? string.Empty
            };

            templateData[$"{property.Name.ToLower()}"] = formattedValue;
        }

        return templateData;
    }
    
    public static string ReplaceTemplateData(string template, Dictionary<string, string> templateData)
    {
        return templateData.Keys.Aggregate(template, (current, key) => current.Replace($"{{{{{key}}}}}", templateData[key]));
    }
}