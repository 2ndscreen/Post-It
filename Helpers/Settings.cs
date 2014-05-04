// Helpers/Settings.cs
using Refractored.Xam.Settings;
using Refractored.Xam.Settings.Abstractions;

namespace PostIt.Helpers
{
  /// <summary>
  /// This is the Settings static class that can be used in your Core solution or in any
  /// of your client applications. All settings are laid out the same exact way with getters
  /// and setters. 
  /// </summary>
  public static class Settings
  {
    private static ISettings AppSettings
    {
      get
      {
        return CrossSettings.Current;
      }
    }

    #region Setting Constants

		private const string ColorKey = "color";
		private static readonly int ColorDefault = 1;

		private const string TwitterKey = "twitter";
		private static readonly bool TwitterDefault = true;

		private const string FacebookKey = "facebook";
		private static readonly bool FacebookDefault = true;

		private const string TencentKey = "tencent";
		private static readonly bool TencentDefault = false;

		private const string SinaKey = "sina";
		private static readonly bool SinaDefault = false;

		private const string NotificationsKey = "notfications";
		private static readonly bool NotificationsDefault = true;


    #endregion


		public static int Color
    {
      get
      {
				return AppSettings.GetValueOrDefault(ColorKey, ColorDefault);
      }
      set
      {
        //if value has changed then save it!
				if (AppSettings.AddOrUpdateValue(ColorKey, value))
          AppSettings.Save();
      }
    }

		public static bool Facebook
		{
			get
			{
				return AppSettings.GetValueOrDefault(FacebookKey, FacebookDefault);
			}
			set
			{
				//if value has changed then save it!
				if (AppSettings.AddOrUpdateValue(FacebookKey, value))
					AppSettings.Save();
			}
		}

		public static bool Twitter
		{
			get
			{
				return AppSettings.GetValueOrDefault(TwitterKey, TwitterDefault);
			}
			set
			{
				//if value has changed then save it!
				if (AppSettings.AddOrUpdateValue(TwitterKey, value))
					AppSettings.Save();
			}
		}

		public static bool Sina
		{
			get
			{
				return AppSettings.GetValueOrDefault(SinaKey, SinaDefault);
			}
			set
			{
				//if value has changed then save it!
				if (AppSettings.AddOrUpdateValue(SinaKey, value))
					AppSettings.Save();
			}
		}

		public static bool Tencent
		{
			get
			{
				return AppSettings.GetValueOrDefault(TencentKey, TencentDefault);
			}
			set
			{
				//if value has changed then save it!
				if (AppSettings.AddOrUpdateValue(TencentKey, value))
					AppSettings.Save();
			}
		}

		public static bool Notifications
		{
			get
			{
				return AppSettings.GetValueOrDefault(NotificationsKey, NotificationsDefault);
			}
			set
			{
				//if value has changed then save it!
				if (AppSettings.AddOrUpdateValue(NotificationsKey, value))
					AppSettings.Save();
			}
		}


  }
}