using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace App.Core.Utils
{
	public static class PredicateBuilder
	{
		public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
		{
			Expression<Func<T, bool>> expression = first.Compose<Func<T, bool>>(second, new Func<Expression, Expression, Expression>(Expression.AndAlso));
			return expression;
		}

		private static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
		{
			Dictionary<ParameterExpression, ParameterExpression> dictionary = first.Parameters.Select((ParameterExpression f, int i) => new { f = f, s = second.Parameters[i] }).ToDictionary((p) => p.s, (p) => p.f);
			Expression expression = PredicateBuilder.ParameterRebinder.ReplaceParameters(dictionary, second.Body);
			Expression<T> expression1 = Expression.Lambda<T>(merge(first.Body, expression), first.Parameters);
			return expression1;
		}

		public static Expression<Func<T, bool>> Create<T>(Expression<Func<T, bool>> predicate)
		{
			return predicate;
		}

		public static Expression<Func<T, bool>> False<T>()
		{
			return (T param) => false;
		}

		public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expression)
		{
			UnaryExpression unaryExpression = Expression.Not(expression.Body);
			return Expression.Lambda<Func<T, bool>>(unaryExpression, expression.Parameters);
		}

		public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
		{
			Expression<Func<T, bool>> expression = first.Compose<Func<T, bool>>(second, new Func<Expression, Expression, Expression>(Expression.OrElse));
			return expression;
		}

		public static Expression<Func<T, bool>> True<T>()
		{
			return (T param) => true;
		}

		private class ParameterRebinder : ExpressionVisitor
		{
			private readonly Dictionary<ParameterExpression, ParameterExpression> map;

			private ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
			{
				this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
			}

			public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
			{
				return (new PredicateBuilder.ParameterRebinder(map)).Visit(exp);
			}

			protected override Expression VisitParameter(ParameterExpression p)
			{
				ParameterExpression parameterExpression;
				if (this.map.TryGetValue(p, out parameterExpression))
				{
					p = parameterExpression;
				}
				return base.VisitParameter(p);
			}
		}
	}
}