using Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace App.FakeEntity.GenericControl
{
	public class GenericControlValueViewModel
	{
		public GenericControlViewModel GenericControl
		{
			get;
			set;
		}

		[Display(Name="GenericControlName", ResourceType=typeof(FormUI))]
		public int GenericControlId
		{
			get;
			set;
		}

		[Display(Name="ColorHex", ResourceType=typeof(FormUI))]
		public string ColorHex
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

		[Display(Name="ValueName", ResourceType=typeof(FormUI))]
		public string ValueName
		{
			get;
			set;
		}

		public GenericControlValueViewModel()
		{
		}
	}
}