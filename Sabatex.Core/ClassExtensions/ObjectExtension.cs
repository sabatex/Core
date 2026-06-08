using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.Core.ClassExtensions;
/// <summary>
/// Реалізація методу розширення для заповнення атрибутів одного об'єкта значеннями атрибутів іншого об'єкта.
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// Розширення для заповнення атрибутів одного об'єкта значеннями атрибутів іншого об'єкта.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    public static T FillAttributes<T>(this T obj, object source) where T : class
    {
        foreach (var property in source.GetType().GetProperties())
        {
            var objAttr = obj.GetType().GetProperty(property.Name);
            if (objAttr != null)
            {
                objAttr.SetValue(obj, property.GetValue(source, null), null);
            }
        }
        return obj;
    }
}
