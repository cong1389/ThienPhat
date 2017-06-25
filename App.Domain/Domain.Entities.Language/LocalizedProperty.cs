using App.Core.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace App.Domain.Entities.Language
{
	public class LocalizedProperty : AuditableEntity<int>
	{
		[MaxLength(250)]
		public string LocaleValue
        {
			get;
			set;
		}

		[MaxLength(50)]
		[Required]
		public string LocaleKey
        {
			get;
			set;
		}

		[MaxLength(250)]
		[Required]
		public string LocaleKeyGroup
        {
			get;
			set;
		}

		public int EntityId
        {
			get;
			set;
		}

        public int LanguageId
        {
            get;
            set;
        }

        public LocalizedProperty()
		{
		}
	}
}