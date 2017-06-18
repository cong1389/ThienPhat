using System;
using System.ComponentModel.DataAnnotations;

namespace App.Framework.Ultis
{
	public enum EnumStatus
	{
		Success = 0,
		[Display(Name="This email is already in use.")]
		EmailDuplicate = 1,
		[Display(Name="This user name is already in use.")]
		UserNameDuplicate = 1
	}
}