using DBLayer.Interceptors;
using DBLayer.Mapping;
using Tests.Model;

namespace Tests.Remote.ServerContainer
{
	public interface IServerContainer
	{
		bool KeepSamePortBetweenThreads { get; set; }

		ITestDataContext Prepare(MappingSchema? ms, IInterceptor? interceptor, bool suppressSequentialAccess, string configuration);
	}
}
