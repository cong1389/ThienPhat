using App.Core.Common;
using App.Domain.Entities.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Domain.Entities.GenericControl
{
    public class GenericControlValue : AuditableEntity<int>
	{
        [ForeignKey("GenericControlId")]
        public virtual GenericControl GenericControl
		{
			get;
			set;
		}

		public int GenericControlId
		{
			get;
			set;
		}

		public string ColorHex
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		//public virtual ICollection<GalleryImage> GalleryImages
		//{
		//	get;
		//	set;
		//}

		public int? OrderDisplay
		{
			get;
			set;
		}
        public int? EntityId
        {
            get;
            set;
        }      

		public int Status
		{
			get;
			set;
		}

		public string ValueName
		{
			get;
			set;
		}

		public GenericControlValue()
		{
		}
	}
}