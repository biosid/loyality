using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Vtb24.Site.Content.DataAccess;
using Vtb24.Site.Content.Infrastructure;
using Vtb24.Site.Content.Snapshots.Models;
using System.Linq;

namespace Vtb24.Site.Content.Snapshots
{
    /// <summary>
    /// Сервис для работы с историей
    /// </summary>
    internal class Snapshots : ISnapshots
    {
        /// <summary>
        /// Создание снимка сущности для истории
        /// </summary>
        /// <remarks>
        /// Снимок создается только в том случае, если он отличается от последнего добавленного
        /// </remarks>
        /// <typeparam name="T">тип сущности</typeparam>
        /// <param name="entityId">id сущности. Требуется для связывания</param>
        /// <param name="entity">сама сущность</param>
        /// <param name="author">пользователь, создавший снимок</param>
        public bool Create<T>(long entityId, T entity, string author) where T : class
        {
            var serializedContent = Serialize(entity);

            var contentHash = serializedContent.GetMD5Hash();
            var type = typeof (T).ToString();

            using (var context = new ContentServiceDbContext())
            {
                var lastSnapshot = context.Snapshots
                                          .Where(s => s.Type == type)
                                          .OrderByDescending(s => s.CreationDate)
                                          .FirstOrDefault();

                if (lastSnapshot == null || contentHash != lastSnapshot.ContentHash)
                {
                    var snapshot = new DbSnapshot
                    {
                        Id = Guid.NewGuid().ToString("N"), 
                        Author = author,
                        SerializedContent = serializedContent,
                        Type = type,
                        EntityId = entityId,
                        CreationDate = DateTime.Now,
                        ContentHash = serializedContent.GetMD5Hash()
                    };

                    context.Snapshots.Add(snapshot);

                    context.SaveChanges();
                }
                else
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Получение снимка по его Id
        /// </summary>
        /// <typeparam name="T">тип сущности</typeparam>
        /// <param name="id">Id снимка</param>
        /// <returns>снимок сущности</returns>
        public Snapshot<T> GetById<T>(string id) where T : class
        {
            using (var context = new ContentServiceDbContext())
            {
                var snapshot = context.Snapshots.Find(id);

                if (snapshot == null)
                {
                    throw new InvalidOperationException(string.Format("Unknown Id: {0}", id));
                }

                var entity = Deserialize<T>(snapshot.SerializedContent);

                return new Snapshot<T>
                {
                    Author = snapshot.Author,
                    CreationDate = snapshot.CreationDate,
                    Entity = entity,
                    Id = id
                };
            }
        }
        
        /// <summary>
        /// Получение списка снимков сущности по ее Id
        /// </summary>
        /// <typeparam name="T">тип сущности</typeparam>
        /// <param name="entityId">Id сущности</param>
        /// <returns>массив снимков сущности</returns>
        public Snapshot<T>[] GetByEntityId<T>(long entityId) where T : class
        {
            using (var context = new ContentServiceDbContext())
            {
                var dbsnapshots = context.Snapshots
                                         .Where(s => s.EntityId == entityId);

                var snapshots = dbsnapshots.ToArray().Select(s => new Snapshot<T>
                {
                    Author = s.Author,
                    CreationDate = s.CreationDate,
                    Entity = Deserialize<T>(s.SerializedContent),
                    Id = s.Id
                }).ToArray();

                return snapshots;
            }
        }

        private static string Serialize<T>(T entity) where T : class
        {
            if (!typeof(T).IsSerializable &&
                !(typeof(ISerializable).IsAssignableFrom(typeof(T))))
            {
                throw new InvalidOperationException("A serializable Type is required");
            }

            var xmlSerializer = new XmlSerializer(typeof(T));

            var serializedContent = string.Empty;
            using (var stringWriter = new StringWriter())
            {
                xmlSerializer.Serialize(stringWriter, entity);
                serializedContent = stringWriter.ToString();
            }

            return serializedContent;
        }

        private static T Deserialize<T>(string serializedContent) where T : class
        {
            var xmlSerializer = new XmlSerializer(typeof(T));

            T entity;
            using (TextReader reader = new StringReader(serializedContent))
            {
                entity = (T)xmlSerializer.Deserialize(reader);
            }

            return entity;
        }
    }
}
