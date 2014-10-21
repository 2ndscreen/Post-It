using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace PostIt
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow window;
		UINavigationController navigation;
		PostItController postItController; 
		//
		// This method is invoked when the application has loaded and is ready to run. In this
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// create a new window instance based on the screen size
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			UINavigationBar.Appearance.BackgroundColor = UIColor.FromRGBA(0,0,0,0);
			UINavigationBar.Appearance.TintColor = UIColor.White;
			UINavigationBar.Appearance.SetTitleTextAttributes (new UITextAttributes () {
				Font = UIFont.FromName (PostItController.FontLightName, 20f),
				TextColor = UIColor.White
			});
			UIButton.Appearance.TintColor = UIColor.White;

			// If you have defined a root view controller, set it here:
			// window.RootViewController = myViewController;
			postItController = new PostItController ();
			navigation = new UINavigationController (postItController);
			window.RootViewController = navigation;
			// make the window visible
			window.MakeKeyAndVisible ();

			// check for a notification
			if (options != null)
			{
				// check for a local notification
				if (options.ContainsKey(UIApplication.LaunchOptionsLocalNotificationKey))
				{
					var localNotification = options[UIApplication.LaunchOptionsLocalNotificationKey] as UILocalNotification;
					if (localNotification != null)
					{
						ProcessNotification (localNotification);
					}
				}
			}

			if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
			{
				const UIUserNotificationType notificationUserTypes = UIUserNotificationType.Alert;
				UIUserNotificationSettings notificationSettings = UIUserNotificationSettings.GetSettingsForTypes(notificationUserTypes, null);
				UIApplication.SharedApplication.RegisterUserNotificationSettings(notificationSettings);
			}

			return true;
		}

		public override void DidRegisterUserNotificationSettings (UIApplication application, UIUserNotificationSettings notificationSettings)
		{
			Helpers.Settings.Notifications = true;
		}
			

		public override void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
		{
			if (application.ApplicationState == UIApplicationState.Active)
				return;

			ProcessNotification (notification);
		}

		private void ProcessNotification(UILocalNotification notification)
		{
			if (postItController == null)
				return;


			if (notification.AlertBody.Contains ("Tweet"))
				postItController.SendSocial(MonoTouch.Social.SLServiceKind.Twitter);
			else if (notification.AlertBody.Contains ("Facebook"))
				postItController.SendSocial( MonoTouch.Social.SLServiceKind.Facebook);
			else if (notification.AlertBody.Contains ("Tencent"))
				postItController.SendSocial( MonoTouch.Social.SLServiceKind.Twitter);
			else if (notification.AlertBody.Contains ("Sina"))
				postItController.SendSocial( MonoTouch.Social.SLServiceKind.SinaWeibo);
		}
	}
}

