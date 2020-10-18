using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Dodo.HttpClientResiliencePolicies.PhiFailureDetectorSettings
{
	public class PhiFailureDetector
	{
		private readonly Statistics _statistics;
		private readonly long _initialInterval;
		private long _last;
		private readonly IPhiFailureStrategy _strategy;

		public PhiFailureDetector(
			long initialInterval,
			IPhiFailureStrategy strategy)
		{
			_statistics = new Statistics(100);
			_initialInterval = initialInterval;
			_strategy = strategy;
		}

		public double Phi()
		{
			var now = Stopwatch.GetTimestamp();
			_last = now;

			if (_statistics.Count == 0)
			{
				_statistics.Add(_initialInterval);
			}
			else
			{
				var interval = now - _last;
				_statistics.Add(interval);
			}

			return (long)_strategy.Phi(now - _last, _statistics);
		}
	}
}
