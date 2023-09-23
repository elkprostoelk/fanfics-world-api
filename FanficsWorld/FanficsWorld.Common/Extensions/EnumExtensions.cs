using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace FanficsWorld.Common.Extensions;

public static class EnumExtensions
{
    public static string GetDisplayAttribute(this Enum value)
    {
        var fieldInfo = value.GetType().GetField(value.ToString());

        var displayAttribute = fieldInfo?.GetCustomAttribute<DisplayAttribute>();

        return displayAttribute?.Name ?? value.ToString();
    }
}