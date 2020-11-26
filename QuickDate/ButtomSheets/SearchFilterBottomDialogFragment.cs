﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AFollestad.MaterialDialogs;
using Android.Gms.Ads;
using Android.Graphics;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Java.Lang;
using QuickDate.Activities.Tabbes;
using QuickDate.Helpers.Ads;
using QuickDate.Helpers.Controller;
using QuickDate.Helpers.Fonts;
using QuickDate.Helpers.Model;
using QuickDate.Helpers.RangeSlider;
using QuickDate.Helpers.Utils;
using QuickDateClient.Requests;
using Exception = System.Exception;

namespace QuickDate.ButtomSheets
{
    public class SearchFilterBottomDialogFragment : BottomSheetDialogFragment, MaterialDialog.IListCallback, MaterialDialog.ISingleButtonCallback, SeekBar.IOnSeekBarChangeListener
    {
        #region Variables Basic

        private TextView Title, IconBack, IconLocation, LocationTextView, LocationPlace, LocationMoreIcon, IconFire, GenderTextView, IconAge, AgeTextView, AgeNumberTextView, OnlineTextView, IconOnline, ResetTextView, IconDistance, TxtDistanceCount;
        private RelativeLayout LocationLayout;
        private Button ButtonMan, ButtonGirls, ButtonBoth, ButtonApply;
        private RangeSliderControl AgeSeekBar;
        private Switch OnlineSwitch;
        private SeekBar DistanceBar;

        public int DistanceCount;
        private int AgeMin = GlobalConstants.FilterOptionAgeMin, AgeMax = GlobalConstants.FilterOptionAgeMax;
        private string Gender = GlobalConstants.FilterOptionGenderMale, Location = "";
        private bool SwitchState = true;
        private HomeActivity GlobalContext;
        private AdView MAdView;
         
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
                var contextThemeWrapper = AppSettings.SetTabDarkTheme ? new ContextThemeWrapper(Activity, Resource.Style.MyTheme_Dark_Base) : new ContextThemeWrapper(Activity, Resource.Style.MyTheme_Base);

                // clone the inflater using the ContextThemeWrapper

                LayoutInflater localInflater = inflater.CloneInContext(contextThemeWrapper);

                View view = localInflater.Inflate(Resource.Layout.ButtomSheetSearchFilter, container, false);

                //Get Value And Set Toolbar
                InitComponent(view); 

                IconBack.Click += IconBackOnClick;
                LocationLayout.Click += LocationLayoutOnClick;
                ButtonMan.Click += ButtonManOnClick;
                ButtonGirls.Click += ButtonGirlsOnClick;
                ButtonBoth.Click += ButtonBothOnClick;
                ButtonApply.Click += ButtonApplyOnClick;
                ResetTextView.Click += ResetTextViewOnClick;
                AgeSeekBar.DragCompleted += AgeSeekBarOnDragCompleted;

