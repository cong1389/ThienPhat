using System;
using System.Web;
using System.Web.Routing;

public class NotNullRouteConstraint : IRouteConstraint
{
	public NotNullRouteConstraint()
	{
	}

	public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
	{
		return values[parameterName] != null;
	}
}