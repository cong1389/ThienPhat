using System;
using System.Web.Configuration;
using System.Web.Mvc;

namespace App.Front.Models
{
	public class PartialCacheAttribute : OutputCacheAttribute
	{
		public PartialCacheAttribute(string cacheProfileName)
		{
			OutputCacheProfile item = ((OutputCacheSettingsSection)WebConfigurationManager.GetSection("system.web/caching/outputCacheSettings")).OutputCacheProfiles[cacheProfileName];
			base.Duration = item.Duration;
			base.VaryByParam = item.VaryByParam;
		}
	}
}