                return view;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public override void OnResume()
        {
            try
            {
                MAdView?.Resume();
                base.OnResume();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public override void OnPause()
        {
            try
            {
                MAdView?.Pause();
                base.OnPause();
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

        public override void OnDestroy()
        {
            try
            { 
                MAdView?.Destroy();

                base.OnDestroy();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        #endregion

        #region Functions

        private void InitComponent(View view)
        {
            try
            {
                Title = view.FindViewById<TextView>(Resource.Id.titlepage);
                IconBack = view.FindViewById<TextView>(Resource.Id.IconBack);
                IconLocation = view.FindViewById<TextView>(Resource.Id.IconLocation);
                LocationTextView = view.FindViewById<TextView>(Resource.Id.LocationTextView);
                LocationPlace = view.FindViewById<TextView>(Resource.Id.LocationPlace);
                LocationMoreIcon = view.FindViewById<TextView>(Resource.Id.LocationMoreIcon);
                GenderTextView = view.FindViewById<TextView>(Resource.Id.GenderTextView);
                IconFire = view.FindViewById<TextView>(Resource.Id.IconFire);
                IconAge = view.FindViewById<TextView>(Resource.Id.IconAge);
                AgeTextView = view.FindViewById<TextView>(Resource.Id.AgeTextView);
                AgeNumberTextView = view.FindViewById<TextView>(Resource.Id.Agenumber);
                OnlineTextView = view.FindViewById<TextView>(Resource.Id.OnlineTextView);
                IconOnline = view.FindViewById<TextView>(Resource.Id.Icononline);
                ResetTextView = view.FindViewById<TextView>(Resource.Id.Resetbutton);
                LocationLayout = view.FindViewById<RelativeLayout>(Resource.Id.LayoutLocation);
                ButtonMan = view.FindViewById<Button>(Resource.Id.ManButton);
                ButtonGirls = view.FindViewById<Button>(Resource.Id.GirlsButton);
                ButtonBoth = view.FindViewById<Button>(Resource.Id.BothButton);
                ButtonApply = view.FindViewById<Button>(Resource.Id.ApplyButton);
                AgeSeekBar = view.FindViewById<RangeSliderControl>(Resource.Id.seekbar);
                OnlineSwitch = view.FindViewById<Switch>(Resource.Id.togglebutton);
                IconDistance = view.FindViewById<TextView>(Resource.Id.IconDistance);
                TxtDistanceCount = view.FindViewById<TextView>(Resource.Id.Distancenumber);
                DistanceBar = view.FindViewById<SeekBar>(Resource.Id.distanceSeeker);

                FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, IconBack, IonIconsFonts.ChevronLeft);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, IconLocation, IonIconsFonts.IosLocationOutline);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, LocationMoreIcon, IonIconsFonts.ChevronRight);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, IconFire, IonIconsFonts.IosPersonOutline);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, IconAge, IonIconsFonts.Calendar);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, IconOnline, IonIconsFonts.Ionic);
                FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, IconDistance, FontAwesomeIcon.StreetView);

                FontUtils.SetFont(LocationTextView, Fonts.SfRegular);
                FontUtils.SetFont(LocationPlace, Fonts.SfRegular);
                FontUtils.SetFont(Title, Fonts.SfMedium);
                FontUtils.SetFont(GenderTextView, Fonts.SfRegular);
                FontUtils.SetFont(AgeTextView, Fonts.SfRegular);
                FontUtils.SetFont(OnlineTextView, Fonts.SfRegular);
                FontUtils.SetFont(TxtDistanceCount, Fonts.SfRegular);

                DistanceBar.Max = 50;
                DistanceBar.SetOnSeekBarChangeListener(this);

                MAdView = view.FindViewById<AdView>(Resource.Id.adView);
                AdsGoogle.InitAdView(MAdView, null);

                AgeSeekBar.SetSelectedMinValue(18);
                AgeSeekBar.SetSelectedMaxValue(75);

                OnlineSwitch.Checked = false;

                ButtonGirls.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                ButtonGirls.SetTextColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#ffffff") : Color.ParseColor("#444444"));

                ButtonBoth.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends_pressed);
                ButtonBoth.SetTextColor(Color.ParseColor("#ffffff"));

                ButtonMan.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                ButtonMan.SetTextColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#ffffff") : Color.ParseColor("#444444"));

                SetLocalData();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
 
        #endregion

        #region Events

