using App.Domain.Entities.Language;
using App.Domain.Interfaces.Services;

namespace App.Service.Common
{
    public interface IWorkContext 
    {
        App.Domain.Entities.Language.Language WorkingLanguage { get; set; }
    }
}