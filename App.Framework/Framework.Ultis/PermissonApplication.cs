using System;
using System.Web.Mvc;

namespace App.Framework.Ultis
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class PermissonApplication : AuthorizeAttribute
	{
		public PermissonApplication()
		{
		}
	}
}