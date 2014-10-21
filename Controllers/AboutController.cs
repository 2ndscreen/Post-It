using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using PostIt.Helpers;

namespace PostIt
{
	public partial class AboutController : DialogViewController
	{
		public AboutController () : base (UITableViewStyle.Grouped, null, true)
		{
		
		}

		private CheckboxElement facebook, twitter, sina, tencent, notifications;
		private RadioGroup color;
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			Root = new RootElement ("About") {
				new Section ("Social Networks") {
					(twitter = new CheckboxElement("Twitter", Settings.Twitter)),
					(facebook = new CheckboxElement("Facebook", Settings.Facebook)),
					(sina = new CheckboxElement("Sina Weibo", Settings.Sina)),
					(tencent = new CheckboxElement("Tencent Weibo", Settings.Tencent))
				},
				new Section("Customization"){ 
					new RootElement("Colors", 
						(color = new RadioGroup("color", Settings.Color))){ 
						new Section(){
							new RadioElement("Blue", "color"),
								new RadioElement("Dark Blue", "color"),
								new RadioElement("Gray", "color"),
								new RadioElement("Green", "color"),
								new RadioElement("Light Gray", "color"),
							new RadioElement("Pink (for @StefiSpice)", "color"),
								new RadioElement("Purple", "color"),
							}
					},
					(notifications = new CheckboxElement("Notifications", Settings.Notifications))
				},
				new Section ("Created") {
					new StringElement ("By @JamesMontemagno", () => {
						UIApplication.SharedApplication.OpenUrl(NSUrl.FromString("http://www.twitter.com/jamesmontemagno"));
					}),
					new StringElement ("In C# with Xamarin", () => {
						UIApplication.SharedApplication.OpenUrl(NSUrl.FromString("http://www.xamarin.com"));
					}),
					new StringElement ("Copyright 2014 Refractored LLC", () => {
						UIApplication.SharedApplication.OpenUrl(NSUrl.FromString("http://www.refractored.com"));
					}),
				},
				new Section ("Thank You") {
					new StringElement ("Copy by Stephanie Sparer", () => {
						UIApplication.SharedApplication.OpenUrl(NSUrl.FromString("http://www.twitter.com/stefispice"));
					}),
					new StringElement ("Icon by Seth D.", () => {
						UIApplication.SharedApplication.OpenUrl(NSUrl.FromString("http://www.fiverr.com/atomicbliss"));
					}),
				},
			};


		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			//tint uibutton so we see check marks
			UIButton.Appearance.TintColor = Color.Blue;

			switch (color.Selected) {
			case 0:
				NavigationController.NavigationBar.BarTintColor = Color.Blue;
				break;
			case 1: 
				NavigationController.NavigationBar.BarTintColor = Color.DarkBlue;
				break;
			case 2:
				NavigationController.NavigationBar.BarTintColor = Color.Gray;
				break;
			case 3:
				NavigationController.NavigationBar.BarTintColor = Color.Green;
				break;
			case 4:
				NavigationController.NavigationBar.BarTintColor = Color.LightGray;
				break;
			case 5:
				NavigationController.NavigationBar.BarTintColor = Color.Pink;
				break;
			case 6: 
				NavigationController.NavigationBar.BarTintColor = Color.Purple;
				break;
			};
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);

			if (notifications.Value && !Settings.Notifications) {
				if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
				{
					const UIUserNotificationType notificationUserTypes = UIUserNotificationType.Alert;
					UIUserNotificationSettings notificationSettings = UIUserNotificationSettings.GetSettingsForTypes(notificationUserTypes, null);
					UIApplication.SharedApplication.RegisterUserNotificationSettings(notificationSettings);
				}
			}

			Settings.Twitter = twitter.Value;
			Settings.Facebook = facebook.Value;
			Settings.Sina = sina.Value;
			Settings.Tencent = tencent.Value;
			Settings.Color = color.Selected;
			Settings.Notifications = notifications.Value;
		}
	}
}
