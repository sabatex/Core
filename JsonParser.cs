using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sabatex.ObjectExchange.Core
{
    public class JsonValueWrapper
    {
        /// <summary>
        /// Тип значення за JSON (String, Number, Object, Array, True, False, Null).
        /// </summary>
        public JsonValueKind Kind { get; set; }
        public string? Name { get; set; }

        /// <summary>
        /// Скалярне значення (рядок, чи його JSON-представлення для чисел/булевих)
        /// </summary>
        public string? ScalarValue { get; set; }
        /// <summary>
        /// Обʼєкт – представлений як словник, якщо елемент є JSON-обʼєктом.
        /// </summary>
        public Dictionary<string, JsonValueWrapper>? ObjectValue { get; set; }

        /// <summary>
        /// Масив – представлений як список словників, якщо елемент є JSON-масивом.
        /// Якщо якийсь елемент масиву не є обʼєктом, то він буде обгорнутий у словник за ключем "value".
        /// </summary>
        public List<Dictionary<string, JsonValueWrapper>>? ArrayValue { get; set; }
        /// <summary>
        /// Індексатор, який дозволяє отримувати дочірні елементи за ключем.
        /// Якщо поточний елемент не є обʼєктом або в ньому відсутній потрібний ключ – повертається null.
        /// </summary>
        /// <param name="key">Ім'я поля (ключ) в JSON-обʼєкті</param>
        public JsonValueWrapper? this[string key]
        {
            get
            {
                if (Kind != JsonValueKind.Object || ObjectValue == null)
                    return null;
                return ObjectValue.TryGetValue(key, out var value) ? value : null;
            }
        }
    }

    public class JsonParser
    {
        /// <summary>
        /// Розбирає довільний JSON і повертає його кореневий об’єкт як Dictionary.
        /// </summary>
        /// <param name="json">Вхідний JSON</param>
        /// <returns>Словник, де ключ — це поле JSON, а значення — допоміжний клас JsonValueWrapper.</returns>
        public static JsonValueWrapper ParseJson(string json)
        {
            var element = JsonSerializer.Deserialize<JsonElement>(json);
            return ParseValue(element);
        }

        /// <summary>
        /// Рекурсивно розбирає JSON-об’єкт і повертає його як Dictionary.
        /// </summary>
        private static Dictionary<string, JsonValueWrapper> ParseObject(JsonElement element)
        {
            var dict = new Dictionary<string, JsonValueWrapper>();

            foreach (var prop in element.EnumerateObject())
            {
                dict[prop.Name] = ParseValue(prop.Value);
            }

            return dict;
        }

        /// <summary>
        /// Розбирає окремий JSON-елемент і повертає допоміжний клас, який містить тип значення та дані.
        /// </summary>
        private static JsonValueWrapper ParseValue(JsonElement element)
        {
            var wrapper = new JsonValueWrapper { Kind = element.ValueKind};

            switch (element.ValueKind)
            {
                case JsonValueKind.String:
                    wrapper.ScalarValue = element.GetString();
                    break;
                case JsonValueKind.Number:
                    // Зберігаємо число як рядок – GetRawText повертає його точне JSON-представлення.
                    wrapper.ScalarValue = element.GetRawText();
                    break;
                case JsonValueKind.True:
                case JsonValueKind.False:
                    wrapper.ScalarValue = element.GetRawText();
                    break;
                case JsonValueKind.Object:
                    wrapper.ObjectValue = ParseObject(element);
                    break;
                case JsonValueKind.Array:
                    wrapper.ArrayValue = ParseArray(element);
                    break;
                case JsonValueKind.Null:
                case JsonValueKind.Undefined:
                    // Залишаємо все як null
                    break;
                default:
                    break;
            }

            return wrapper;
        }

        /// <summary>
        /// Розбирає JSON-масив і повертає його як List словників.
        /// Припускається, що елементи масиву є об’єктами. Якщо елемент не є об’єктом, він обгортається в словник з ключем "value".
        /// </summary>
        private static List<Dictionary<string, JsonValueWrapper>> ParseArray(JsonElement element)
        {
            var list = new List<Dictionary<string, JsonValueWrapper>>();

            foreach (var item in element.EnumerateArray())
            {
                if (item.ValueKind == JsonValueKind.Object)
                {
                    list.Add(ParseObject(item));
                }
                else
                {
                    // Якщо елемент не об’єкт, обгортаємо його в словник із ключем "value"
                    list.Add(new Dictionary<string, JsonValueWrapper>
                {
                    { "value", ParseValue(item) }
                });
                }
            }

            return list;
        }
    }

}
