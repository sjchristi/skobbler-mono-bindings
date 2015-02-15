using System;
using MonoTouch.Dialog;
using UIKit;

namespace SKMapUtil
{
	public class OfflineMapInfoElement : StyledStringElement
	{
		protected OfflineMapInfo _mapInfo;
		protected WeakReference<DialogViewController> _parentVC;

		public OfflineMapInfoElement (DialogViewController parentVC, OfflineMapInfo info)
			: base (info.LocalizedDescription, "", UITableViewCellStyle.Subtitle)
		{
			this._parentVC = new WeakReference<DialogViewController>(parentVC);
			this._mapInfo = info;

			this.Value = this.GetValueForCurrentState ();

			if (this._mapInfo.children.Count > 0) {
				this.Accessory = UITableViewCellAccessory.DisclosureIndicator;
			}

			this.Tapped += CellTapped;

			this._mapInfo.MapInfoChanged += OnMapInfoChanged;
		}

		protected DialogViewController ParentVC {
			get {
				DialogViewController vc;

				this._parentVC.TryGetTarget (out vc);

				return vc;
			}
		}

		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);

			if (this._mapInfo != null) {
				this._mapInfo.MapInfoChanged -= OnMapInfoChanged;
				this._mapInfo = null;
			}
		}

		protected void OnMapInfoChanged(object sender, OfflineMapInfo e)
		{
			this.RefreshCell ();
		}

		public override UITableViewCell GetCell (UITableView tv)
		{
			UITableViewCell cell = base.GetCell (tv);

			cell.TextLabel.Lines = 1;
			cell.TextLabel.AdjustsFontSizeToFitWidth = true;

			cell.DetailTextLabel.Lines = 1;
			cell.DetailTextLabel.AdjustsFontSizeToFitWidth = true;

			return cell;
		}

		public void RefreshCell()
		{
			this.ParentVC.BeginInvokeOnMainThread (() => {
				this.Value = this.GetValueForCurrentState ();

				var root = this.GetImmediateRootElement ();

				if (root != null) {
					root.Reload (this, UITableViewRowAnimation.None);
				} else {
					// ghetto.  must be a better way?
					// https://stackoverflow.com/questions/27283567/how-do-you-refresh-a-monotouch-dialog-element-when-using-a-search-filter
					this.ParentVC.ReloadData ();
				}
			});
		}

		protected string GetValueForCurrentState() {
			string value = null;
			double size = (double)this._mapInfo.DownloadSizeBytes / (1024.0 * 1024.0);

			switch (this._mapInfo.State) {
			case OfflineMapInfo.PackageState.NotInstalled:
				{
					if (this._mapInfo.children.Count > 0) {
						value = "";
					} else {
						value = string.Format ("{0} MB | Tap to Install", size.ToString("N1"));
					}
					break;
				}
			case OfflineMapInfo.PackageState.Installing:
				{
					string progress = this._mapInfo.InstallProgress;
					value = string.Format ("{0} | Installing...", progress);
					break;
				}
			case OfflineMapInfo.PackageState.Installed:
				{
					value = string.Format ("{0} MB | Installed", size.ToString ("N1"));
					break;
				}
			case OfflineMapInfo.PackageState.InstallError:
				{
					value = "Error installing; tap to retry.";
					break;
				}
			case OfflineMapInfo.PackageState.InstallCancelled:
				{
					value = "Installation cancelled; tap to install.";
					break;
				}
			}

			return value;
		}

		protected async void CellTapped()
		{
			Console.WriteLine ("Selected {0}", this._mapInfo.LocalizedName);
			switch (this._mapInfo.State) {
			case OfflineMapInfo.PackageState.NotInstalled:
				{
					if (this._mapInfo.children.Count > 0) {
						SKMapUtilOfflineMapInfoViewController vc = new SKMapUtilOfflineMapInfoViewController (this._mapInfo);

						this.ParentVC.NavigationController.PushViewController (vc, true);
					} else {
						string message = string.Format ("Would you like to install {0}?", this._mapInfo.LocalizedName);
						nint button = await DialogExtensions.ShowAlertAsync ("", message, "OK", "Cancel");

						if (button == 0) {
							this._mapInfo.DownloadAndInstallPackage ();
						}
					}
					break;
				}
			case OfflineMapInfo.PackageState.Installing:
				{
					string message = string.Format ("Would you like to cancel installation of {0}?", this._mapInfo.LocalizedName);
					nint button = await DialogExtensions.ShowAlertAsync ("", message, "OK", "Cancel");

					if (button == 0) {
						this._mapInfo.CancelInstall ();
					}
					break;
				}
			case OfflineMapInfo.PackageState.Installed:
				{
					string message = string.Format ("Would you like to uninstall {0}?", this._mapInfo.LocalizedName);
					nint button = await DialogExtensions.ShowAlertAsync ("", message, "OK", "Cancel");

					if (button == 0) {
						bool uninstalled = this._mapInfo.UninstallMapPackage ();
						Console.WriteLine ("{0} - uninstalled map {1}", uninstalled, this._mapInfo.LocalizedName);
					}
					break;
				}
			case OfflineMapInfo.PackageState.InstallError:
				{
					string message = string.Format ("There was an error installing the map {0}.  Would you like to try again?", this._mapInfo.LocalizedName);
					nint button = await DialogExtensions.ShowAlertAsync ("", message, "OK", "Error Details", "Cancel");

					if (button == 0) {
						this._mapInfo.DownloadAndInstallPackage ();
					} else if (button == 1) {
						string errordetails;
						if (this._mapInfo.LastError is System.Net.WebException) {
							System.Net.WebException webError = this._mapInfo.LastError as System.Net.WebException;
							errordetails = string.Format ("Network Error: {0}.  Check your internet connection and try again.\n\n{1}", webError.Status.ToString(), this._mapInfo.LastError.StackTrace);
						} else {
							errordetails = string.Format ("{0}\n\n{1}", this._mapInfo.LastError.Message, this._mapInfo.LastError.StackTrace);
						}
						DialogExtensions.ShowAlert ("", errordetails, "OK");
					} else {
						this._mapInfo.SetState (OfflineMapInfo.PackageState.NotInstalled);
					}
					break;
				}
			case OfflineMapInfo.PackageState.InstallCancelled:
				{
					string message = string.Format ("Would you like to install {0}?", this._mapInfo.LocalizedName);
					nint button = await DialogExtensions.ShowAlertAsync ("", message, "OK", "Cancel");

					if (button == 0) {
						this._mapInfo.DownloadAndInstallPackage ();
					} else {
						this._mapInfo.SetState (OfflineMapInfo.PackageState.NotInstalled);
					}
					break;
				}
			}
		}

		public override bool Matches (string text)
		{
			// Note: the base class will match the caption and value (which could say "tap to install"), so don't use it..
			return this._mapInfo.Matches (text);
		}
	}
}

