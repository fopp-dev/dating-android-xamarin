﻿using System;
using System.Globalization;
using System.Linq;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using ME.Itangqi.Waveloadingview;
using QuickDate.Activities.Blogs;
using QuickDate.Activities.Favorite;
using QuickDate.Activities.Friends;
using QuickDate.Activities.IDisliked;
using QuickDate.Activities.ILiked;
using QuickDate.Activities.InviteFriends;
using QuickDate.Activities.MyProfile;
using QuickDate.Activities.Premium;
using QuickDate.Activities.SettingsUser;
using QuickDate.Helpers.CacheLoaders;
using QuickDate.Helpers.Controller;
using QuickDate.Helpers.Model;
using QuickDate.Helpers.Utils;
using QuickDate.SQLite;
using QuickDateClient;
using QuickDateClient.Classes.Authorization;
using QuickDateClient.Classes.Global;
using QuickDateClient.Requests;
using Console = System.Console;

namespace QuickDate.Activities.Tabbes.Fragment
{
    public class ProfileFragment : Android.Support.V4.App.Fragment
    {
        #region Variables Basic

        private HomeActivity GlobalContext;
        private TextView Username, XtBoostMe, TxtUpgrade, TxtLiked, TxtVisits;
        public TextView WalletNumber;
        public ImageView ProfileImage;
        private FrameLayout EditButton, SettingsButton, BoostButton;
        private RelativeLayout WalletButton, PopularityButton, UpgradeButton, FavoriteButton, HelpButton, InviteButton, BlogsButton, FriendsButton, LikedButton, DislikedButton;
        private LinearLayout HeaderSection;
        public FavoriteUserFragment FavoriteFragment;
        public FriendsFragment FriendsFragment;
        public LikedFragment LikedFragment;
        public DislikedFragment DislikedFragment;
        public TimerTime Time;
        private WaveLoadingView MWaveLoadingView;

        #endregion

        #region General

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
            GlobalContext = (HomeActivity)Activity;
        }
          
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                View view = inflater.Inflate(Resource.Layout.TProfileLayout, container, false);
     
