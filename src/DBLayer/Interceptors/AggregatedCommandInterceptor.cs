﻿using DBLayer.Common;
using System.Data;
using System.Data.Common;

namespace DBLayer.Interceptors;
class AggregatedCommandInterceptor : AggregatedInterceptor<ICommandInterceptor>, ICommandInterceptor
{
	protected override AggregatedInterceptor<ICommandInterceptor> Create()
	{
		return new AggregatedCommandInterceptor();
	}

	public DbCommand CommandInitialized(CommandEventData eventData, DbCommand command)
	{
		return Apply(() =>
		{
			foreach (var interceptor in Interceptors)
				command = interceptor.CommandInitialized(eventData, command);
			return command;
		});
	}

	public Option<object?> ExecuteScalar(CommandEventData eventData, DbCommand command, Option<object?> result)
	{
		return Apply(() =>
		{
			foreach (var interceptor in Interceptors)
				result = interceptor.ExecuteScalar(eventData, command, result);
			return result;
		});
	}

	public async Task<Option<object?>> ExecuteScalarAsync(CommandEventData eventData, DbCommand command, Option<object?> result, CancellationToken cancellationToken)
	{
		return await Apply(async () =>
		{
			foreach (var interceptor in Interceptors)
				result = await interceptor.ExecuteScalarAsync(eventData, command, result, cancellationToken)
				.ConfigureAwait(DBLayer.Common.Configuration.ContinueOnCapturedContext);
			return result;
		}).ConfigureAwait(DBLayer.Common.Configuration.ContinueOnCapturedContext);
	}

	public Option<int> ExecuteNonQuery(CommandEventData eventData, DbCommand command, Option<int> result)
	{
		return Apply(() =>
		{
			foreach (var interceptor in Interceptors)
				result = interceptor.ExecuteNonQuery(eventData, command, result);
			return result;
		});
	}

	public async Task<Option<int>> ExecuteNonQueryAsync(CommandEventData eventData, DbCommand command, Option<int> result, CancellationToken cancellationToken)
	{
		return await Apply(async () =>
		{
			foreach (var interceptor in Interceptors)
				result = await interceptor.ExecuteNonQueryAsync(eventData, command, result, cancellationToken)
				.ConfigureAwait(DBLayer.Common.Configuration.ContinueOnCapturedContext);
			return result;
		}).ConfigureAwait(DBLayer.Common.Configuration.ContinueOnCapturedContext);
	}

	public Option<DbDataReader> ExecuteReader(CommandEventData eventData, DbCommand command, CommandBehavior commandBehavior, Option<DbDataReader> result)
	{
		return Apply(() =>
		{
			foreach (var interceptor in Interceptors)
				result = interceptor.ExecuteReader(eventData, command, commandBehavior, result);
			return result;
		});
	}

	public async Task<Option<DbDataReader>> ExecuteReaderAsync(CommandEventData eventData, DbCommand command, CommandBehavior commandBehavior, Option<DbDataReader> result, CancellationToken cancellationToken)
	{
		return await Apply(async () =>
		{
			foreach (var interceptor in Interceptors)
				result = await interceptor.ExecuteReaderAsync(eventData, command, commandBehavior, result, cancellationToken)
				.ConfigureAwait(DBLayer.Common.Configuration.ContinueOnCapturedContext);
			return result;
		}).ConfigureAwait(DBLayer.Common.Configuration.ContinueOnCapturedContext);
	}

	public void AfterExecuteReader(CommandEventData eventData, DbCommand command, CommandBehavior commandBehavior, DbDataReader dataReader)
	{
		Apply(() =>
		{
			foreach (var interceptor in Interceptors)
				interceptor.AfterExecuteReader(eventData, command, commandBehavior, dataReader);
		});
	}
}