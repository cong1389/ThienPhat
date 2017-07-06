using App.Core.Utils;
using App.Domain.Interfaces.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Service.GenericAttribute
{
    public interface IGenericAttributeService : IBaseService<App.Domain.Entities.Data.GenericAttribute>, IService
    {
        void CreateGenericAttribute(App.Domain.Entities.Data.GenericAttribute attribute);

        App.Domain.Entities.Data.GenericAttribute GetGenericAttributeById(int Id);

        App.Domain.Entities.Data.GenericAttribute GetGenericAttributeByKey(int entityId, string keyGroup, string key);

        IEnumerable<App.Domain.Entities.Data.GenericAttribute> PagedList(SortingPagingBuilder sortBuider, Paging page);
        
        int SaveGenericAttribute();
    }
}