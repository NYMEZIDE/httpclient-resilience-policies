using System;
using System.Net;
using System.Threading.Tasks;
using Dodo.HttpClientResiliencePolicies.CircuitBreakerPolicy;
using Dodo.HttpClientResiliencePolicies.RetryPolicy;
using Dodo.HttpClientResiliencePolicies.Tests.DSL;
using NUnit.Framework;
using Polly.CircuitBreaker;

namespace Dodo.HttpClientResiliencePolicies.Tests
{
	[TestFixture]
	public class CircuitBreakerTests
	{
		[Test]
		public void Should_break_after_4_concurrent_calls()
		{
			const int retryCount = 5;
			const int minimumThroughput = 2;
			var retrySettings = RetryPolicySettings.Constant(retryCount, TimeSpan.FromMilliseconds(50));

			var wrapper = Create.HttpClientWrapperWrapperBuilder
				.WithStatusCode(HttpStatusCode.ServiceUnavailable)
				.WithTimeoutOverall(TimeSpan.FromSeconds(5))
				.WithCircuitBreakerSettings(BuildCircuitBreakerSettings(minimumThroughput))
				.WithRetrySettings(retrySettings)
				.Please();

			const int taskCount = 4;
			Assert.CatchAsync<BrokenCircuitException>(async () =>
				await Helper.InvokeMultipleHttpRequests(wrapper.Client, taskCount));

			Assert.AreEqual(minimumThroughput, wrapper.NumberOfCalls);
		}

		[Test]
		public async Task Should_Open_Circuit_Breaker_for_RU_and_do_not_affect_EE()
		{
			const int retryCount = 5;
			const int minimumThroughput = 2;
			var retrySettings = RetryPolicySettings.Constant(retryCount, TimeSpan.FromMilliseconds(50));

			var circuitBreakerSettings = BuildCircuitBreakerSettings(minimumThroughput);
			var wrapper = Create.HttpClientWrapperWrapperBuilder
				.WithHostAndStatusCode("ru-prod.com", HttpStatusCode.ServiceUnavailable)
				.WithHostAndStatusCode("ee-prod.com", HttpStatusCode.OK)
				.WithTimeoutOverall(TimeSpan.FromSeconds(5))
				.WithCircuitBreakerSettings(circuitBreakerSettings)
				.WithRetrySettings(retrySettings)
				.Please();

			const int taskCount = 4;
			Assert.CatchAsync<BrokenCircuitException>(async () =>
				await Helper.InvokeMultipleHttpRequests(wrapper.Client, taskCount,
					"http://ru-prod.com/Test1/Test2"));

			await Helper.InvokeMultipleHttpRequests(wrapper.Client, taskCount,
				"http://ee-prod.com/Test3/Test4/Test5");

			Assert.AreEqual(minimumThroughput + taskCount, wrapper.NumberOfCalls);
		}

		private static ICircuitBreakerPolicySettings BuildCircuitBreakerSettings(int throughput)
		{
			return new CircuitBreakerPolicySettings(
				failureThreshold: 0.5,
				minimumThroughput: throughput,
				durationOfBreak: TimeSpan.FromMinutes(1),
				samplingDuration: TimeSpan.FromMilliseconds(20)
			);
		}
	}
}
