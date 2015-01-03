using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using System.Threading.Tasks;

namespace SKMapUtil
{
	public static class DialogExtensions
	{
		// Displays a UIAlertView and returns the index of the button pressed.
		public static Task<int> ShowAlertAsync (string title, string message, params string [] buttons)
		{
			var tcs = new TaskCompletionSource<int> ();
			var alert = new UIAlertView {
				Title = title,
				Message = message
			};
			foreach (var button in buttons)
				alert.AddButton (button);
			alert.Clicked += (s, e) => tcs.TrySetResult (e.ButtonIndex);
			alert.Show ();
			return tcs.Task;
		}

		public static UIAlertView ShowAlert (string title, string message, params string [] buttons)
		{
			var alert = new UIAlertView {
				Title = title,
				Message = message
			};
			foreach (var button in buttons)
				alert.AddButton (button);
			alert.Show ();
			return alert;
		}
	}
}

