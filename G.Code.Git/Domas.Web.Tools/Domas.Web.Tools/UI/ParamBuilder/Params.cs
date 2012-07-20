using System.Collections.Generic;
using Domas.Web.Tools.UI.ReturnUrl;

namespace Domas.Web.Tools.UI.ParamBuilder
{
	public class Params
	{
		public static ParamBuilder With
		{
			get { return new ParamBuilder(new ReturnUrlManager()); }
		}

		public static IDictionary<string, object> Empty
		{
			get
			{
				return new Dictionary<string, object>();
			}
		}
	}
}