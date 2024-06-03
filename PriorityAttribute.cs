using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiDocumentEx.Test;
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
internal class PriorityAttribute : Attribute
{
    public int Priority { get; private set; }
    public PriorityAttribute(int priority) => Priority = priority;
}
