using App.Core.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Language
{
    public interface ITextService
    {
        LocalizedString Get(string key, params object[] args);
    }
}
