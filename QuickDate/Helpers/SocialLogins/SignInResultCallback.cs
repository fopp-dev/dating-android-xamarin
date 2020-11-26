﻿using System;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common.Apis;
using QuickDate.Activities.Default;
using Exception = Java.Lang.Exception;
using Object = Java.Lang.Object;

namespace QuickDate.Helpers.SocialLogins
{
    public class SignInResultCallback : Object, IResultCallback
    {
        public SocialLoginBaseActivity Activity { get; set; }

        public void OnResult(Object result)
        {
            try
            {
                var googleSignInResult = result as GoogleSignInResult;
                Activity.HandleSignInResult(googleSignInResult);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        } 
    }
}