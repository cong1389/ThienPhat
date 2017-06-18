using System.Web;
using System.Web.Mvc;

namespace App.Front
{
    public class FilterConfig
    {
        public FilterConfig()
        {
        }
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
