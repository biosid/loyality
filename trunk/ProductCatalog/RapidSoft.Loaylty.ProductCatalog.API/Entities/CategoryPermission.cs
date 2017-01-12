using System;

namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    /// <summary>
    /// Представляет связку категории и партнера указывающие на надоступность категории партнеру
    /// </summary>
    public class CategoryPermission
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryPermission"/> class.
        /// </summary>
        public CategoryPermission()
        {
            this.InsertedDate = DateTime.Now;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryPermission"/> class.
        /// </summary>
        /// <param name="partnerId">
        /// Идентификатор партнера.
        /// </param>
        /// <param name="categoryId">
        ///  Идентификатор категории.
        /// </param>
        /// <param name="insertedUserId">
        /// Идентификатор пользователя создавшего связку (установившего разрешение).
        /// </param>
        public CategoryPermission(int partnerId, int categoryId, string insertedUserId)
        {
            this.PartnerId = partnerId;
            this.CategoryId = categoryId;
            this.InsertedUserId = insertedUserId;
            this.InsertedDate = DateTime.Now;
        }

        /// <summary>
        /// Идентификатор категории.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Идентификатор партнера.
        /// </summary>
        public int PartnerId { get; set; }

        /// <summary>
        /// Идентификатор пользователя создавшего связку (установившего разрешение).
        /// </summary>
        public string InsertedUserId { get; set; }

        /// <summary>
        /// Дата и время создания связки.
        /// </summary>
        public DateTime InsertedDate { get; set; }
    }
}
