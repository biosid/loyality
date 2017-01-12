using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidSoft.VTB24.BankConnector.Tests.StubServices
{
	public abstract class StubBase
	{
		protected string GetStubDescription()
		{
			return string.Format("Stub ({0}) is used", this.GetType().FullName);
		}
	}
}
