using System;
using System.Collections.Generic;
using System.Text;

namespace Dodo.HttpClientResiliencePolicies.PhiFailureDetectorSettings
{
	public class PhiFailureStrategyFactory
	{
		public static IPhiFailureStrategy Exponental => new ExponentalStrategy();

		public static IPhiFailureStrategy Normal => new NormalStrategy();
	}
}
