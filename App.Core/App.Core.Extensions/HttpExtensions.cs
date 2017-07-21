using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace App.Core.Extensions
{
    public static class HttpExtensions
    {
        public static ControllerContext GetMasterControllerContext(this ControllerContext controllerContext)
        {
            var ctx = controllerContext;

            while (ctx.ParentActionViewContext != null)
            {
                ctx = ctx.ParentActionViewContext;
            }

            return ctx;
        }
    }
}
