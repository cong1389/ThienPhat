using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Language
{
    public interface ILocalizedModelLocal
    {
        int LanguageId { get; set; }
        int LocalesId { get; set; }
    }
}
