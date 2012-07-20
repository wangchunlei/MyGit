using System.Web.Routing;

namespace Domas.Web.Tools.SimplyRestful
{
	public interface IRestfulActionResolver
	{
		RestfulAction ResolveAction(RequestContext context);
	}
}
