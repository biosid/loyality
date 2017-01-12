namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using System.Collections.Generic;

    using Entities;

    public class UpdatePartnerParameters
    {
        /// <summary>
        /// Внутренний идентификатор партнера
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор пользователя обновляющего партнера.
        /// </summary>
        public string UpdatedUserId
        {
            get;
            set;
        }

        /// <summary>
        /// Описание партнера.
        /// </summary>
        public string NewDescription
        {
            get;
            set;
        }

        /// <summary>
        /// Название партнера.
        /// </summary>
        public string NewName
        {
            get;
            set;
        }

        /// <summary>
        /// Тип партнёра
        /// </summary>
        public PartnerType NewType
        {
            get;
            set;
        }

        /// <summary>
        /// Статус партнёра
        /// </summary>
        public PartnerStatus NewStatus
        {
            get;
            set;
        }

        /// <summary>
        /// Степень доверия партнёра
        /// </summary>
        public PartnerThrustLevel NewThrustLevel
        {
            get;
            set;
        }

        /// <summary>
        /// Идентификатор курьера
        /// </summary>
        public int? NewCarrierId
        {
            get;
            set;
        }

        public Dictionary<string, string> NewSettings { get; set; }
    }
}