namespace RapidSoft.Loaylty.PromoAction.Repositories.Core
{
    using RapidSoft.Loaylty.PromoAction.Api.Entities;

    public interface IEntityRepository<TEntity, in TKey> : IEntityRepositoryBase<TEntity, TKey>
		where TEntity : class, IEntity<TKey>
	{
		/// <summary>
		/// ���������� �������� � ���������.
		/// </summary>
		/// <param name="entity">
		/// ����������� ��������.
		/// </param>
		void Save(TEntity entity);

		/// <summary>
		/// �������� �������� �� ��������� �� ����������� ��������������.
		/// </summary>
		/// <param name="id">
		/// ���������� �������������.
		/// </param>
		void DeleteById(TKey id);
	}
}