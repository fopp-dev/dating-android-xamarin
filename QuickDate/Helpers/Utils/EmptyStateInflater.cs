using System;
using Android.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Com.Airbnb.Lottie;
using QuickDate.Helpers.Fonts;

namespace QuickDate.Helpers.Utils
{
   public class EmptyStateInflater
   {
        public Button EmptyStateButton;
        public TextView EmptyStateIcon;
        public TextView DescriptionText;
        public TextView TitleText;
        public LottieAnimationView LottieAnimationView;
        public enum Type
        {
            NoConnection,
            NoSearchResult,
            SomeThingWentWrong,
            NoUsers,
            NoMatches,
            NoNotifications,
            NoMessage,
            NoBlock,
            NoArticle,
            NoSessions,
            NoMedia,
            NoFriendsRequests,
            GetPremium,
        }
       
        public void InflateLayout(View inflated , Type type)
        {
            try
            {
                
                EmptyStateIcon = (TextView)inflated.FindViewById(Resource.Id.emtyicon);
                TitleText = (TextView)inflated.FindViewById(Resource.Id.headText);
                DescriptionText = (TextView)inflated.FindViewById(Resource.Id.seconderyText);
                EmptyStateButton = inflated.FindViewById<Button> (Resource.Id.button);
                LottieAnimationView = inflated.FindViewById<LottieAnimationView>(Resource.Id.animation_view);

                if (type == Type.NoConnection)
                {
                    LottieAnimationView.Visibility = ViewStates.Gone;
                    EmptyStateIcon.Visibility = ViewStates.Visible;
                    FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, EmptyStateIcon, IonIconsFonts.IosThunderstormOutline);
                    TitleText.Text = Application.Context.GetText(Resource.String.Lbl_NoConnection_TitleText);
                    DescriptionText.Text = Application.Context.GetText(Resource.String.Lbl_NoConnection_DescriptionText);
                    EmptyStateButton.Text = Application.Context.GetText(Resource.String.Lbl_NoConnection_Button);
                }
                else if (type == Type.NoSearchResult)
                {
                    LottieAnimationView.Visibility = ViewStates.Gone;
                    EmptyStateIcon.Visibility = ViewStates.Visible;
                    FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, EmptyStateIcon, IonIconsFonts.Search);
                    TitleText.Text = Application.Context.GetText(Resource.String.Lbl_NoSearchResult_TitleText);
                    DescriptionText.Text = Application.Context.GetText(Resource.String.Lbl_NoSearchResult_DescriptionText);
                    EmptyStateButton.Text = Application.Context.GetText(Resource.String.Lbl_NoSearchResult_Button);
                }
                else if (type == Type.SomeThingWentWrong)
                {
                    LottieAnimationView.Visibility = ViewStates.Gone;
                    EmptyStateIcon.Visibility = ViewStates.Visible;
                    FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, EmptyStateIcon, IonIconsFonts.Close);
                    TitleText.Text = Application.Context.GetText(Resource.String.Lbl_SomThingWentWrong_TitleText);
                    DescriptionText.Text = Application.Context.GetText(Resource.String.Lbl_SomThingWentWrong_DescriptionText);
                    EmptyStateButton.Text = Application.Context.GetText(Resource.String.Lbl_SomThingWentWrong_Button);
                }
                else if (type == Type.NoMatches)
                {
                    LottieAnimationView.Visibility = ViewStates.Gone;
                    EmptyStateIcon.Visibility = ViewStates.Visible;
                    FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, EmptyStateIcon, IonIconsFonts.Pin);
                    TitleText.Text = Application.Context.GetText(Resource.String.Lbl_NoMoreUsers);
                    DescriptionText.Text = " ";
                    EmptyStateButton.Visibility =ViewStates.Gone;
                } 
                else if (type == Type.NoUsers)
                {
                    LottieAnimationView.Visibility = ViewStates.Gone;
                    EmptyStateIcon.Visibility = ViewStates.Visible;
                    FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, EmptyStateIcon, IonIconsFonts.Person);
                    TitleText.Text = Application.Context.GetText(Resource.String.Lbl_NoMoreUsers);
                    DescriptionText.Text = " ";
                    EmptyStateButton.Visibility =ViewStates.Gone;
                } 
                else if (type == Type.NoNotifications)
                {
                    LottieAnimationView.Visibility = ViewStates.Visible;
                    EmptyStateIcon.Visibility = ViewStates.Gone;
                    //FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, EmptyStateIcon, IonIconsFonts.AndroidNotifications);
                    TitleText.Text = Application.Context.GetText(Resource.String.Lbl_NoNotification_TitleText);
                    DescriptionText.Text = Application.Context.GetText(Resource.String.Lbl_NoNotification_DescriptionText);
                    EmptyStateButton.Visibility = ViewStates.Gone;
                }
                else if (type == Type.NoMessage)
                {
                    LottieAnimationView.Visibility = ViewStates.Gone;
                    EmptyStateIcon.Visibility = ViewStates.Visible;
                    FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, EmptyStateIcon, IonIconsFonts.Chatbox);
                    TitleText.Text = Application.Context.GetText(Resource.String.Lbl_NoMessage_TitleText);
                    DescriptionText.Text = Application.Context.GetText(Resource.String.Lbl_NoMessage_DescriptionText) + " " + AppSettings.ApplicationName;
                    EmptyStateButton.Visibility = ViewStates.Gone;
                }
                else if (type == Type.NoBlock)
                {
                    LottieAnimationView.Visibility = ViewStates.Gone;
                    EmptyStateIcon.Visibility = ViewStates.Visible;
                    FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, EmptyStateIcon, IonIconsFonts.Person);
                    TitleText.Text = Application.Context.GetText(Resource.String.Lbl_NoBlockUsers);
                    DescriptionText.Text = " ";
                    EmptyStateButton.Visibility = ViewStates.Gone;
                }
                else if (type == Type.NoArticle)
                {
                    LottieAnimationView.Visibility = ViewStates.Gone;
                    EmptyStateIcon.Visibility = ViewStates.Visible;
                    FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, EmptyStateIcon, FontAwesomeIcon.FileAlt);
                    EmptyStateIcon.SetTextSize(ComplexUnitType.Dip, 45f);
                    TitleText.Text = Application.Context.GetText(Resource.String.Lbl_Empty_Article);
                    DescriptionText.Text = Application.Context.GetText(Resource.String.Lbl_Start_Article);
                    EmptyStateButton.Visibility = ViewStates.Gone;
                }
                else if (type == Type.NoSessions)
                {
                    LottieAnimationView.Visibility = ViewStates.Gone;
                    EmptyStateIcon.Visibility = ViewStates.Visible;
                    FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, EmptyStateIcon, FontAwesomeIcon.Fingerprint);
                    EmptyStateIcon.SetTextSize(ComplexUnitType.Dip, 45f);
                    TitleText.Text = Application.Context.GetText(Resource.String.Lbl_Empty_Sessions);
                    DescriptionText.Text = "";
                    EmptyStateButton.Visibility = ViewStates.Gone;
                }
                else if (type == Type.NoMedia)
                {
                    LottieAnimationView.Visibility = ViewStates.Gone;
                    EmptyStateIcon.Visibility = ViewStates.Visible;
                    FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, EmptyStateIcon, FontAwesomeIcon.Images);
                    EmptyStateIcon.SetTextSize(ComplexUnitType.Dip, 45f);
                    TitleText.Text = Application.Context.GetText(Resource.String.Lbl_Empty_Media);
                    DescriptionText.Text = "";
                    EmptyStateButton.Visibility = ViewStates.Gone;
                } 
                else if (type == Type.NoFriendsRequests)
                {
                    LottieAnimationView.Visibility = ViewStates.Gone;
                    EmptyStateIcon.Visibility = ViewStates.Visible;
                    FontUtils.SetTextViewIcon(FontsIconFrameWork.FontAwesomeLight, EmptyStateIcon, FontAwesomeIcon.UserFriends);
                    EmptyStateIcon.SetTextSize(ComplexUnitType.Dip, 45f);
                    TitleText.Text = Application.Context.GetText(Resource.String.Lbl_Empty_FriendsRequests);
                    DescriptionText.Text = Application.Context.GetText(Resource.String.Lbl_NoFriendsRequests);
                    EmptyStateButton.Visibility = ViewStates.Gone;
                }
                else if (type == Type.GetPremium)
                {
                    LottieAnimationView.Visibility = ViewStates.Gone;
                    EmptyStateIcon.Visibility = ViewStates.Visible;
                    FontUtils.SetTextViewIcon(FontsIconFrameWork.IonIcons, EmptyStateIcon, IonIconsFonts.Person);
                    EmptyStateIcon.SetTextSize(ComplexUnitType.Dip, 45f);
                    TitleText.Text = Application.Context.GetText(Resource.String.Lbl_Empty_GetPremium);
                    DescriptionText.Text = "";
                    EmptyStateButton.Visibility = ViewStates.Gone;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
           
        }
    }
}