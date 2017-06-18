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
	public class ContactInfoRepository : RepositoryBase<ContactInfomation>, IContactInfoRepository, IRepositoryBase<ContactInfomation>
	{
		public ContactInfoRepository(IDbFactory dbFactory) : base(dbFactory)
		{
		}

		public ContactInfomation GetById(int Id)
		{
			ContactInfomation contactInfomation = this.FindBy((ContactInfomation x) => x.Id == Id, false).FirstOrDefault<ContactInfomation>();
			return contactInfomation;
		}

		protected override IOrderedQueryable<ContactInfomation> GetDefaultOrder(IQueryable<ContactInfomation> query)
		{
			IOrderedQueryable<ContactInfomation> contactInfomations = 
				from p in query
				orderby p.Id
				select p;
			return contactInfomations;
		}

		public IEnumerable<ContactInfomation> PagedList(Paging page)
		{
			return this.GetAllPagedList(page).ToList<ContactInfomation>();
		}

		public IEnumerable<ContactInfomation> PagedSearchList(SortingPagingBuilder sortBuider, Paging page)
		{
			Expression<Func<ContactInfomation, bool>> expression = PredicateBuilder.True<ContactInfomation>();
			if (!string.IsNullOrEmpty(sortBuider.Keywords))
			{
				expression = expression.And<ContactInfomation>((ContactInfomation x) => x.Title.ToLower().Contains(sortBuider.Keywords.ToLower()) || x.Email.ToLower().Contains(sortBuider.Keywords.ToLower()) || x.Address.ToLower().Contains(sortBuider.Keywords.ToLower()) || x.MobilePhone.ToLower().Contains(sortBuider.Keywords.ToLower()) || x.Hotline.ToLower().Contains(sortBuider.Keywords.ToLower()) || x.Fax.ToLower().Contains(sortBuider.Keywords.ToLower()));
			}
			return this.FindAndSort(expression, sortBuider.Sorts, page);
		}
	}
}