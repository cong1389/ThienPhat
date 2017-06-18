using App.Utils;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Web;

namespace App.FileUtil
{
	public static class ExcelUtil
	{
		public static void ListToExcel<T>(List<T> query)
		{
			using (ExcelPackage excelPackage = new ExcelPackage())
			{
				string str = DateTime.Now.ToString("dd-MM-yyyy");
				ExcelWorksheet name = excelPackage.Workbook.Worksheets.Add(str);
				PropertyInfo[] properties = typeof(T).GetProperties();
				for (int i = 0; i < properties.Count<PropertyInfo>(); i++)
				{
					object[] customAttributes = properties[i].GetCustomAttributes(typeof(DisplayAttribute), true);
					if (customAttributes.Length == 0)
					{
						name.Cells[1, i + 1].Value = properties[i].Name;
					}
					else
					{
						string name1 = ((DisplayAttribute)customAttributes[0]).Name;
						name.Cells[1, i + 1].Value = name1;
					}
				}
				if (query.IsAny<T>())
				{
					name.Cells["A2"].LoadFromCollection<T>(query);
				}
				using (ExcelRange item = name.Cells["A1:BZ1"])
				{
					item.Style.Font.Bold = true;
					item.Style.Fill.PatternType = ExcelFillStyle.Solid;
					item.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));
					item.Style.Font.Color.SetColor(Color.White);
				}
				HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
				HttpContext.Current.Response.AddHeader("content-disposition", string.Concat("attachment;  filename=", str, ".xlsx"));
				HttpContext.Current.Response.BinaryWrite(excelPackage.GetAsByteArray());
				HttpContext.Current.Response.End();
			}
		}
	}
}