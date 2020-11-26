using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Ads;
using Android.OS;
using Android.Support.V7.App;
using Java.Lang;
using QuickDate.Activities.Default;
using QuickDate.Activities.SharedPreferences;
using QuickDate.Activities.Tabbes;
using QuickDate.Helpers.Ads;
using QuickDate.Helpers.Controller;
using QuickDate.Helpers.Model;
using QuickDate.SQLite;
using QuickDateClient;
using Exception = System.Exception;

namespace QuickDate.Activities
{
    [Activity(MainLauncher = true, Icon = "@mipmap/icon", Theme = "@style/SplashScreenTheme", NoHistory = true, ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class SplashScreenActivity : AppCompatActivity
    {
        #region Variables Basic

        private SqLiteDatabase DbDatabase;

        #endregion
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            LoadAppSettings();
        }

        protected override void OnResume()
        {
            try
            {
                base.OnResume();
                DbDatabase = new SqLiteDatabase();
                DbDatabase.CheckTablesStatus();
                 
                new Handler(Looper.MainLooper).Post(new Runnable(FirstRunExcite)); 
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void LoadAppSettings()
        {
            var appPref = new AppPreferences();
            AppSettings.ShowWalkTroutPage = appPref.GetShowWalkThroughPageValue();
        }

        private void FirstRunExcite()
        {
            try
            {
                DbDatabase = new SqLiteDatabase();
                DbDatabase.CheckTablesStatus();

                if (!string.IsNullOrEmpty(AppSettings.Lang))
                {
                    UserDetails.LangName = AppSettings.Lang;
                    LangController.SetApplicationLang(Application.Context, AppSettings.Lang);
                }
                else
                {
                    UserDetails.LangName = Resources.Configuration.Locale.Language.ToLower();
                    LangController.SetApplicationLang(Application.Context, UserDetails.LangName);
                }

                PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => ApiRequest.GetSettings_Api(this) });

                var result = DbDatabase.Get_data_Login_Credentials();
                if (result != null)
                {
                    Current.AccessToken = result.AccessToken; 
                    switch (result.Status)
                    {
                        case "Active":
                        case "Pending":
                            StartActivity(new Intent(this, typeof(HomeActivity)));
                            break;
                        default:
                            StartActivity(new Intent(this, typeof(FirstActivity)));
                            break;
                    }
                }
                else
                {
                    StartActivity(new Intent(this, typeof(FirstActivity)));
                }

                DbDatabase.Dispose();

                if (AppSettings.ShowAdMobBanner || AppSettings.ShowAdMobInterstitial || AppSettings.ShowAdMobRewardVideo || AppSettings.ShowAdMobNative)
                    MobileAds.Initialize(this, GetString(Resource.String.admob_app_id));

                if (AppSettings.ShowFbBannerAds || AppSettings.ShowFbInterstitialAds || AppSettings.ShowFbRewardVideoAds)
                    InitializeFacebook.Initialize(this);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
         
        public override void OnLowMemory()
        {
            try
            {
                GC.Collect(GC.MaxGeneration);
                base.OnLowMemory();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public override void OnTrimMemory(TrimMemory level)
        {
            try
            {

                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
                base.OnTrimMemory(level);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    
    }
}