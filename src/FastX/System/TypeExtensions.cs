using System.ComponentModel;

namespace System;

/// <summary>
/// Extension methods for Type class.
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// IsBasicType
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsBasicType(this Type type)
    {
        return type.IsPrimitive    
               || type.IsEnum        
               || type == typeof(string)
               || type == typeof(DateTime);
    }

    /// <summary>
    /// ConvertFromString
    /// </summary>
    /// <param name="type"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static object? ConvertFromString(this Type type,string value)
    {
        if (type == typeof(string))
            return value;

        if (type.IsEnum)
            return Enum.Parse(type, value);

        var converter = TypeDescriptor.GetConverter(type);
        return converter.ConvertFromString(value);  
    }
}