﻿using System;
using Android.App;
using QuickDate.Activities.Tabbes;

namespace QuickDate.Helpers.Model
{
    public static class UserDetails
    {
        public static string AccessToken = "";
        public static int UserId;
        public static string Username = "";
        public static string FullName = "";
        public static string Password = "";
        public static string Email = "";
        public static string Cookie = "";
        public static string Status = "";
        public static string Avatar = "";
        public static string Cover = "";
        public static string DeviceId = "";
        public static string LangName = "";
        public static string IsPro = "";
        public static string Url = "";

        public static string Lat = "";
        public static string Lng = "";
        public static string Located = GlobalConstants.FilterOptionDistance; //DistanceCount
        public static int AgeMin = GlobalConstants.FilterOptionAgeMin, AgeMax = GlobalConstants.FilterOptionAgeMax;
        public static string Gender = GlobalConstants.FilterOptionGender, Location = "";
        public static bool SwitchState;

        //new 
        public static string Language = GlobalConstants.FilterOptionLanguage;
        public static string Ethnicity = "";
        public static string Religion = "";

        public static string RelationShip= "";
        public static string Smoke = "";
        public static string Drink = "";

        public static string FromHeight = GlobalConstants.FilterOptionFromHeight;
        public static string ToHeight = GlobalConstants.FilterOptionToHeight;
        public static string Body = "";

        public static string Interest = "";
        public static string Education = "";
        public static string Pets = "";
       
        public static bool NotificationPopup { get; set; } = true;
         
        public static int UnixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        public static string Time = UnixTimestamp.ToString();

        public static string AndroidId = Android.Provider.Settings.Secure.GetString(Application.Context.ContentResolver, Android.Provider.Settings.Secure.AndroidId);
         
        public static void ClearAllValueUserDetails()
        {
            try
            {
                AccessToken = string.Empty;
                UserId = 0;
                Username = string.Empty;
                FullName = string.Empty;
                Password = string.Empty;
                Email = string.Empty;
                Cookie = string.Empty;
                Status = string.Empty;
                Avatar = string.Empty;
                Cover = string.Empty;
                DeviceId = string.Empty;
                LangName = string.Empty;
                Lat = string.Empty;
                Lng = string.Empty;
                Located = string.Empty;
                Gender = GlobalConstants.FilterOptionGender;
                Location = string.Empty;
                SwitchState = true;

                AgeMin = GlobalConstants.FilterOptionAgeMin;
                AgeMax = GlobalConstants.FilterOptionAgeMax;

                HomeActivity.CountNotificationsStatic = 0;
                HomeActivity.CountMessagesStatic = 0;
            }
            catch (Exception e)
            {
               Console.WriteLine(e);
            }
        }

    }
}