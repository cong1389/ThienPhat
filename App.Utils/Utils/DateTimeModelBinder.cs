using System;
using System.Globalization;
using System.Web.Mvc;

namespace App.Utils
{
	public class DateTimeModelBinder : DefaultModelBinder
	{
		public DateTimeModelBinder()
		{
		}

		public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			DateTime dateTime;
			ValueProviderResult value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
			if (value == null || string.IsNullOrEmpty(value.AttemptedValue))
			{
				return base.BindModel(controllerContext, bindingContext);
			}
			string str = "MM/dd/yyyy";
			DateTime.TryParseExact(value.AttemptedValue, str, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
			return dateTime;
		}
	}
}