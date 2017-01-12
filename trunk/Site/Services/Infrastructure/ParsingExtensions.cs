using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Vtb24.Site.Services.Infrastructure
{
    public static class ParsingExtensions
    {
        /// <summary>
        /// Распарсить текстовый список в иерархическую структуру
        /// </summary>
        /// <param name="text">Текст, содержащий список любой вложенности</param>
        /// <param name="map">Функция маппинга строк в сущности. На входе строка и родительская сущность</param>
        public static IEnumerable<T> ParsePlainTextList<T>(this string text, Func<string, T, T> map) where T: class
        {
            var id = 0;
            // "токенизация"
            var parsed =
                // нормализация
                Regex.Replace(text, "^?#.*$?", string.Empty, RegexOptions.Multiline)
                .Replace("    ", "\t")
                // разбивка
                .Split(new[] { '\n' })
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line => new { 
                    Id = id++,
                    Level = line.LastIndexOf("\t", StringComparison.Ordinal) + 1,
                    Text = line.Trim()
                })
                .ToArray();

            var index = new List<T>();
            var reversed = parsed.Reverse().ToArray();            

            // сборка
            for (var i = 0; i < parsed.Length; i++)
            {
                var current = parsed[i];
                var parent = current.Level == 0 
                                 ? null
                                 : reversed.MaybeSkip(parsed.Length - i).FirstOrDefault(c => c.Level < current.Level);

                var entity = map(current.Text, parent == null ? null : index[parent.Id]);
                index.Add(entity);

                if (parent == null)
                {
                    yield return entity;
                }
            }
        }
    }
}