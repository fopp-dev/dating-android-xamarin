using System;
using System.Collections.Generic;
using Android.App;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Com.Airbnb.Lottie;
using QuickDate.Helpers.Ads;
using FragmentTransaction = Android.Support.V4.App.FragmentTransaction;
using FragmentManager = Android.Support.V4.App.FragmentManager;
using QuickDate.Helpers.Controller;
using QuickDate.Helpers.Model;

namespace QuickDate.Activities.Tabbes
{
    public class CustomNavigationController : Java.Lang.Object, View.IOnClickListener
    {
        public Activity MainContext;

        private FrameLayout NotificationButton;
        public LinearLayout HomeButton, ProfileButton, TrendButton, MessagesButton;
        public ImageView HomeImage, NotificationImage, ProfileImage, TrendImage, MessagesImage;
        private int PageNumber;


        public readonly List<Android.Support.V4.App.Fragment> FragmentListTab0 = new List<Android.Support.V4.App.Fragment>();
        public readonly List<Android.Support.V4.App.Fragment> FragmentListTab1 = new List<Android.Support.V4.App.Fragment>();
        public readonly List<Android.Support.V4.App.Fragment> FragmentListTab2 = new List<Android.Support.V4.App.Fragment>();
        public readonly List<Android.Support.V4.App.Fragment> FragmentListTab4 = new List<Android.Support.V4.App.Fragment>();

        private readonly HomeActivity Context;

        public CustomNavigationController(Activity activity)
        {
            MainContext = activity;

            if (activity is HomeActivity cont)
                Context = cont;

            Initialize();
        }
          
