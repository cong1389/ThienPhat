using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Common
{
    public class CommonServices : ICommonServices
    {
        private readonly IWorkContext _workContext;

        public CommonServices(IWorkContext workContext)
        {
            this._workContext = workContext;
        }

        public IWorkContext WorkContext
        {
            get
            {
                return _workContext;
            }

        }
    }
}
