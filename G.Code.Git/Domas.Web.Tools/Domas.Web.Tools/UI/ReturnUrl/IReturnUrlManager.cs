namespace Domas.Web.Tools.UI.ReturnUrl
{
	public interface IReturnUrlManager
	{
		string GetReturnUrl();
		bool HasReturnUrl();
	}
}