using System;
using System.Net.Http;
using Polly;

namespace Dodo.HttpClientResiliencePolicies.RetrySettings
{
	public class SimpleRetryPolicySettings : IRetryPolicySettings
	{
		public int RetryCount { get; }
		public Func<int, TimeSpan> SleepDurationProvider { get; set; }
		public Action<DelegateResult<HttpResponseMessage>, TimeSpan> OnRetry { get; set; }

		public SimpleRetryPolicySettings()
		: this(Defaults.Retry.RetryCount)
		{
			RetryCount = Defaults.Retry.RetryCount;
			SleepDurationProvider = _defaultSleepDurationProvider;
			OnRetry = _doNothingOnRetry;
		}

		public SimpleRetryPolicySettings(int retryCount)
		{
			RetryCount = retryCount;
			SleepDurationProvider = _defaultSleepDurationProvider;
			OnRetry = _doNothingOnRetry;
		}

		private static readonly Func<int, TimeSpan> _defaultSleepDurationProvider =
			i => TimeSpan.FromMilliseconds(20 * Math.Pow(2, i));

		private static readonly Action<DelegateResult<HttpResponseMessage>, TimeSpan> _doNothingOnRetry = (_, __) => { };
	}
}
