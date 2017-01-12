namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using System;
    using System.Collections.Generic;

    using Entities;

    public class CreatePartnerParameters
    {
        /// <summary>
        /// Идентификатор клиента создающего партнера.
        /// </summary>
        public string UserId
        {
            get;
            set;
        }

        /// <summary>
        /// Описание партнера.
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Название партнера.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Тип партнёра
        /// </summary>
        public PartnerType Type
        {
            get;
            set;
        }

        /// <summary>
        /// Статус партнёра
        /// </summary>
        public PartnerStatus Status
        {
            get;
            set;
        }

        /// <summary>
        /// Степень доверия партнёра
        /// </summary>
        public PartnerThrustLevel ThrustLevel
        {
            get;
            set;
        }

        /// <summary>
        /// Идентификатор курьера
        /// </summary>
        public int? CarrierId
        {
            get;
            set;
        }

        /// <summary>
        /// Признак что партнёр курьер
        /// </summary>
        public bool IsCarrier
        {
            get;
            set;
        }

        public Dictionary<string, string> Settings { get; set; }
    }
}
