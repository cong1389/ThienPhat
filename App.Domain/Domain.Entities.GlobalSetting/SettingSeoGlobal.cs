using App.Core.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace App.Domain.Entities.GlobalSetting
{
	public class SettingSeoGlobal : AuditableEntity<int>
	{
		public string FacebookRetargetSnippet
		{
			get;
			set;
		}

		public string FbAdminsId
		{
			get;
			set;
		}

		[MaxLength(150)]
		public string FbAppId
		{
			get;
			set;
		}

		public string GoogleRetargetSnippet
		{
			get;
			set;
		}

		public string MetaTagMasterTool
		{
			get;
			set;
		}

		public string PublisherGooglePlus
		{
			get;
			set;
		}

		public string SnippetGoogleAnalytics
		{
			get;
			set;
		}

		public int Status
		{
			get;
			set;
		}

		public SettingSeoGlobal()
		{
		}
	}
}