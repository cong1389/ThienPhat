using Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace App.FakeEntity.GenericControl
{
	public class GenericControlViewModel
	{
		[Display(Name="GenericControlName", ResourceType=typeof(FormUI))]
		public string GenericControlName
		{
			get;
			set;
		}

		[Display(Name="Description", ResourceType=typeof(FormUI))]
		public string Description
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		[Display(Name="OrderDisplay", ResourceType=typeof(FormUI))]
		public int? OrderDisplay
		{
			get;
			set;
		}

		[Display(Name="Status", ResourceType=typeof(FormUI))]
		public int Status
		{
			get;
			set;
		}

        [Display(Name = "Entity", ResourceType = typeof(FormUI))]
        public int EntityId
        {
            get;
            set;
        }
        public GenericControlViewModel()
		{
		}
	}
}