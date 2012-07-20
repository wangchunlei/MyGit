using System;

namespace Domas.Web.Tools.UI.InputBuilder.InputSpecification
{
	public class RenderInputBuilderException : Exception
	{
		public RenderInputBuilderException(string message, Exception innerException) : base(message, innerException) {}
	}
}