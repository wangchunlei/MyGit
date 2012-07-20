using System;
using System.Collections.Generic;

namespace Domas.Web.Tools.PortableAreas
{
	public interface IApplicationBus:IList<Type>
	{
		void Send(IEventMessage eventMessage);
		void SetMessageHandlerFactory(IMessageHandlerFactory factory);
	}
}