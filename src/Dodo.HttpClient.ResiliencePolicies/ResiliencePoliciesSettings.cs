﻿using System;
using Dodo.HttpClientResiliencePolicies.CircuitBreakerSettings;
using Dodo.HttpClientResiliencePolicies.RetrySettings;

namespace Dodo.HttpClientResiliencePolicies
{
	public class ResiliencePoliciesSettings
	{
		public TimeSpan HttpClientTimeout { get; set; }
		public TimeSpan TimeoutPerTry { get; set; }

		public IRetrySettings RetrySettings { get; set; }
		public ICircuitBreakerSettings CircuitBreakerSettings { get; set; }


		public static ResiliencePoliciesSettings Default() =>
			new ResiliencePoliciesSettings
		{
			HttpClientTimeout = TimeSpan.FromMilliseconds(Defaults.Timeout.HttpClientTimeoutInMilliseconds),
			TimeoutPerTry = TimeSpan.FromMilliseconds(Defaults.Timeout.TimeoutPerTryInMilliseconds),
			RetrySettings = JitterRetrySettings.Default(),
			CircuitBreakerSettings =
			HttpClientResiliencePolicies.CircuitBreakerSettings.CircuitBreakerSettings.Default()
		};

	}
}