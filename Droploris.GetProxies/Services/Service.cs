/*
 ______   ______  _____   _____          _____   ______ _____ _______
 |     \ |_____/ |     | |_____] |      |     | |_____/   |   |______
 |_____/ |    \_ |_____| |       |_____ |_____| |    \_ __|__ ______|
					Making amazing tools since 2016

							 @Droploris
							 XaoticLabs
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Droploris.GetProxies.Services
{
	public abstract class Service
	{

		public virtual string GetServiceName() { return "Unnamed"; }
		public Service()
		{
		}

		public virtual string[] GetProxies() { return GetProxies(1); }
		public virtual string[] GetProxies(int page) { return null; }

	}
}
