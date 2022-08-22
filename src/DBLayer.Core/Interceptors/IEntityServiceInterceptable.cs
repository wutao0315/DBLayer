using System;

namespace DBLayer.Interceptors
{
	interface IEntityServiceInterceptable
	{
		AggregatedInterceptor<IEntityServiceInterceptor>? Interceptors { get; }
	}
}
