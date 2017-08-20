using App.Core.Common;
using App.Domain.Entities.GlobalSetting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace App.Domain.Entities.GenericControl
{
	public class GenericControl : AuditableEntity<int>
	{
		public string Name
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public int? OrderDisplay
		{
			get;
			set;
		}

		public int Status
		{
			get;
			set;
		}

        public int EntityId
        {
            get;
            set;
        }        

        public GenericControl()
		{
		}

        [ForeignKey("EntityId")]
        public virtual ContactInformation ContactInfo
        {
            get;
            set;
        }

        public int ControlTypeId { get; set; }

        private ICollection<GenericControlValue> _genericControlValues;
        public virtual ICollection<GenericControlValue> GenericControlValues
        {
            get
            {
                ICollection<GenericControlValue> genericControlValues = this._genericControlValues;
                if (genericControlValues == null)
                {
                    List<GenericControlValue> genericControlValues1 = new List<GenericControlValue>();
                    ICollection<GenericControlValue> genericControlValues2 = genericControlValues1;
                    this._genericControlValues = genericControlValues1;
                    genericControlValues = genericControlValues2;
                }
                return genericControlValues;
            }
            set
            {
                this._genericControlValues = value;
            }
        }

    }
}