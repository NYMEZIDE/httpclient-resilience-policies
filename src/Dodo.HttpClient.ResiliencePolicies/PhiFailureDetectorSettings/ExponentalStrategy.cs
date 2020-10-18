using System;
using System.Collections.Generic;
using System.Text;

namespace Dodo.HttpClientResiliencePolicies.PhiFailureDetectorSettings
{
	internal class ExponentalStrategy : IPhiFailureStrategy
	{
		public double Phi(double value, Statistics statistics)
		{
			if (statistics.Avg == 0)
				return 0;

			return value / statistics.Avg;
		}
	}
}
