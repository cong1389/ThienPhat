using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace App.Core.Common
{
	public abstract class BaseEntity
	{/// <summary>
     /// Gets or sets the entity identifier
     /// </summary>
        [DataMember]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        protected BaseEntity()
		{
		}
	}
}