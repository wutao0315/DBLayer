using System.Threading.Tasks;

namespace DBLayer.Interceptors
{
	public abstract class EntityServiceInterceptor : IEntityServiceInterceptor
	{
		public virtual object EntityCreated(EntityCreatedEventData eventData, object entity) => entity;
	}
}
