﻿using System.IO;
using System.Web.Mvc;

namespace Domas.Web.Tools.Services
{
	/// <summary>
	/// Reads view as a stream.
	/// </summary>
	public interface IViewStreamReader
	{
		Stream GetViewStream(string viewName, object model, ControllerContext context);
	}
}