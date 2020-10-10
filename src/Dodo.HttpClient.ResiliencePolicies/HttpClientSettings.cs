using System;
using Dodo.HttpClient.ResiliencePolicies.CircuitBreakerSettings;
using Dodo.HttpClient.ResiliencePolicies.RetrySettings;

namespace Dodo.HttpClient.ResiliencePolicies
{
	public class HttpClientSettings
	{
		public TimeSpan HttpClientTimeout { get; }
		public TimeSpan TimeoutPerTry { get; }
		public TimeSpan? TimeoutOverall { get; }
		public IRetrySettings RetrySettings { get; }
		public ICircuitBreakerSettings CircuitBreakerSettings { get; }

		public HttpClientSettings(
			TimeSpan httpClientTimeout,
			TimeSpan timeoutPerTry,
			int retryCount,
			TimeSpan? timeoutOverall = null) : this(httpClientTimeout, timeoutPerTry,
			new JitterRetrySettings(retryCount),
			ResiliencePolicies.CircuitBreakerSettings.CircuitBreakerSettings.Default(),
			timeoutOverall)
		{
		}

		public HttpClientSettings(
			IRetrySettings retrySettings,
			ICircuitBreakerSettings circuitBreakerSettings) : this(
			TimeSpan.FromMilliseconds(Defaults.Timeout.HttpClientTimeoutInMilliseconds),
			TimeSpan.FromMilliseconds(Defaults.Timeout.TimeoutPerTryInMilliseconds),
			retrySettings,
			circuitBreakerSettings,
			TimeSpan.FromMilliseconds(Defaults.Timeout.TimeoutOverallInMillicesons))
		{
		}

		public HttpClientSettings(
			TimeSpan httpClientTimeout,
			TimeSpan timeoutPerTry,
			IRetrySettings retrySettings,
			ICircuitBreakerSettings circuitBreakerSettings,
			TimeSpan? timeoutOverall = null)
		{
			TimeoutOverall = timeoutOverall;

			HttpClientTimeout = (timeoutOverall == null || httpClientTimeout > timeoutOverall.Value)
				? httpClientTimeout
				: timeoutOverall.Value;

			TimeoutPerTry = timeoutPerTry;
			RetrySettings = retrySettings;
			CircuitBreakerSettings = circuitBreakerSettings;
		}

		public static HttpClientSettings Default() =>
			new HttpClientSettings(
				JitterRetrySettings.Default(),
				ResiliencePolicies.CircuitBreakerSettings.CircuitBreakerSettings.Default()
			);
	}
}
