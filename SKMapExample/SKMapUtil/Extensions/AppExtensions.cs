using System;

using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace SKMapUtil
{
	public class BackgroundUrlEventArgs : System.EventArgs
	{
		public UIApplication application;
		public string sessionIdentifier;
		public NSAction completionHandler;

		public BackgroundUrlEventArgs(UIApplication application, string sessionIdentifier, NSAction completionHandler)
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

