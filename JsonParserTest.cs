using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.ObjectsExchange.Tests
{
    using Sabatex.ObjectExchange.Core;
    using System.Collections.Generic;
    using Xunit;

    public class JsonParserTests
    {
        [Fact]
        public void Test_ScalarValuesAndIndexer()
        {
            string json = "{\"name\":\"John\", \"age\":30, \"isActive\":true}";
            var root = JsonParser.ParseJson(json);

            // Перевірка доступу до скалярних значень з кореневого словника
            Assert.Equal("John", root["name"].ScalarValue);
            Assert.Equal("30", root["age"].ScalarValue);
            Assert.Equal("true", root["isActive"].ScalarValue);

            // Спроба звернутися до неіснуючого ключа повертає null
            Assert.Null(root["nonexistent"]);
        }

        [Fact]
        public void Test_ObjectIndexer()
        {
            string json = "{\"user\":{\"id\":1, \"username\":\"Alice\"}}";
            var root = JsonParser.ParseJson(json);

            // Отримуємо обʼєкт "user" та звертаємось через індексатор
            var userWrapper = root["user"];
            Assert.NotNull(userWrapper);
            Assert.Equal("1", userWrapper["id"]?.ScalarValue);
            Assert.Equal("Alice", userWrapper["username"]?.ScalarValue);

            // Звернення за відсутнім ключем – має повернути null
            Assert.Null(userWrapper["nonexistent"]);
        }

        [Fact]
        public void Test_ArrayOfObjectsAndIndexer()
        {
            string json = "{\"users\":[ {\"id\":1, \"username\":\"Alice\"}, {\"id\":2, \"username\":\"Bob\"} ]}";
            var root = JsonParser.ParseJson(json);

            var usersArray = root["users"].ArrayValue;
            Assert.NotNull(usersArray);
            Assert.Equal(2, usersArray!.Count);

            // Перевірка доступу до полів обʼєктів із масиву через індексатор
            Assert.Equal("1", usersArray[0]["id"].ScalarValue);
            Assert.Equal("Alice", usersArray[0]["username"].ScalarValue);

            Assert.Equal("2", usersArray[1]["id"].ScalarValue);
            Assert.Equal("Bob", usersArray[1]["username"].ScalarValue);
        }
    }

}
