using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using MonoTouch.Social;
using PostIt.Helpers;

namespace PostIt
{
	public partial class PostItController : UIViewController
	{
		private	AboutController about;
		public PostItController ()
		{

			this.Title = "Post It";

		}



		private UIButton facebook, twitter, tencent, sina;

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			View.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
			NavigationController.NavigationBar.BarStyle = UIBarStyle.Black;

			about = new AboutController ();

			var infoButton = UIButton.FromType (UIButtonType.InfoLight);
			infoButton.TintColor = UIColor.White;

			infoButton.AddTarget ((s, e) => {
				NavigationController.PushViewController(about, true);
			}, UIControlEvent.TouchUpInside);

			var infoBarButton = new UIBarButtonItem (infoButton);

			NavigationItem.SetRightBarButtonItem (infoBarButton, true);

			twitter = UIButton.FromType (UIButtonType.System);
			twitter.Font = FontToUse;
			twitter.SetTitle ("Tweet", UIControlState.Normal);


			facebook = UIButton.FromType (UIButtonType.System);
			facebook.Font = FontToUse;
			facebook.SetTitle ("Post", UIControlState.Normal);

			tencent = UIButton.FromType (UIButtonType.System);
			tencent.Font = FontToUse;
			tencent.SetTitle ("Tencent", UIControlState.Normal);

			sina = UIButton.FromType (UIButtonType.System);
			sina.Font = FontToUse;
			sina.SetTitle ("Sina", UIControlState.Normal);

			facebook.TouchUpInside += (sender, e) => {
				SendSocial(SLServiceKind.Facebook);
			};

			twitter.TouchUpInside += (sender, e) => {
				SendSocial(SLServiceKind.Twitter);
			};

			tencent.TouchUpInside += (sender, e) => {
				SendSocial(SLServiceKind.TencentWeibo);
			};

			sina.TouchUpInside += (sender, e) => {
				SendSocial(SLServiceKind.SinaWeibo);
			};

			facebook.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleTopMargin |
				UIViewAutoresizing.FlexibleBottomMargin;
			twitter.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleTopMargin |
				UIViewAutoresizing.FlexibleBottomMargin;
			sina.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleTopMargin |
				UIViewAutoresizing.FlexibleBottomMargin;
			tencent.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleTopMargin |
				UIViewAutoresizing.FlexibleBottomMargin;
		}

		private int padding = UIDevice.CurrentDevice.UserInterfaceIdiom ==
			UIUserInterfaceIdiom.Pad ? 30 : 15;

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			UIApplication.SharedApplication.CancelAllLocalNotifications ();

			UIButton.Appearance.TintColor = UIColor.White;
			facebook.RemoveFromSuperview ();
			twitter.RemoveFromSuperview ();
			sina.RemoveFromSuperview ();
			tencent.RemoveFromSuperview ();
			int total = 0;
			if (Settings.Tencent)
				total++;
			if (Settings.Sina)
				total++;
			if (Settings.Twitter)
				total++;
			if (Settings.Facebook)
				total++;

			var top = (View.Frame.Height / 2) - NavigationController.NavigationBar.Frame.Height; //center of screen

			top = top - (45 * (total - 1)) + padding;

			if (Settings.Twitter) {
				twitter.Frame = new System.Drawing.RectangleF (0, top, View.Frame.Width, 80);
				top = twitter.Frame.Bottom + padding;
				View.Add (twitter);
			}

			if (Settings.Facebook) {
				facebook.Frame = new System.Drawing.RectangleF (0, top, View.Frame.Width, 80);
				top = facebook.Frame.Bottom + padding;
				View.Add (facebook);
			}

			if (Settings.Sina) {
				sina.Frame = new System.Drawing.RectangleF (0, top, View.Frame.Width, 80);
				top = sina.Frame.Bottom + padding;
				View.Add (sina);
			}

			if (Settings.Tencent) {
				tencent.Frame = new System.Drawing.RectangleF (0, top, View.Frame.Width, 80);
				top = tencent.Frame.Bottom + padding;
				View.Add (tencent);
				CreateNotification (SLServiceKind.TencentWeibo);
			}

			if(Settings.Sina)
				CreateNotification (SLServiceKind.SinaWeibo);
			if(Settings.Facebook)
				CreateNotification (SLServiceKind.Facebook);
			if(Settings.Twitter)
				CreateNotification (SLServiceKind.Twitter);



			switch (Settings.Color) {
			case 0:
				View.BackgroundColor = Color.Blue;
				NavigationController.NavigationBar.BarTintColor = Color.Blue;
				break;
			case 1: 
				View.BackgroundColor = Color.DarkBlue;
				NavigationController.NavigationBar.BarTintColor = Color.DarkBlue;
				break;
			case 2:
				View.BackgroundColor = Color.Gray;
				NavigationController.NavigationBar.BarTintColor = Color.Gray;
				break;
			case 3:
				View.BackgroundColor = Color.Green;
				NavigationController.NavigationBar.BarTintColor = Color.Green;
				break;
			case 4:
				View.BackgroundColor = Color.LightGray;
				NavigationController.NavigationBar.BarTintColor = Color.LightGray;
				break;
			case 5:
				View.BackgroundColor = Color.Pink;
				NavigationController.NavigationBar.BarTintColor = Color.Pink;
				break;
			case 6: 
				View.BackgroundColor = Color.Purple;
				NavigationController.NavigationBar.BarTintColor = Color.Purple;
				break;
			};
				
		}


		public void SendSocial(SLServiceKind framework)
		{

			var slComposer = SLComposeViewController.FromService(framework);
			if (slComposer == null) {
				new UIAlertView ("Unavailable", "This social network is not available.", null, "OK").Show();
				return;
			}

			slComposer.CompletionHandler += (result) => {
				InvokeOnMainThread (() => {
					DismissViewController (true, null);
				});
			};
			PresentViewController(slComposer, true, null);
		}

		public static UIFont FontToUse = UIFont.FromName(FontLightName,
			UIDevice.CurrentDevice.UserInterfaceIdiom ==
			UIUserInterfaceIdiom.Pad ? 60f : 40f); 

		public static string FontLightName
		{
			get
			{
				return "HelveticaNeue-Light";
			}
		}

		private void CreateNotification(SLServiceKind framework)
		{
			// create the notification
			var notification = new UILocalNotification ();
			// configure the alert stuff
			notification.AlertAction = "Post It";
			switch (framework) {
			case SLServiceKind.Facebook:
				notification.AlertBody = "Facebook Post";
				break;
			case SLServiceKind.Twitter:
				notification.AlertBody = "Tweet";
				break;
			case SLServiceKind.SinaWeibo:
				notification.AlertBody = "Sina Weibo Post";
				break;
			case SLServiceKind.TencentWeibo:
				notification.AlertBody = "Tencent Weibo Post";
				break;
			}

			// schedule it
			UIApplication.SharedApplication.PresentLocationNotificationNow (notification);
		}
	}
}
