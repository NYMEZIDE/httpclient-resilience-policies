using System;
using System.Collections.Generic;
using System.Text;

namespace Dodo.HttpClientResiliencePolicies.PhiFailureDetectorSettings
{
	internal class NormalStrategy : IPhiFailureStrategy
	{
		public double Phi(double value, Statistics statistics)
		{
			//var duration = now - last;
			var deviation = Math.Sqrt(((double)statistics.SquaredSum / statistics.Count) - statistics.SquaredAvg);
			var y = (value - statistics.Avg) / deviation;
			var exp = Math.Exp(-y * (1.5976 + 0.070566 * y * y));
			if (value > statistics.Avg)
			{
				return -Math.Log10(exp / (1 + exp));
			}
			else
			{
				return -Math.Log10(1 - 1 / (1 + exp));
			}
		}
	}
}
