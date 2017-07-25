using App.Core.Common;
using App.Core.Utils;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Interfaces.Repository;
using App.Infra.Data.Common;
using App.Infra.Data.DbFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace App.Infra.Data.Repository.ContactInformation
{
	public class ContactInfoRepository : RepositoryBase<Domain.Entities.GlobalSetting.ContactInformation>, IContactInfoRepository, IRepositoryBase<Domain.Entities.GlobalSetting.ContactInformation>
	{
		public ContactInfoRepository(IDbFactory dbFactory) : base(dbFactory)
		{
		}

		public Domain.Entities.GlobalSetting.ContactInformation GetById(int Id)
		{
            Domain.Entities.GlobalSetting.ContactInformation ContactInformation = 
                this.FindBy((Domain.Entities.GlobalSetting.ContactInformation x) 
                => x.Id == Id, false).FirstOrDefault<Domain.Entities.GlobalSetting.ContactInformation>();
			return ContactInformation;
		}

		protected override IOrderedQueryable<Domain.Entities.GlobalSetting.ContactInformation> GetDefaultOrder(IQueryable<Domain.Entities.GlobalSetting.ContactInformation> query)
		{
            IOrderedQueryable<Domain.Entities.GlobalSetting.ContactInformation> ContactInformations = 
				from p in query
				orderby p.Id
				select p;
			return ContactInformations;
		}

		public IEnumerable<Domain.Entities.GlobalSetting.ContactInformation> PagedList(Paging page)
		{
			return this.GetAllPagedList(page).ToList();
		}

		public IEnumerable<Domain.Entities.GlobalSetting.ContactInformation> PagedSearchList(SortingPagingBuilder sortBuider, Paging page)
		{
            Expression<Func<Domain.Entities.GlobalSetting.ContactInformation, bool>> expression = PredicateBuilder.True<Domain.Entities.GlobalSetting.ContactInformation>();
			if (!string.IsNullOrEmpty(sortBuider.Keywords))
			{
				expression = expression.And((Domain.Entities.GlobalSetting.ContactInformation x) => x.Title.ToLower().Contains(sortBuider.Keywords.ToLower()) || x.Email.ToLower().Contains(sortBuider.Keywords.ToLower()) || x.Address.ToLower().Contains(sortBuider.Keywords.ToLower()) || x.MobilePhone.ToLower().Contains(sortBuider.Keywords.ToLower()) || x.Hotline.ToLower().Contains(sortBuider.Keywords.ToLower()) || x.Fax.ToLower().Contains(sortBuider.Keywords.ToLower()));
			}
			return this.FindAndSort(expression, sortBuider.Sorts, page);
		}
	}
}