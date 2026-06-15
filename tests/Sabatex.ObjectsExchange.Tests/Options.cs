using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.ObjectsExchange.Tests;

public class Options<T>: IOptions<T> where T : class, new()
{
    public Options(T value)
    {
        Value = value;
    }
    public T Value { get; set; } = new T();
    public T Get(string name)
    {
        return Value;
    }
}

