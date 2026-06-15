using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.Extensions.ClassExtensions;

public static class ObjectExtensions
{
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
