namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    public class SetCategoriesPermissionsParameters
    {
        /// <summary>
        /// Идентификатор партнера.
        /// </summary>
        public int PartnerId { get; set; }

        /// <summary>
        /// Набор идентификаторов категорий для которых необходимо добавить разрешение.
        /// </summary>
        public int[] AddCategoriesId { get; set; }

        /// <summary>
        /// Набор идентификаторов категорий для которых необходимо удалить разрешение.
        /// </summary>
        public int[] RemoveCategoriesId { get; set; }

        /// <summary>
        /// Идентификатор пользователя, выполняющего изменение.
        /// </summary>
        public string UserId { get; set; }
    }
}