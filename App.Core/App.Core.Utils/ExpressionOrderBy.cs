using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace App.Core.Utils
{
	public static class ExpressionOrderBy
	{
		private static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
		{
			string[] strArrays = property.Split(new char[] { '.' });
			Type propertyType = typeof(T);
			ParameterExpression parameterExpression = Expression.Parameter(propertyType, "x");
			Expression expression = parameterExpression;
			string[] strArrays1 = strArrays;
			for (int i = 0; i < (int)strArrays1.Length; i++)
			{
				PropertyInfo propertyInfo = propertyType.GetProperty(strArrays1[i]);
				expression = Expression.Property(expression, propertyInfo);
				propertyType = propertyInfo.PropertyType;
			}
			Type type = typeof(Func<,>).MakeGenericType(new Type[] { typeof(T), propertyType });
			LambdaExpression lambdaExpression = Expression.Lambda(type, expression, new ParameterExpression[] { parameterExpression });
			return (IOrderedQueryable<T>)typeof(Queryable).GetMethods().Single<MethodInfo>((MethodInfo method) => (!(method.Name == methodName) || !method.IsGenericMethodDefinition || (int)method.GetGenericArguments().Length != 2 ? false : (int)method.GetParameters().Length == 2)).MakeGenericMethod(new Type[] { typeof(T), propertyType }).Invoke(null, new object[] { source, lambdaExpression });
		}

		public static object GetPropertyValue(object obj, string property)
		{
			PropertyInfo propertyInfo = obj.GetType().GetProperty(property);
			return propertyInfo.GetValue(obj, null);
		}

		public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property)
		{
			return ExpressionOrderBy.ApplyOrder<T>(source, property, "OrderBy");
		}

		public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string property)
		{
			return ExpressionOrderBy.ApplyOrder<T>(source, property, "OrderByDescending");
		}

		public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string property)
		{
			return ExpressionOrderBy.ApplyOrder<T>(source, property, "ThenBy");
		}

		public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string property)
		{
			return ExpressionOrderBy.ApplyOrder<T>(source, property, "ThenByDescending");
		}
	}
}