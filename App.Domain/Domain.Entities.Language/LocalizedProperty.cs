using App.Core.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace App.Domain.Entities.Language
{
	public class LocalizedProperty : AuditableEntity<int>
	{		
		public string LocaleValue
        {
			get;
			set;
		}

		[MaxLength(400)]		
		public string LocaleKey
        {
			get;
			set;
		}

		[MaxLength(400)]	
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