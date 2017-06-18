using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace App.Core.Utils
{
	public class PredicateExtention
	{
		private readonly static MethodInfo StringContainsMethod;

		private readonly static MethodInfo BooleanEqualsMethod;

		static PredicateExtention()
		{
			PredicateExtention.StringContainsMethod = typeof(string).GetMethod("Contains", BindingFlags.Instance | BindingFlags.Public, null, new Type[] { typeof(string) }, null);
			PredicateExtention.BooleanEqualsMethod = typeof(bool).GetMethod("Equals", BindingFlags.Instance | BindingFlags.Public, null, new Type[] { typeof(bool) }, null);
		}

		public PredicateExtention()
		{
		}

		private static Expression<Func<TDbType, bool>> ApplyBoolCriterion<TDbType, TSearchCriteria>(TSearchCriteria searchCriteria, PropertyInfo searchCriterionPropertyInfo, Type dbType, MemberInfo dbFieldMemberInfo, Expression<Func<TDbType, bool>> predicate)
		{
			Expression<Func<TDbType, bool>> expression;
			bool? value = (bool?)(searchCriterionPropertyInfo.GetValue(searchCriteria) as bool?);
			if (value.HasValue)
			{
				ParameterExpression parameterExpression = Expression.Parameter(dbType, "x");
				MemberExpression memberExpression = Expression.MakeMemberAccess(parameterExpression, dbFieldMemberInfo);
				Expression[] expressionArray = new Expression[] { Expression.Constant(value) };
				MethodCallExpression methodCallExpression = Expression.Call(memberExpression, PredicateExtention.BooleanEqualsMethod, expressionArray);
				Expression<Func<TDbType, bool>> expression1 = Expression.Lambda(methodCallExpression, new ParameterExpression[] { parameterExpression }) as Expression<Func<TDbType, bool>>;
				expression = predicate.And<TDbType>(expression1);
			}
			else
			{
				expression = predicate;
			}
			return expression;
		}

		private static Expression<Func<TDbType, bool>> ApplyStringCriterion<TDbType, TSearchCriteria>(TSearchCriteria searchCriteria, PropertyInfo searchCriterionPropertyInfo, Type dbType, MemberInfo dbFieldMemberInfo, Expression<Func<TDbType, bool>> predicate)
		{
			Expression<Func<TDbType, bool>> expression;
			string value = searchCriterionPropertyInfo.GetValue(searchCriteria) as string;
			if (!string.IsNullOrWhiteSpace(value))
			{
				ParameterExpression parameterExpression = Expression.Parameter(dbType, "x");
				MemberExpression memberExpression = Expression.MakeMemberAccess(parameterExpression, dbFieldMemberInfo);
				Expression[] expressionArray = new Expression[] { Expression.Constant(value) };
				MethodCallExpression methodCallExpression = Expression.Call(memberExpression, PredicateExtention.StringContainsMethod, expressionArray);
				Expression<Func<TDbType, bool>> expression1 = Expression.Lambda(methodCallExpression, new ParameterExpression[] { parameterExpression }) as Expression<Func<TDbType, bool>>;
				expression = predicate.And<TDbType>(expression1);
			}
			else
			{
				expression = predicate;
			}
			return expression;
		}

		public static Expression<Func<TDbType, bool>> BuildPredicate<TDbType, TSearchCriteria>(TSearchCriteria searchCriteria)
		{
			Expression<Func<TDbType, bool>> expression = PredicateBuilder.True<TDbType>();
			PropertyInfo[] properties = searchCriteria.GetType().GetProperties();
			for (int i = 0; i < (int)properties.Length; i++)
			{
				PropertyInfo propertyInfo = properties[i];
				string dbFieldName = PredicateExtention.GetDbFieldName(propertyInfo);
				Type type = typeof(TDbType);
				MemberInfo memberInfo = type.GetMember(dbFieldName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public).Single<MemberInfo>();
				if (propertyInfo.PropertyType == typeof(string))
				{
					expression = PredicateExtention.ApplyStringCriterion<TDbType, TSearchCriteria>(searchCriteria, propertyInfo, type, memberInfo, expression);
				}
				else if (propertyInfo.PropertyType == typeof(bool?))
				{
					expression = PredicateExtention.ApplyBoolCriterion<TDbType, TSearchCriteria>(searchCriteria, propertyInfo, type, memberInfo, expression);
				}
			}
			return expression;
		}

		private static string GetDbFieldName(PropertyInfo propertyInfo)
		{
			object obj = propertyInfo.GetCustomAttributes(typeof(DbFieldMapAttribute), false).FirstOrDefault<object>();
			return (obj != null ? ((DbFieldMapAttribute)obj).Field : propertyInfo.Name);
		}
	}
}