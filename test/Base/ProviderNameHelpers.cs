﻿using System.Collections.Generic;
using System.Linq;
using DBLayer;

namespace Tests
{
	public static class ProviderNameHelpers
	{
		/// <summary>
		/// Returns <c>true</c>, if <paramref name="context"/> is a context or provider name for provider, mentioned
		/// in any of <paramref name="providers"/>.
		/// </summary>
		/// <param name="context">Test provider name or context name (e.g. with .LinqService suffix).</param>
		/// <param name="providers">List of test providers to check against. Each entry could be provider name or comma-separated list of providers.</param>
		public static bool IsAnyOf(this string context, params string[] providers)
		{
			var providerName = context.Replace(".LinqService", string.Empty);
			foreach (var provider in providers)
			{
				if (provider.Split(',').Any(p => p == providerName))
					return true;
			}

			return false;
		}

		/// <summary>
		/// Converts list of strings where each string contain one or more provider names with comma as separator
		/// to a sequence of provider names.
		/// </summary>
		public static IEnumerable<string> SplitAll(this IEnumerable<string> providers)
		{
			foreach (var provider in providers)
			{
				foreach (var prov in provider.Split(','))
					yield return prov;
			}
		}
	}
}
