using System;

using UIKit;
using Foundation;

namespace SKMapUtil
{
	public class BackgroundUrlEventArgs : System.EventArgs
	{
		public UIApplication application;
		public string sessionIdentifier;
		public Action completionHandler;

		public BackgroundUrlEventArgs(UIApplication application, string sessionIdentifier, Action completionHandler)
		{
			this.application = application;
			this.sessionIdentifier = sessionIdentifier;
			this.completionHandler = completionHandler;
		}
	}

	public interface IBackgroundUrlEventDispatcher
	{
		event EventHandler<BackgroundUrlEventArgs> HandleEventsForBackgroundUrlEvent;
	}
}

