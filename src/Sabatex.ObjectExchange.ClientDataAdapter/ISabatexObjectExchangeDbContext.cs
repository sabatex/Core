using Microsoft.EntityFrameworkCore;
using Sabatex.ObjectExchange.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.ObjectExchange.ClientDataAdapter
{
    /// <summary>
    /// Визначає контракт для контексту бази даних, який використовується в процесі обміну об'єктами між клієнтським додатком та сервером Sabatex Object Exchange.
    /// </summary>
    public interface ISabatexObjectExchangeDbContext
    {
        /// <summary>
        /// Набір об'єктів, які призначені для завантаження на сервер Sabatex Object Exchange. Кожен об'єкт у цьому наборі представляє дані, які будуть передані на сервер для обробки та збереження.
        /// </summary>
        DbSet<UploadObject> UploadObjects { get; set; }
        /// <summary>
        /// Набір об'єктів, які не вдалося розпізнати або обробити під час обміну даними з сервером Sabatex Object Exchange. Ці об'єкти можуть містити інформацію про помилки або невідповідності, які виникли під час процесу обміну даними, і можуть бути використані для діагностики та виправлення проблем у процесі обміну даними.
        /// </summary>
        DbSet<UnresolvedObject> UnresolvedObjects { get; set; }
        /// <summary>
        /// Зберегти зміни в базі даних асинхронно.
        /// Цей метод використовується для збереження змін, внесених до наборів даних (UploadObjects та UnresolvedObjects), у базі даних.
        /// Він приймає необов'язковий параметр cancellationToken, який дозволяє скасувати операцію збереження змін.
        /// </summary>
        /// <param name="cancellationToken">Токен скасування, який дозволяє скасувати операцію збереження змін.</param>
        /// <returns>Кількість змін, збережених у базі даних.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// Налаштовує модель даних для контексту бази даних, який використовується в процесі обміну об'єктами між клієнтським додатком та сервером Sabatex Object Exchange. Цей метод викликається під час створення моделі даних і дозволяє налаштувати структуру бази даних, визначити зв'язки між таблицями та встановити інші параметри моделі даних, необхідні для правильного функціонування процесу обміну даними з сервером Sabatex Object Exchange.
        /// </summary>
        /// <param name="builder">Об'єкт ModelBuilder, який використовується для налаштування моделі даних.</param>
        void SabatexObjectExchangeModelCreating(ModelBuilder builder)
        {
        }
    }
}
