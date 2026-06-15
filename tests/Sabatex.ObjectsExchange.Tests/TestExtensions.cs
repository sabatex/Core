using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;
using Xunit.v3;

namespace Sabatex.ObjectsExchange.Tests
{
    [AttributeUsage( AttributeTargets.Method , AllowMultiple = false)]
    public sealed class TestPriorityAttribute : Attribute
    {
        public int Priority { get; }
        public TestPriorityAttribute(int priority) => Priority = priority;
    }

    public class PriorityOrderer : ITestCaseOrderer
    {
        static TValue GetOrCreate<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key)
    where TValue : new()
        {
            if (dictionary.TryGetValue(key, out var result))
                return result;

            result = new TValue();
            dictionary[key] = result;

            return result;
        }

        public IReadOnlyCollection<TTestCase> OrderTestCases<TTestCase>(IReadOnlyCollection<TTestCase> testCases)
           where TTestCase : notnull, ITestCase
        {
            var result = new List<TTestCase>();
            var sortedMethods = new SortedDictionary<int, List<IXunitTestCase>>();

            foreach (IXunitTestCase testCase in testCases)
            {
                var priority = 0;
                var attr = testCase.TestMethod.Method.GetCustomAttributes<TestPriorityAttribute>().FirstOrDefault();
                if (attr is not null)
                    priority = attr.Priority;

                GetOrCreate(sortedMethods, priority).Add(testCase);
            }

            foreach (var list in sortedMethods.Keys.Select(priority => sortedMethods[priority]))
            {
                list.Sort((x, y) => StringComparer.OrdinalIgnoreCase.Compare(x.TestMethod.Method.Name, y.TestMethod.Method.Name));
                for (var i = 0; i < list.Count; i++)
                    if (list[i] is TTestCase tTestCase)
                        result.Add(tTestCase);
            }

            return result;
        }
    }




}
