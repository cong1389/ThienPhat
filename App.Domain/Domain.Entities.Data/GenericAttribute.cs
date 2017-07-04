using App.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Entities.Data
{
    public class GenericAttribute : AuditableEntity<int>
    {
        public int EntityId
        {
            get;
            set;
        }

        [MaxLength(50)]
        public string KeyGroup
        {
            get;
            set;
        }

        [MaxLength(250)]
        public string Key
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }

        public int StoreId
        {
            get;
            set;
        }


        public GenericAttribute()
        {
        }
    }
}
