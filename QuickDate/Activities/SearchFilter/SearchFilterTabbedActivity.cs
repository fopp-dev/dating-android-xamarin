using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using QuickDate.Activities.SearchFilter.Fragment;
using QuickDate.Activities.Tabbes;
using QuickDate.Adapters;
using QuickDate.Helpers.Ads;
using QuickDate.Helpers.Controller;
using QuickDate.Helpers.Model;
using QuickDate.Helpers.Utils;
using QuickDate.SQLite;
using QuickDateClient.Requests;
using static Android.Support.V4.View.ViewPager;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace QuickDate.Activities.SearchFilter
{
    [Activity(Icon = "@mipmap/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class SearchFilterTabbedActivity : AppCompatActivity, IOnPageChangeListener
    {
        #region Variables Basic

        private MainTabAdapter Adapter;
        private ViewPager ViewPager;
        private TabLayout TabLayout;

        private FilterBackgroundFragment BackgroundTab;
        private BasicFragment BasicTab;
        private LooksFragment LooksTab;
        private MoreFragment MoreTab;
        private LifestyleFragment LifestyleTab;
        private TextView ActionButton;
        public Button ResetFilterButton;
        private AdsGoogle.AdMobRewardedVideo RewardedVideoAd;

        #endregion

        #region General

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);

                Methods.App.FullScreenApp(this);
                SetTheme(AppSettings.SetTabDarkTheme ? Resource.Style.MyTheme_Dark_Base : Resource.Style.MyTheme_Base);

                // Create your application here
                SetContentView(Resource.Layout.SearchFilterTabbedLayout);

                LoadFilterOptionsData();

                //Get Value And Set Toolbar
                InitComponent();
                InitToolbar();

                RewardedVideoAd = AdsGoogle.Ad_RewardedVideo(this);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        protected override void OnResume()
        {
            try
            {
                base.OnResume();
                AddOrRemoveEvent(true);
                RewardedVideoAd?.OnResume(this);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        protected override void OnPause()
        {
            try
            {
                base.OnPause();
                AddOrRemoveEvent(false);
                RewardedVideoAd?.OnPause(this);
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

        protected override void OnDestroy()
        {
            try
            {
                RewardedVideoAd?.OnDestroy(this);

                base.OnDestroy();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        #endregion

        #region Menu

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        #endregion

        #region Functions

        private void InitComponent()
        {
            try
            {
                ViewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
                TabLayout = FindViewById<TabLayout>(Resource.Id.tabs);

                ViewPager.OffscreenPageLimit = 5;
                SetUpViewPager(ViewPager);
                ViewPager.AddOnPageChangeListener(this);
                TabLayout.SetupWithViewPager(ViewPager);

                TabLayout.SetTabTextColors(AppSettings.SetTabDarkTheme ? AppSettings.TitleTextColorDark : AppSettings.TitleTextColor, Color.ParseColor(AppSettings.MainColor));

                ActionButton = FindViewById<TextView>(Resource.Id.toolbar_title);
                ActionButton.Text = GetText(Resource.String.Lbl_ApplyFilter);
                ActionButton.SetTextColor(AppSettings.SetTabDarkTheme ? AppSettings.TitleTextColorDark : AppSettings.TitleTextColor);

                ResetFilterButton = FindViewById<Button>(Resource.Id.ResetFilterButton);
                ResetFilterButton.Text = GetText(Resource.String.Lbl_ResetFilter);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void InitToolbar()
        {
            try
            {
                var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
                if (toolbar != null)
                {
                    toolbar.Title = GetString(Resource.String.Lbl_Filter);
                    toolbar.SetTitleTextColor(AppSettings.SetTabDarkTheme ? AppSettings.TitleTextColorDark : AppSettings.TitleTextColor);
                    SetSupportActionBar(toolbar);
                    SupportActionBar.SetDisplayShowCustomEnabled(true);
                    SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                    SupportActionBar.SetHomeButtonEnabled(true);
                    SupportActionBar.SetDisplayShowHomeEnabled(true);

                    toolbar.SetBackgroundResource(AppSettings.SetTabDarkTheme ? Resource.Drawable.linear_gradient_drawable_Dark : Resource.Drawable.linear_gradient_drawable);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void AddOrRemoveEvent(bool addEvent)
        {
            try
            {
                // true +=  // false -=
                if (addEvent)
                {
                    ActionButton.Click += ActionButtonOnClick;
                    ResetFilterButton.Click += ResetFilterButtonClick;
                }
                else
                {
                    ActionButton.Click -= ActionButtonOnClick;
                    ResetFilterButton.Click -= ResetFilterButtonClick;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void ResetFilterButtonClick(object sender, EventArgs e)
        {
            ResetAllFilters();
        }

        #endregion

        #region Set Tab

        private void SetUpViewPager(ViewPager viewPager)
        {
            try
            {
                Adapter = new MainTabAdapter(SupportFragmentManager);

                if (AppSettings.ShowFilterBasic)
                {
                    BasicTab = new BasicFragment();
                    Adapter.AddFragment(BasicTab, GetText(Resource.String.Lbl_Basics));
                }

                if (AppSettings.ShowFilterLooks)
                {
                    LooksTab = new LooksFragment();
                    Adapter.AddFragment(LooksTab, GetText(Resource.String.Lbl_Looks));
                }

                if (AppSettings.ShowFilterBackground)
                {
                    BackgroundTab = new FilterBackgroundFragment();

                    Adapter.AddFragment(BackgroundTab, GetText(Resource.String.Lbl_Background));
                }
                if (AppSettings.ShowFilterLifestyle)
                {
                    LifestyleTab = new LifestyleFragment();
                    Adapter.AddFragment(LifestyleTab, GetText(Resource.String.Lbl_Lifestyle));
                }

                if (AppSettings.ShowFilterMore)
                {
                    MoreTab = new MoreFragment();
                    Adapter.AddFragment(MoreTab, GetText(Resource.String.Lbl_More));
                }

                viewPager.CurrentItem = Adapter.Count;
                viewPager.Adapter = Adapter;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        #endregion Set Tab

        #region Event

        private void ActionButtonOnClick(object sender, EventArgs e)
        {
            try
            {
                // check current state of a Switch (true or false).
                if (BasicTab != null)
                {
                    UserDetails.AgeMin = BasicTab.AgeMin = (int)BasicTab.AgeSeekBar.GetSelectedMinValue();
                    UserDetails.AgeMax = BasicTab.AgeMax = (int)BasicTab.AgeSeekBar.GetSelectedMaxValue();
                    UserDetails.Gender = BasicTab.Gender;
                    UserDetails.Location = BasicTab.Location;
                    UserDetails.SwitchState = BasicTab.SwitchState;
                    UserDetails.Located = BasicTab.DistanceCount.ToString();
                }

                if (BackgroundTab != null)
                {
                    UserDetails.Language = BackgroundTab.Language;
                    UserDetails.Ethnicity = BackgroundTab.IdEthnicity.ToString();
                    UserDetails.Religion = BackgroundTab.IdReligion.ToString();
                }

                if (LifestyleTab != null)
                {
                    UserDetails.RelationShip = LifestyleTab.IdRelationShip.ToString();
                    UserDetails.Smoke = LifestyleTab.IdSmoke.ToString();
                    UserDetails.Drink = LifestyleTab.IdDrink.ToString();
                }

                if (LooksTab != null)
                {
                    UserDetails.Body = LooksTab.IdBody.ToString();
                    UserDetails.FromHeight = LooksTab.FromHeight;
                    UserDetails.ToHeight = LooksTab.ToHeight;
                }

                if (LooksTab != null)
                {
                    UserDetails.Interest = MoreTab.Interest;
                    UserDetails.Education = MoreTab.IdEducation.ToString();
                    UserDetails.Pets = MoreTab.IdPets.ToString();
                }


                SaveFilterOptions();

                SetLocationUser();

                var mainContext = HomeActivity.GetInstance();
                if (mainContext.TrendingFragment != null)
                {
                    var checkList = mainContext.TrendingFragment.MAdapter?.TrendingList?.Where(q => q.Type == ItemType.Users).ToList();
                    if (checkList?.Count > 0)
                    {
                        checkList.Clear();
                        mainContext.TrendingFragment.MAdapter.NotifyDataSetChanged();
                    }

                    var emptyStateChecker = mainContext.TrendingFragment.MAdapter?.TrendingList?.FirstOrDefault(a => a.Type == ItemType.EmptyPage);
                    if (emptyStateChecker != null)
                    {
                        mainContext.TrendingFragment.MAdapter.TrendingList.Remove(emptyStateChecker);
                        mainContext.TrendingFragment.MAdapter.NotifyDataSetChanged();
                    }

                    if (mainContext.TrendingFragment.MainScrollEvent != null)
                        mainContext.TrendingFragment.MainScrollEvent.IsLoading = false;

                    mainContext.TrendingFragment.SwipeRefreshLayout.Refreshing = true;
                    PollyController.RunRetryPolicyFunction(new List<Func<Task>> { mainContext.TrendingFragment.LoadUser });
                }

                Finish();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        #endregion

        private void SetLocationUser()
        {
            try
            {
                var dictionary = new Dictionary<string, string>
                {
                    {"show_me_to", UserDetails.Location},
                };
                PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Users.UpdateProfileAsync(dictionary) });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void LoadFilterOptionsData()
        {
            var dbDatabase = new SqLiteDatabase();
            dbDatabase.Get_data_Filter_Options();
            dbDatabase.Dispose();
        }

        private void SaveFilterOptions()
        {
            var filterOptions = new DataTables.FilterOptionsTb();
            filterOptions.AgeMin = UserDetails.AgeMin;
            filterOptions.AgeMax = UserDetails.AgeMax;
            filterOptions.Gender = UserDetails.Gender;
            filterOptions.Location = UserDetails.Location;
            filterOptions.IsOnline = UserDetails.SwitchState;
            filterOptions.Distance = UserDetails.Located.ToString();

            filterOptions.Language = UserDetails.Language;
            filterOptions.Ethnicity = UserDetails.Ethnicity;
            filterOptions.Religion = UserDetails.Religion;

            filterOptions.RelationShip = UserDetails.RelationShip;
            filterOptions.Smoke = UserDetails.Smoke;
            filterOptions.Drink = UserDetails.Drink;

            filterOptions.Body = UserDetails.Body;
            filterOptions.FromHeight = UserDetails.FromHeight;
            filterOptions.ToHeight = UserDetails.ToHeight;

            filterOptions.Interest = UserDetails.Interest;
            filterOptions.Education = UserDetails.Education.ToString();
            filterOptions.Pets = UserDetails.Pets.ToString();

            var dbDatabase = new SqLiteDatabase();
            dbDatabase.InsertOrUpdateFilter_Options(filterOptions);
            dbDatabase.Dispose();
        }

        public void ResetAllFilters()
        {
            var filterOptions = new DataTables.FilterOptionsTb();
            UserDetails.AgeMin = GlobalConstants.FilterOptionAgeMin;
            UserDetails.AgeMax = GlobalConstants.FilterOptionAgeMax;
            UserDetails.Gender = GlobalConstants.FilterOptionGender;
            UserDetails.Location = "";
            UserDetails.SwitchState = GlobalConstants.FilterOptionIsOnline;
            UserDetails.Located = GlobalConstants.FilterOptionDistance;

            UserDetails.Language = GlobalConstants.FilterOptionLanguage;
            UserDetails.Ethnicity = "";
            UserDetails.Religion = "";

            UserDetails.RelationShip = "";
            UserDetails.Smoke = "";
            UserDetails.Drink = "";

            UserDetails.Body = "";
            UserDetails.FromHeight = GlobalConstants.FilterOptionFromHeight;
            UserDetails.ToHeight = GlobalConstants.FilterOptionToHeight;

            UserDetails.Interest = "";
            UserDetails.Education = "";
            UserDetails.Pets = "";

            SaveFilterOptions();
            LoadFilterOptionsData();

            BasicTab.SetLocalData();
            BackgroundTab.SetLocalData();
            LooksTab.SetLocalData();
            MoreTab.SetLocalData();
            LifestyleTab.SetLocalData();
        }

        public void OnPageScrollStateChanged(int state)
        {
        }

        public void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
        {
            ShowHideResetFilterOption(position);
        }

        public void OnPageSelected(int position)
        {
            ShowHideResetFilterOption(position);
        }

        private void ShowHideResetFilterOption(int position)
        {
            ResetFilterButton.Visibility = AppSettings.ShowResetFilterForAllPages || position == 0 ? ViewStates.Visible : ViewStates.Gone;
        }
    }
}