        //Save and sent data and set new search 
        private void ButtonApplyOnClick(object sender, EventArgs e)
        {
            try
            {
                // check current state of a Switch (true or false).
                SwitchState = OnlineSwitch.Checked;

                UserDetails.AgeMin = AgeMin = (int)AgeSeekBar.GetSelectedMinValue();
                UserDetails.AgeMax = AgeMax = (int)AgeSeekBar.GetSelectedMaxValue();
                UserDetails.Gender = Gender;
                UserDetails.Location = Location;
                UserDetails.SwitchState = SwitchState;

                SetLocationUser();

                var checkList = GlobalContext.TrendingFragment.MAdapter?.TrendingList?.Where(q => q.Type == ItemType.Users).ToList();
                if (checkList?.Count > 0)
                {
                    checkList.Clear();
                    GlobalContext.TrendingFragment.MAdapter.NotifyDataSetChanged();
                }

                var emptyStateChecker = GlobalContext.TrendingFragment.MAdapter?.TrendingList?.FirstOrDefault(a => a.Type == ItemType.EmptyPage);
                if (emptyStateChecker != null)
                {
                    GlobalContext.TrendingFragment.MAdapter.TrendingList.Remove(emptyStateChecker);
                    GlobalContext.TrendingFragment.MAdapter.NotifyDataSetChanged();
                }

                if (GlobalContext.TrendingFragment.MainScrollEvent != null)
                    GlobalContext.TrendingFragment.MainScrollEvent.IsLoading = false;

                GlobalContext.TrendingFragment.SwipeRefreshLayout.Refreshing = true;
                PollyController.RunRetryPolicyFunction(new List<Func<Task>> { GlobalContext.TrendingFragment.LoadUser });
                 
                Dismiss();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        //Reset Value
        private void ResetTextViewOnClick(object sender, EventArgs e)
        {
            try
            {
                ButtonGirls.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                ButtonGirls.SetTextColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#ffffff") : Color.ParseColor("#444444"));

                ButtonBoth.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends_pressed);
                ButtonBoth.SetTextColor(Color.ParseColor("#ffffff"));

                ButtonMan.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                ButtonMan.SetTextColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#ffffff") : Color.ParseColor("#444444"));
                Gender = GlobalConstants.FilterOptionGender;

                LocationPlace.Text = GetText(Resource.String.Lbl_NearBy);

                AgeMin = GlobalConstants.FilterOptionAgeMin;
                AgeMax = GlobalConstants.FilterOptionAgeMax;

                AgeNumberTextView.Text = AgeMin + " - " + AgeMax;

                SwitchState = false;
                OnlineSwitch.Checked = false;

                Location = ListUtils.MyUserInfo?.FirstOrDefault()?.Country;

                UserDetails.AgeMin = AgeMin;
                UserDetails.AgeMax = AgeMax;
                UserDetails.Gender = Gender;
                UserDetails.Location = Location;
                UserDetails.SwitchState = SwitchState;

                if ((int)AgeSeekBar.GetSelectedMinValue() != GlobalConstants.FilterOptionAgeMin)
                    AgeSeekBar.SetSelectedMinValue(GlobalConstants.FilterOptionAgeMin);

                if ((int)AgeSeekBar.GetSelectedMaxValue() != GlobalConstants.FilterOptionAgeMax)
                    AgeSeekBar.SetSelectedMaxValue(GlobalConstants.FilterOptionAgeMax);

                SetLocationUser();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        //Location
        private void LocationLayoutOnClick(object sender, EventArgs e)
        {
            try
            {
                //string[] countriesArray = Context.Resources.GetStringArray(Resource.Array.countriesArray);
                var countriesArray = ListUtils.SettingsSiteList.FirstOrDefault()?.Countries;

                var arrayAdapter = new List<string>();
                var dialogList = new MaterialDialog.Builder(Context).Theme(AppSettings.SetTabDarkTheme ? AFollestad.MaterialDialogs.Theme.Dark : AFollestad.MaterialDialogs.Theme.Light);

                if (countriesArray != null) arrayAdapter.AddRange(countriesArray.Select(item => Methods.FunString.DecodeString(item.Values.FirstOrDefault()?.Name)));

                dialogList.Title(GetText(Resource.String.Lbl_Location));
                dialogList.Items(arrayAdapter);
                dialogList.PositiveText(GetText(Resource.String.Lbl_Close)).OnPositive(this);
                dialogList.AlwaysCallSingleChoiceCallback();
                dialogList.ItemsCallback(this).Build().Show();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        //Back
        private void IconBackOnClick(object sender, EventArgs e)
        {
            try
            {
                Dismiss();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        //Select gender >> Both (0,1)
        private void ButtonBothOnClick(object sender, EventArgs e)
        {
            try
            {
                //follow_button_profile_friends >> Un click
                //follow_button_profile_friends_pressed >> click
                ButtonBoth.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends_pressed);
                ButtonBoth.SetTextColor(Color.ParseColor("#ffffff"));

                ButtonGirls.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                ButtonGirls.SetTextColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#ffffff") : Color.ParseColor("#444444"));

                ButtonMan.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                ButtonMan.SetTextColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#ffffff") : Color.ParseColor("#444444"));

                Gender = GlobalConstants.FilterOptionGender;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        //Select gender >> Girls (1)
        private void ButtonGirlsOnClick(object sender, EventArgs e)
        {
            try
            {
                //follow_button_profile_friends >> Un click
                //follow_button_profile_friends_pressed >> click
                ButtonGirls.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends_pressed);
                ButtonGirls.SetTextColor(Color.ParseColor("#ffffff"));

                ButtonBoth.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                ButtonBoth.SetTextColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#ffffff") : Color.ParseColor("#444444"));

                ButtonMan.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                ButtonMan.SetTextColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#ffffff") : Color.ParseColor("#444444"));
                Gender = GlobalConstants.FilterOptionGenderFemale;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        //Select gender >> Man (0)
        private void ButtonManOnClick(object sender, EventArgs e)
        {
            try
            {
                //follow_button_profile_friends >> Un click
                //follow_button_profile_friends_pressed >> click
                ButtonMan.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends_pressed);
                ButtonMan.SetTextColor(Color.ParseColor("#ffffff"));

                ButtonBoth.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                ButtonBoth.SetTextColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#ffffff") : Color.ParseColor("#444444"));

                ButtonGirls.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                ButtonGirls.SetTextColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#ffffff") : Color.ParseColor("#444444"));

                Gender = GlobalConstants.FilterOptionGenderMale;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        //Select Age SeekBar >> Right #Max and >> Left #Min
        private void AgeSeekBarOnDragCompleted(object sender, EventArgs e)
        {
            try
            {

                GC.Collect(GC.MaxGeneration);

                AgeMin = (int)AgeSeekBar.GetSelectedMinValue();
                AgeMax = (int)AgeSeekBar.GetSelectedMaxValue();

                AgeNumberTextView.Text = AgeMin + " - " + AgeMax;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        #endregion

        #region MaterialDialog

        public void OnClick(MaterialDialog p0, DialogAction p1)
        {
            try
            {
                if (p1 == DialogAction.Positive)
                {

                }
                else if (p1 == DialogAction.Negative)
                {
                    p0.Dismiss();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void OnSelection(MaterialDialog p0, View p1, int itemId, ICharSequence itemString)
        {
            try
            {
                var id = itemId;
                var text = itemString.ToString();

                //string[] countriesArrayId = Context.Resources.GetStringArray(Resource.Array.countriesArray_id);
                var countriesArrayId = ListUtils.SettingsSiteList.FirstOrDefault()?.Countries?.FirstOrDefault(a => a.Values.FirstOrDefault()?.Name == itemString.ToString())?.Keys.FirstOrDefault(); 
                var data = countriesArrayId;

                Location = data;
                LocationPlace.Text = text;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        #endregion

        #region SeekBar

        public void OnProgressChanged(SeekBar seekBar, int progress, bool fromUser)
        {
            try
            {
                TxtDistanceCount.Text = progress + " " + GetText(Resource.String.Lbl_km);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void OnStartTrackingTouch(SeekBar seekBar)
        {

        }

        public void OnStopTrackingTouch(SeekBar seekBar)
        {
            try
            {
                DistanceCount = seekBar.Progress;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        #endregion

        private void SetLocationUser()
        {
            try
            {
                if (!Methods.CheckConnectivity()) return;
              
                var dictionary = new Dictionary<string, string>
                {
                    {"show_me_to", Location},
                };
                PollyController.RunRetryPolicyFunction(new List<Func<Task>> { () => RequestsAsync.Users.UpdateProfileAsync(dictionary) });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void SetLocalData()
        {
            try
            {
                AgeMin = UserDetails.AgeMin;
                AgeMax = UserDetails.AgeMax;
                Gender = UserDetails.Gender;
                Location = UserDetails.Location;
                SwitchState = UserDetails.SwitchState;
                DistanceCount = int.TryParse(UserDetails.Located, out int distance) ? distance : 0;
                // check current state of a Switch (true or false).
                AgeSeekBar.SetSelectedMinValue(UserDetails.AgeMin);
                AgeSeekBar.SetSelectedMaxValue(UserDetails.AgeMax);
                DistanceBar.Progress = DistanceCount;
                OnlineSwitch.Checked = SwitchState;
                AgeNumberTextView.Text = AgeMin + " - " + AgeMax;

                if (UserDetails.Gender == GlobalConstants.FilterOptionGender)
                {
                    ButtonBoth.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends_pressed);
                    ButtonBoth.SetTextColor(Color.ParseColor("#ffffff"));

                    ButtonGirls.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                    ButtonGirls.SetTextColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#ffffff") : Color.ParseColor("#444444"));

                    ButtonMan.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                    ButtonMan.SetTextColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#ffffff") : Color.ParseColor("#444444"));
                }
                else if (UserDetails.Gender == GlobalConstants.FilterOptionGenderFemale)
                {
                    ButtonGirls.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends_pressed);
                    ButtonGirls.SetTextColor(Color.ParseColor("#ffffff"));

                    ButtonBoth.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                    ButtonBoth.SetTextColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#ffffff") : Color.ParseColor("#444444"));

                    ButtonMan.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                    ButtonMan.SetTextColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#ffffff") : Color.ParseColor("#444444"));
                }
                else if (UserDetails.Gender == GlobalConstants.FilterOptionGenderMale)
                {
                    ButtonMan.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends_pressed);
                    ButtonMan.SetTextColor(Color.ParseColor("#ffffff"));

                    ButtonBoth.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                    ButtonBoth.SetTextColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#ffffff") : Color.ParseColor("#444444"));

                    ButtonGirls.SetBackgroundResource(Resource.Drawable.follow_button_profile_friends);
                    ButtonGirls.SetTextColor(AppSettings.SetTabDarkTheme ? Color.ParseColor("#ffffff") : Color.ParseColor("#444444"));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}