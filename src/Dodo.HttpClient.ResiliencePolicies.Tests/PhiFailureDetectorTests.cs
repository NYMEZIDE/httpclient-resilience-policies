using Dodo.HttpClientResiliencePolicies.PhiFailureDetectorSettings;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dodo.HttpClientResiliencePolicies.Tests
{
	[TestFixture]
	public class PhiFailureDetectorTests
	{
		[Test]
		public void Check_Exponental_Strategy_with_linear_data()
		{
			var stats = new Statistics(5);
			stats.Add(100);
			stats.Add(100);
			stats.Add(100);
			stats.Add(100);
			stats.Add(100);

			var service = PhiFailureStrategyFactory.Exponental;

			Assert.AreEqual(1.0, service.Phi(100, stats));
			Assert.AreEqual(9.0, service.Phi(900, stats), 0.01);
		}

		[Test]
		public void Check_Exponental_Strategy_with_not_linear_data()
		{
			var stats = new Statistics(5);
			stats.Add(100);
			stats.Add(200);
			stats.Add(300);
			stats.Add(500);
			stats.Add(800);

			var service = PhiFailureStrategyFactory.Exponental;

			Assert.AreEqual(3.42, service.Phi(1300, stats), 0.01);
		}

		[Test]
		public void Check_Exponental_Strategy_with_empty_data()
		{
			var stats = new Statistics(5);
			var service = PhiFailureStrategyFactory.Exponental;

			Assert.AreEqual(0, service.Phi(0, stats));
		}
	}
}
