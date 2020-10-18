using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Dodo.HttpClientResiliencePolicies.PhiFailureDetectorSettings
{
	public sealed class Statistics
	{
		private readonly ConcurrentQueue<long> _queue;
		private readonly int _capacity;

		private long _sum;
		private long _squaredSum;
		private double _avg;
		private double _squaredAvg;

		public long SquaredSum => _squaredSum;

		public double Avg => _avg;

		public double SquaredAvg => _squaredAvg;

		public int Count => _queue.Count;

		public Statistics(int capacity)
		{
			_capacity = capacity;
			_queue = new ConcurrentQueue<long>();
		}

		public void Add(long item)
		{
			if (Count == _capacity)
			{
				_queue.TryDequeue(out var value);
				_sum -= value;
				_squaredSum -= value * value;
			}
			_queue.Enqueue(item);
			_sum += item;
			_squaredSum += item * item;
			_avg = _sum / Count;
			_squaredAvg = _avg * _avg;
		}
	}
}