                return view;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            try
            {
                base.OnViewCreated(view, savedInstanceState);

                //Get Value 
                InitComponent(view);

                WalletButton.Click += WalletButtonOnClick;
                PopularityButton.Click += PopularityButtonOnClick;
                UpgradeButton.Click += UpgradeButtonOnClick;
                EditButton.Click += EditButtonOnClick;
                ProfileImage.Click += ProfileImageOnClick;
                SettingsButton.Click += SettingsButtonOnClick;
                FavoriteButton.Click += FavoriteButtonOnClick;
                HelpButton.Click += HelpButtonOnClick;
                InviteButton.Click += InviteButtonOnClick;
                BlogsButton.Click += BlogsButtonOnClick;
                BoostButton.Click += BoostButtonOnClick;
                FriendsButton.Click += FriendsButtonOnClick;
                LikedButton.Click += ILikedButtonOnClick;
                DislikedButton.Click += DislikedButtonOnClick;

                GetMyInfoData();
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

        #endregion

        #region Functions

        private void InitComponent(View view)
        {
            try
            {
                ProfileImage = view.FindViewById<ImageView>(Resource.Id.Iconimage2);
                Username = view.FindViewById<TextView>(Resource.Id.username);
                WalletNumber = view.FindViewById<TextView>(Resource.Id.walletnumber);
                TxtUpgrade = view.FindViewById<TextView>(Resource.Id.upgradeText);
                TxtLiked = view.FindViewById<TextView>(Resource.Id.LikesText);
                TxtVisits = view.FindViewById<TextView>(Resource.Id.VisitsText);
                XtBoostMe = view.FindViewById<TextView>(Resource.Id.tv_Boost);
                EditButton = view.FindViewById<FrameLayout>(Resource.Id.EditButton);
                SettingsButton = view.FindViewById<FrameLayout>(Resource.Id.SettingsButton);
                BoostButton = view.FindViewById<FrameLayout>(Resource.Id.BoostButton);
                WalletButton = view.FindViewById<RelativeLayout>(Resource.Id.walletSection);
                PopularityButton = view.FindViewById<RelativeLayout>(Resource.Id.popularitySection);
                UpgradeButton = view.FindViewById<RelativeLayout>(Resource.Id.upgradeSection);
                LikedButton = view.FindViewById<RelativeLayout>(Resource.Id.IlikedLayout);
                DislikedButton = view.FindViewById<RelativeLayout>(Resource.Id.IDislikedLayout);
                FriendsButton = view.FindViewById<RelativeLayout>(Resource.Id.StFriendsLayout);
                FavoriteButton = view.FindViewById<RelativeLayout>(Resource.Id.StFavoriteLayout);
                InviteButton = view.FindViewById<RelativeLayout>(Resource.Id.StInviteFriendsLayout);
                BlogsButton = view.FindViewById<RelativeLayout>(Resource.Id.StBlogsLayout);
                HelpButton = view.FindViewById<RelativeLayout>(Resource.Id.StNeedHelpLayout);
                HeaderSection = view.FindViewById<LinearLayout>(Resource.Id.headerSection);

                MWaveLoadingView = (WaveLoadingView)view.FindViewById(Resource.Id.waveLoadingView);
                MWaveLoadingView.Visibility = ViewStates.Gone;

                BoostButton.Tag = "Off";

                GlideImageLoader.LoadImage(Activity, UserDetails.Avatar, ProfileImage, ImageStyle.CircleCrop, ImagePlaceholders.Drawable);
                 
                if (AppSettings.EnableAppFree)
                {
                    WalletButton.Visibility = ViewStates.Invisible;
                    UpgradeButton.Visibility = ViewStates.Invisible; 
                }
                 
                if (!AppSettings.PremiumSystemEnabled)
                    Activity.RunOnUiThread(()=>
                    {
                        UpgradeButton.Visibility = ViewStates.Invisible;
                        UpgradeButton.Enabled = false;
                    });

                if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
                    return;

                Activity.Window.ClearFlags(WindowManagerFlags.TranslucentStatus);
                Activity.Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                Activity.Window.SetStatusBarColor(Color.ParseColor(AppSettings.MainColor));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        #endregion

        #region Events

        //Update Avatar Async
        private void ProfileImageOnClick(object sender, EventArgs e)
        {
            try
            {
                GlobalContext.OpenDialogGallery(OpenGalleryDialogFor.Avatar);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }


        //Open I Disliked User 
        private void DislikedButtonOnClick(object sender, EventArgs e)
        {
            try
            {
                DislikedFragment = new DislikedFragment();
                GlobalContext.FragmentBottomNavigator.DisplayFragment(DislikedFragment);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }


        //Open I Liked User 
        private void ILikedButtonOnClick(object sender, EventArgs e)
        {
            try
            {
                LikedFragment = new LikedFragment();
                GlobalContext.FragmentBottomNavigator.DisplayFragment(LikedFragment);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }


        //Open Blogs
        private void BlogsButtonOnClick(object sender, EventArgs e)
        {
            try
            {
                StartActivity(new Intent(Context, typeof(BlogsActivity)));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        //Open Friend 
        private void FriendsButtonOnClick(object sender, EventArgs e)
        {
            try
            {
                FriendsFragment = new FriendsFragment();
                GlobalContext.FragmentBottomNavigator.DisplayFragment(FriendsFragment);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

         //Open edit my info
        private void EditButtonOnClick(object sender, EventArgs e)
        {
            try
            {
                StartActivity(new Intent(Activity, typeof(EditProfileActivity)));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        //Open Upgrade
        private void UpgradeButtonOnClick(object sender, EventArgs e)
        {
            try
            {
                var dataUser = ListUtils.MyUserInfo.FirstOrDefault();
                if (dataUser?.IsPro == "1")
                { 
                    var window = new DialogController(Activity);
                    window.OpenDialogYouArePremium(dataUser.ProTime , dataUser.ProType); 
                }
                else
                {
                    var window = new PopupController(Activity);
                    window.DisplayPremiumWindow();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        //Open Popularity >> Very Low
        private void PopularityButtonOnClick(object sender, EventArgs e)
        {
            try
            {
                StartActivity(new Intent(Context, typeof(PopularityActivity)));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        //Open Wallet
        private void WalletButtonOnClick(object sender, EventArgs e)
        {
            try
            { 
                var window = new PopupController(Activity);
                window.DisplayCreditWindow("credits");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        //Boost me
        private async void BoostButtonOnClick(object sender, EventArgs e)
        {
            try
            {
                var dataUser = ListUtils.MyUserInfo.FirstOrDefault();
                if (dataUser != null)
                {
                    if (BoostButton.Tag.ToString() == "Off")
                    { 
                        if (!AppSettings.EnableAppFree)
                        {
                            string myBalance = dataUser.Balance ?? "0.00";
                            if (!string.IsNullOrEmpty(myBalance) && myBalance != "0.00")
                            {
                                //sent new api
                                (int apiStatus, var respond) = await RequestsAsync.Users.ManagePopularityAsync("boost").ConfigureAwait(false);
                                if (apiStatus == 200)
                                { 
                                    if (respond is AmountObject result)
                                    {
                                        Activity.RunOnUiThread(() =>
                                        {
                                            try
                                            {
                                                BoostButton.Tag = "Run";
                                                myBalance = result.CreditAmount.ToString();
                                                WalletNumber.Text = result.CreditAmount.ToString();

                                                var timeBoost = ListUtils.SettingsSiteList.FirstOrDefault()?.BoostExpireTime ?? "4";
                                                var timeBoostMilliseconds = Methods.Time.ConvertMinutesToMilliseconds(Convert.ToDouble(timeBoost));
                                                dataUser.BoostedTime = timeBoostMilliseconds.ToString(CultureInfo.InvariantCulture);
                                                dataUser.IsBoosted = "1";

                                                GetMyInfoData();
                                            }
                                            catch (Exception exception)
                                            {
                                                Console.WriteLine(exception);
                                            }
                                        });
                                    }
                                }
                                else Methods.DisplayReportResult(Activity, respond);
                            }
                            else
                            {
                                var window = new PopupController(Activity);
                                window.DisplayCreditWindow("credits");
                            }
                        }
                        else
                        {
                            //sent new api
                            (int apiStatus, var respond) = await RequestsAsync.Users.ManagePopularityAsync("boost").ConfigureAwait(false);
                            if (apiStatus == 200)
                            {
                                if (respond is AmountObject result)
                                {
                                    Activity.RunOnUiThread(() =>
                                    {
                                        try
                                        {
                                            BoostButton.Tag = "Run";
                                            //myBalance = result.CreditAmount.ToString();
                                            WalletNumber.Text = result.CreditAmount.ToString();

                                            var timeBoost = ListUtils.SettingsSiteList.FirstOrDefault()?.BoostExpireTime ?? "4";
                                            var timeBoostMilliseconds = Methods.Time.ConvertMinutesToMilliseconds(Convert.ToDouble(timeBoost));
                                            dataUser.BoostedTime = timeBoostMilliseconds.ToString(CultureInfo.InvariantCulture);
                                            dataUser.IsBoosted = "1";

                                            GetMyInfoData();
                                        }
                                        catch (Exception exception)
                                        {
                                            Console.WriteLine(exception);
                                        }
                                    });
                                }
                            }
                            else Methods.DisplayReportResult(Activity, respond);
                        } 
                    }
                    else
                    {
                        Toast.MakeText(Context,GetText(Resource.String.Lbl_YourBoostExpireInMinutes),ToastLength.Long).Show();
                    } 
                } 
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        //Open Settings
        private void SettingsButtonOnClick(object sender, EventArgs e)
        {
            try
            {
                StartActivity(new Intent(Context, typeof(SettingsActivity)));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
         
        //Invite Friends
        private void InviteButtonOnClick(object sender, EventArgs e)
        {
            try
            {
                StartActivity(new Intent(Context, typeof(InviteFriendsActivity)));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        //Help
        private void HelpButtonOnClick(object sender, EventArgs e)
        {
            try
            { 
                var intent = new Intent(Context, typeof(LocalWebViewActivity));
                intent.PutExtra("URL", Client.WebsiteUrl + "/contact");
                intent.PutExtra("Type", GetText(Resource.String.Lbl_Help));
                Activity.StartActivity(intent);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        //Favorite
        private void FavoriteButtonOnClick(object sender, EventArgs e)
        {
            try
            {
                FavoriteFragment = new FavoriteUserFragment();
                GlobalContext.FragmentBottomNavigator.DisplayFragment(FavoriteFragment);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        #endregion

        public async void GetMyInfoData()
        {
            try
            {
                if (!Methods.CheckConnectivity())
                {
                    Toast.MakeText(Context, Context.GetString(Resource.String.Lbl_CheckYourInternetConnection), ToastLength.Short).Show();
                    return;
                }

                if (ListUtils.MyUserInfo.Count == 0)
                {
                    var sqlEntity = new SqLiteDatabase();
                    sqlEntity.GetDataMyInfo();
                    sqlEntity.Dispose();
                }

                var dataUser = ListUtils.MyUserInfo.FirstOrDefault();
                if (dataUser != null)
                {
                    LoadDataUser(dataUser); 
                }
                  
                var data = await ApiRequest.GetInfoData(Activity, UserDetails.UserId.ToString());
                if (data != null)
                {
                    LoadDataUser(data.Data);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void LoadDataUser(UserInfoObject data)
        {
            try
            { 
                GlideImageLoader.LoadImage(Activity, data.Avater, ProfileImage, ImageStyle.CircleCrop, ImagePlaceholders.Drawable);
                GlideImageLoader.LoadImage(Activity, data.Avater, GlobalContext.FragmentBottomNavigator.ProfileImage, ImageStyle.CircleCrop, ImagePlaceholders.Drawable);

                Username.Text = QuickDateTools.GetNameFinal(data);

                if (data.Verified == "1")
                    Username.SetCompoundDrawablesWithIntrinsicBounds(0, 0, Resource.Drawable.icon_checkmark_vector, 0);

                WalletNumber.Text = data.Balance.Replace(".00", "");

                if (data.LikesCount != null)
                    TxtLiked.Text = data.LikesCount.Value + " " + Activity.GetText(Resource.String.Lbl_Likes);
                else
                    TxtLiked.Text = data.Likes.Count + " " + Activity.GetText(Resource.String.Lbl_Likes);

                if (data.VisitsCount != null)
                    TxtLiked.Text = data.VisitsCount.Value + " " + Activity.GetText(Resource.String.Lbl_Likes);
                else
                    TxtVisits.Text = data.Visits.Count + " " + Activity.GetText(Resource.String.Lbl_Visits);

                if (data.IsPro == "1")
                {
                    #region UpgradeButton >> ViewStates.Gone

                    //UpgradeButton.Visibility = ViewStates.Gone;

                    //LinearLayout.LayoutParams layoutParam1 = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, 100, 1f);
                    //LinearLayout.LayoutParams layoutParam2 = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, 100, 1f);

                    //((ViewGroup)WalletButton.Parent)?.RemoveView(WalletButton);
                    //((ViewGroup)PopularityButton.Parent)?.RemoveView(PopularityButton);
                    //((ViewGroup)UpgradeButton.Parent)?.RemoveView(UpgradeButton);

                    //HeaderSection.WeightSum = 2;

                    //layoutParam1.TopMargin = 20;
                    //layoutParam2.TopMargin = 20;
                    //layoutParam2.MarginStart = 20;

                    //WalletButton.LayoutParameters = layoutParam1;
                    //PopularityButton.LayoutParameters = layoutParam2;

                    //HeaderSection.AddView(WalletButton, layoutParam1);
                    //HeaderSection.AddView(PopularityButton, layoutParam2); 

                    #endregion

                    switch (data.ProType)
                    {
                        case "1":
                            TxtUpgrade.Text = GetText(Resource.String.Lbl_Weekly);
                            break;
                        case "2":
                            TxtUpgrade.Text = GetText(Resource.String.Lbl_Monthly);
                            break;
                        case "3":
                            TxtUpgrade.Text = GetText(Resource.String.Lbl_Yearly);
                            break;
                        case "4":
                            TxtUpgrade.Text = GetText(Resource.String.Lbl_Lifetime);
                            break;
                        default:
                            TxtUpgrade.Text = GetText(Resource.String.Lbl_Upgrade);
                            break;
                    }
                }
                else
                {
                    if (AppSettings.PremiumSystemEnabled)
                    {
                        TxtUpgrade.Text = GetText(Resource.String.Lbl_Upgrade);
                        UpgradeButton.Visibility = ViewStates.Visible;
                    }
                }

                if (Convert.ToInt32(data.BoostedTime) > 0)
                {
                    var timeBoost = ListUtils.SettingsSiteList.FirstOrDefault()?.BoostExpireTime ?? "4";
                    var timeBoostSeconds = Methods.Time.ConvertMinutesToSeconds(Convert.ToDouble(timeBoost)); //240

                    double progressStart;
                    double progress = 100 / timeBoostSeconds; //0.4

                    if (Time == null)
                    {
                        double progressPlus = 100 / timeBoostSeconds;

                        Time = new TimerTime();
                        TimerTime.TimerCount = Time.GetTimer();
                        var plus1 = progressPlus;
                        TimerTime.TimerCount.Elapsed += (sender, args) =>
                        {
                            var plus = plus1;
                            Activity.RunOnUiThread(() =>
                            {
                                try
                                {
                                    var (minutes, seconds) = Time.TimerCountOnElapsed();
                                    if ((minutes == "" || minutes == "0") && (seconds == "" || seconds == "0"))
                                    {
                                        Time.SetStopTimer();
                                        Time = null;
                                        TimerTime.TimerCount = null;

                                        data.BoostedTime = "0";
                                        XtBoostMe.Text = Context.GetText(Resource.String.Lbl_BoostMe);
                                        SetStopAnimationPopularity();
                                        progress = 0;
                                        progressStart = 0;
                                        MWaveLoadingView.CancelAnimation();

                                        BoostButton.Tag = "Off";
                                    }
                                    else
                                    {
                                        XtBoostMe.Text = minutes + ":" + seconds;
                                        progress += plus;

                                        progressStart = Math.Round(progress, MidpointRounding.AwayFromZero);
                                        MWaveLoadingView.ProgressValue = Convert.ToInt32(progressStart);
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                }
                            });
                        };
                    } 

                    string countTime = Time.CheckCountTime(Convert.ToInt32(data.BoostedTime));
                    if (countTime != "0:0" && !countTime.Contains("-") && !string.IsNullOrEmpty(countTime))
                    {
                        int min = Convert.ToInt32(countTime.Split(":")[0]);
                        int sec = Convert.ToInt32(countTime.Split(":")[1]);
                        Time.SetMinutes(min);
                        Time.SetSeconds(sec);
                        Time.SetStartTimer();
                        XtBoostMe.Text = countTime;

                        var minSeconds = Methods.Time.ConvertMinutesToSeconds(Convert.ToDouble(min));

                        //start in here  
                        progress = (timeBoostSeconds - minSeconds) * 100 / timeBoostSeconds;

                        SetStartAnimationPopularity();
                    }
                    else
                    {
                        Time.SetStopTimer();
                        Time = null;
                        TimerTime.TimerCount = null;

                        XtBoostMe.Text = Context.GetText(Resource.String.Lbl_BoostMe);
                        SetStopAnimationPopularity();

                        BoostButton.Tag = "Off";
                    }
                }
                else
                {
                    if (Time != null)
                    {
                        Time.SetStopTimer();
                        Time = null;
                        TimerTime.TimerCount = null;

                        XtBoostMe.Text = Context.GetText(Resource.String.Lbl_BoostMe);
                        SetStopAnimationPopularity();

                        BoostButton.Tag = "Off";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void SetStartAnimationPopularity()
        {
            try
            {
                BoostButton.Visibility = ViewStates.Invisible;
                MWaveLoadingView.Visibility = ViewStates.Visible;
                MWaveLoadingView.StartAnimation();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void SetStopAnimationPopularity()
        {
            try
            {
                BoostButton.Visibility = ViewStates.Visible;
                MWaveLoadingView.Visibility = ViewStates.Gone;

                MWaveLoadingView?.CancelAnimation();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        } 
    }
}