        public void Initialize()
        {
            try
            {
                HomeButton = MainContext.FindViewById<LinearLayout>(Resource.Id.llHome);
                NotificationButton = MainContext.FindViewById<FrameLayout>(Resource.Id.llNotification);
                ProfileButton = MainContext.FindViewById<LinearLayout>(Resource.Id.llProfile);
                TrendButton = MainContext.FindViewById<LinearLayout>(Resource.Id.llTrend);
                MessagesButton = MainContext.FindViewById<LinearLayout>(Resource.Id.llMessages);

                HomeImage = MainContext.FindViewById<ImageView>(Resource.Id.ivHome);
                NotificationImage = MainContext.FindViewById<ImageView>(Resource.Id.ivNotification);
                ProfileImage = MainContext.FindViewById<ImageView>(Resource.Id.ivProfile);
                TrendImage = MainContext.FindViewById<ImageView>(Resource.Id.ivTrend);
                MessagesImage = MainContext.FindViewById<ImageView>(Resource.Id.ivMessages);

                HomeButton.SetOnClickListener(this);
                TrendButton.SetOnClickListener(this);
                NotificationButton.SetOnClickListener(this);
                ProfileButton.SetOnClickListener(this);
                MessagesButton.SetOnClickListener(this);
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        
        public void OnClick(View v)
        {
            try
            {
                switch (v.Id)
                {
                    case Resource.Id.llHome:
                        PageNumber = 0; 
                        EnableNavigationButton(HomeImage);
                        ShowFragment0();
                        Context?.TracksCounter?.CheckTracksCounter();
                        break;

                    case Resource.Id.llTrend:
                        if (AppSettings.ShowTrending)
                        {
                            PageNumber = 1;
                            EnableNavigationButton(TrendImage);
                            ShowFragment1();
                            Context?.TracksCounter?.CheckTracksCounter();
                            AdsGoogle.Ad_Interstitial(Context);
                        }
                        else
                        {
                            PageNumber = 2;
                            EnableNavigationButton(NotificationImage);
                            ShowNotificationBadge(false);
                            ShowFragment2();
                            Context?.TracksCounter?.CheckTracksCounter();
                            AdsGoogle.Ad_RewardedVideo(Context);
                        }

                        break;

                    case Resource.Id.llNotification: 
                        if (AppSettings.ShowTrending)
                        {
                            PageNumber = 2;
                            EnableNavigationButton(NotificationImage);
                            ShowNotificationBadge(false);
                            ShowFragment2();
                            Context?.TracksCounter?.CheckTracksCounter();
                            AdsGoogle.Ad_Interstitial(Context);

                        }
                        else
                        {
                            PageNumber = 3;
                            EnableNavigationButton(MessagesImage);
                            //NavigationTabBar.Model tabMessages = Models.FirstOrDefault(a => a.Title == Context.GetText(Resource.String.Lbl_messages));
                            //tabMessages?.HideBadge();
                            Context.ShowChat();
                        } 
                        break;

                    case Resource.Id.llMessages:
                        if (AppSettings.ShowTrending)
                        {
                            PageNumber = 3;
                            EnableNavigationButton(MessagesImage);
                            //NavigationTabBar.Model tabMessages = Models.FirstOrDefault(a => a.Title == Context.GetText(Resource.String.Lbl_messages));
                            //tabMessages?.HideBadge();
                            Context.ShowChat();
                        }
                        else
                        {
                            PageNumber = 4;
                            EnableNavigationButton(ProfileImage);
                            ShowFragment4();
                            Context.ProfileFragment?.GetMyInfoData();
                        }
                        break;
                    case Resource.Id.llProfile:
                        PageNumber = 4;
                        EnableNavigationButton(ProfileImage);
                        ShowFragment4();
                        break;

                    default:
                        PageNumber = 0;
                        EnableNavigationButton(HomeImage);
                        ShowFragment0();
                        break;
                } 
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void EnableNavigationButton(ImageView image)
        {
            DisableAllNavigationButton();
            image.Background = MainContext.GetDrawable(Resource.Drawable.shape_bg_bottom_navigation);

            if (image.Id == ProfileImage.Id)
            {
                ApiRequest.GetInfoData(MainContext, UserDetails.UserId.ToString()).ConfigureAwait(false);
                return;
            }

            image.SetColorFilter(Color.ParseColor(AppSettings.MainColor));

        }

        public void DisableAllNavigationButton()
        {
            HomeImage.Background = null;
            HomeImage.SetColorFilter(AppSettings.SetTabDarkTheme ? Color.White : Color.Black);

            NotificationImage.Background = null;
            NotificationImage.SetColorFilter(AppSettings.SetTabDarkTheme ? Color.White : Color.Black);

            ProfileImage.Background = null;
            //ProfileImage.SetColorFilter(AppSettings.SetTabDarkTheme ? Color.White : Color.Black);

            TrendImage.Background = null;
            TrendImage.SetColorFilter(AppSettings.SetTabDarkTheme ? Color.White : Color.Black);

            MessagesImage.Background = null;
            MessagesImage.SetColorFilter(AppSettings.SetTabDarkTheme ? Color.White : Color.Black);

        }

        public void ShowNotificationBadge(bool showBadge)
        {
            try
            {
                LottieAnimationView animationView2 = MainContext.FindViewById<LottieAnimationView>(Resource.Id.animation_view2);

                if (showBadge)
                {
                    NotificationImage.SetImageDrawable(null);

                    animationView2.SetAnimation("NotificationLotti.json");
                    animationView2.PlayAnimation();
                }
                else
                {
                    animationView2.Progress = 0;
                    animationView2.CancelAnimation();
                    NotificationImage.SetImageResource(Resource.Drawable.icon_notification_vector);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public Android.Support.V4.App.Fragment GetSelectedTabBackStackFragment()
        {
            switch (PageNumber)
            {
                case 0:
                    {
                        var currentFragment = FragmentListTab0[FragmentListTab0.Count - 2];
                        if (currentFragment != null)
                            return currentFragment;
                        break;
                    }
                case 1:
                    {
                        var currentFragment = FragmentListTab1[FragmentListTab1.Count - 2];
                        if (currentFragment != null)
                            return currentFragment;
                        break;
                    }
                case 2:
                    {
                        var currentFragment = FragmentListTab2[FragmentListTab2.Count - 2];
                        if (currentFragment != null)
                            return currentFragment;
                        break;
                    }
                case 4:
                    {
                        var currentFragment = FragmentListTab4[FragmentListTab4.Count - 2];
                        if (currentFragment != null)
                            return currentFragment;
                        break;
                    }

                default:
                    return null;

            }

            return null;
        }

        public int GetCountFragment()
        {
            try
            {
                switch (PageNumber)
                {
                    case 0:
                        return FragmentListTab0.Count > 1 ? FragmentListTab0.Count : 0;
                    case 1:
                        return FragmentListTab1.Count > 1 ? FragmentListTab1.Count : 0;
                    case 2:
                        return FragmentListTab2.Count > 1 ? FragmentListTab2.Count : 0;
                    case 4:
                        return FragmentListTab4.Count > 1 ? FragmentListTab4.Count : 0;
                    default:
                        return 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 0;
            }
        }

        public static void HideFragmentFromList(List<Android.Support.V4.App.Fragment> fragmentList, FragmentTransaction ft)
        {
            try
            {
                if (fragmentList.Count < 0)
                    return;

                foreach (var fra in fragmentList)
                {
                    if (fra.IsVisible)
                        ft.Hide(fra);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void DisplayFragment(Android.Support.V4.App.Fragment newFragment)
        {
            try
            {
                FragmentTransaction ft = Context.SupportFragmentManager.BeginTransaction();

                HideFragmentFromList(FragmentListTab0, ft);
                HideFragmentFromList(FragmentListTab1, ft);
                HideFragmentFromList(FragmentListTab2, ft);
                HideFragmentFromList(FragmentListTab4, ft);

                if (PageNumber == 0)
                    if (!FragmentListTab0.Contains(newFragment))
                        FragmentListTab0.Add(newFragment);

                if (PageNumber == 1)
                    if (!FragmentListTab1.Contains(newFragment))
                        FragmentListTab1.Add(newFragment);

                if (PageNumber == 2)
                    if (!FragmentListTab2.Contains(newFragment))
                        FragmentListTab2.Add(newFragment);

                if (PageNumber == 4)
                    if (!FragmentListTab4.Contains(newFragment))
                        FragmentListTab4.Add(newFragment);
               
                if (!newFragment.IsAdded)
                    ft.Add(Resource.Id.content, newFragment, newFragment.Id.ToString());

                ft.SetCustomAnimations(Resource.Animation.fab_slide_in_from_right, Resource.Animation.fab_slide_in_from_left);
                ft.Show(newFragment).Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void RemoveFragment(Android.Support.V4.App.Fragment oldFragment)
        {
            try
            {
                FragmentTransaction ft = Context.SupportFragmentManager.BeginTransaction();

                if (PageNumber == 0)
                    if (FragmentListTab0.Contains(oldFragment))
                        FragmentListTab0.Remove(oldFragment);

                if (PageNumber == 1)
                    if (FragmentListTab1.Contains(oldFragment))
                        FragmentListTab1.Remove(oldFragment);

                if (PageNumber == 2)
                    if (FragmentListTab2.Contains(oldFragment))
                        FragmentListTab2.Remove(oldFragment);

                if (PageNumber == 4)
                    if (FragmentListTab4.Contains(oldFragment))
                        FragmentListTab4.Remove(oldFragment);


                HideFragmentFromList(FragmentListTab0, ft);
                HideFragmentFromList(FragmentListTab1, ft);
                HideFragmentFromList(FragmentListTab2, ft);
                HideFragmentFromList(FragmentListTab4, ft);

                if (oldFragment.IsAdded)
                    ft.Remove(oldFragment);

                switch (PageNumber)
                {
                    case 0:
                        {
                            var currentFragment = FragmentListTab0[FragmentListTab0.Count - 1];
                            ft.Show(currentFragment).Commit();
                            break;
                        }
                    case 1:
                        {
                            var currentFragment = FragmentListTab1[FragmentListTab1.Count - 1];
                            ft.Show(currentFragment).Commit();
                            break;
                        }
                    case 2:
                        {
                            var currentFragment = FragmentListTab2[FragmentListTab2.Count - 1];
                            ft.Show(currentFragment).Commit();
                            break;
                        }
                    case 4:
                        {
                            var currentFragment = FragmentListTab4[FragmentListTab4.Count - 1];
                            ft.Show(currentFragment).Commit();
                            break;
                        }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void OnBackStackClickFragment()
        {
            try
            {
                if (PageNumber == 0)
                {
                    if (FragmentListTab0.Count > 1)
                    {
                        var currentFragment = FragmentListTab0[FragmentListTab0.Count - 1];
                        if (currentFragment != null)
                            RemoveFragment(currentFragment);
                    }
                    else
                    {
                        Context.Finish();
                    }
                }
                else if (PageNumber == 1)
                {
                    if (FragmentListTab1.Count > 1)
                    {
                        var currentFragment = FragmentListTab1[FragmentListTab1.Count - 1];
                        if (currentFragment != null)
                            RemoveFragment(currentFragment);
                    }
                    else
                    {
                        Context.Finish();
                    }

                }
                else if (PageNumber == 2)
                {
                    if (FragmentListTab2.Count > 1)
                    {
                        var currentFragment = FragmentListTab2[FragmentListTab2.Count - 1];
                        if (currentFragment != null)
                            RemoveFragment(currentFragment);
                    }
                    else
                    {
                        Context.Finish();
                    }
                }
                else if (PageNumber == 4)
                {
                    if (FragmentListTab4.Count > 1)
                    {
                        var currentFragment = FragmentListTab4[FragmentListTab4.Count - 1];
                        if (currentFragment != null)
                            RemoveFragment(currentFragment);
                    }
                    else
                    {
                        Context.Finish();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void ShowFragment0()
        {
            try
            {
                if (FragmentListTab0.Count <= 0)
                    return;
                var currentFragment = FragmentListTab0[FragmentListTab0.Count - 1];
                if (currentFragment != null)
                    DisplayFragment(currentFragment);

               
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void ShowFragment1()
        {
            try
            {
                if (FragmentListTab1.Count <= 0) return;
                var currentFragment = FragmentListTab1[FragmentListTab1.Count - 1];
                if (currentFragment != null)
                    DisplayFragment(currentFragment);

                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void ShowFragment2()
        {
            try
            {
                if (FragmentListTab2.Count <= 0) return;
                var currentFragment = FragmentListTab2[FragmentListTab2.Count - 1];
                if (currentFragment != null)
                    DisplayFragment(currentFragment);

               
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void ShowFragment4()
        {
            try
            {
                if (FragmentListTab4.Count <= 0) return;
                var currentFragment = FragmentListTab4[FragmentListTab4.Count - 1];
                if (currentFragment != null)
                    DisplayFragment(currentFragment);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static bool BringFragmentToTop(Android.Support.V4.App.Fragment Tobeshown, FragmentManager fragmentManager, List<Android.Support.V4.App.Fragment> videoFrameLayoutFragments)
        {



            if (Tobeshown != null)
            {
                FragmentTransaction fragmentTransaction = fragmentManager.BeginTransaction();


                foreach (var f in fragmentManager.Fragments)
                {
                    if (videoFrameLayoutFragments.Contains(f))
                    {
                        if (f == Tobeshown)
                            fragmentTransaction.Show(f);
                        else
                            fragmentTransaction.Hide(f);
                    }

                }

                fragmentTransaction.Commit();

                return true;
            }
            else
            {
                FragmentTransaction fragmentTransaction = fragmentManager.BeginTransaction();

                foreach (var f in videoFrameLayoutFragments)
                {
                    fragmentTransaction.Hide(f);
                }

                fragmentTransaction.Commit();
            }

            return false;
        }
    }
}