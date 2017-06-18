using System;
using System.Globalization;
using System.Web.Mvc;

namespace App.Utils
{
	public class DoubleModelBinder : DefaultModelBinder
	{
		public DoubleModelBinder()
		{
		}

		public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			ValueProviderResult value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
			if (value == null)
			{
				return base.BindModel(controllerContext, bindingContext);
			}
			return Convert.ToDouble(value.AttemptedValue, CultureInfo.GetCultureInfo("en-US"));
		}
	}
}