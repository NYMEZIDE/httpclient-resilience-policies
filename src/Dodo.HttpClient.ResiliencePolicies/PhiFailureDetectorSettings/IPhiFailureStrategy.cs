using System;
using System.Collections.Generic;
using System.Text;

namespace Dodo.HttpClientResiliencePolicies.PhiFailureDetectorSettings
{
	public interface IPhiFailureStrategy
	{
		double Phi(double value, Statistics statistics);
	}
}
