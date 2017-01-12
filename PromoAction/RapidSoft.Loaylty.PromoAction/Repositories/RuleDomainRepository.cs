namespace RapidSoft.Loaylty.PromoAction.Repositories 
{
    using System;
    using System.Linq;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Repositories.Core;

    /// <summary>
    /// ����������� ��� �������� <see cref="RuleDomain"/>.
    /// </summary>
    public class RuleDomainRepository : TraceableEntityRepository<RuleDomain>, IRuleDomainRepository
    {
        /// <summary>
        /// ��������� ������ ������ �� ��������.
        /// </summary>
        /// <param name="name">
        /// �������� ������ ������.
        /// </param>
        /// <returns>
        /// ��������� ����� ������.
        /// </returns>
        public RuleDomain GetByName(string name)
        {
            return name == null ? null : this.GetEntityDbSet().SingleOrDefault(x => x.Name == name);
        }

        protected override void BeforeSave(RuleDomain entity)
        {
            base.BeforeSave(entity);

            if (entity != null && entity.Id != 0)
            {
                entity.UpdatedDate = DateTime.Now;
            }
        }
    }
}