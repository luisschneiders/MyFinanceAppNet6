using System.ComponentModel;

namespace MainApp.Helpers;

public class EnumHelper : IEnumHelper
{
    public string GetDescription(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field!, typeof(DescriptionAttribute))!;

        return attribute == null ? value.ToString() : attribute.Description;
    }
}
