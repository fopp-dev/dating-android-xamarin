using Android.App;
using Android.Content;
using Newtonsoft.Json;
using QuickDate.Helpers.Model;
using System;

namespace QuickDate.Activities.SharedPreferences
{
    public class AppPreferences
    {
        private ISharedPreferences _sharedPreferences;

        private const string SHOW_TUTORAL_DIALOG_KEY = "SHOW_TUTORAL_DIALOG_KEY";
        private const string SHOW_WALK_THROUGH_PAGE_KEY = "SHOW_WALK_THROUGH_PAGE_KEY";
        private const string SWIPE_COUNT_DETAILS_KEY = "SWIPE_COUNT_DETAILS_KEY";
        private const string PREF_NAME = "APP_SETTINGS";

        public AppPreferences()
        {
            _sharedPreferences = Application.Context.GetSharedPreferences(PREF_NAME, FileCreationMode.Private);
        }

        public void StoreShowTutorialDialogValue(bool showTutorialDialogAgain)
        {
            _sharedPreferences
                .Edit()
                .PutBoolean(SHOW_TUTORAL_DIALOG_KEY, showTutorialDialogAgain)
                .Commit();
        }

        public bool GetShowTutorialDialogValue()
        {
            return _sharedPreferences.GetBoolean(SHOW_TUTORAL_DIALOG_KEY, true);
        }

        public void StoreShowWalkThroughPageValue(bool showWalkThroughPageAgain)
        {
            _sharedPreferences
                .Edit()
                .PutBoolean(SHOW_WALK_THROUGH_PAGE_KEY, showWalkThroughPageAgain)
                .Commit();
        }

        public bool GetShowWalkThroughPageValue()
        {
            return _sharedPreferences.GetBoolean(SHOW_WALK_THROUGH_PAGE_KEY, true);
        }

        public void StoreSwipeCountValue(int swipeCount)
        {
            var swipeDetail = new SwipeLimitDetails()
            {
                SwapCount = swipeCount,
                LastSwapDate = DateTime.UtcNow
            };

            var swipeDetailAsJsonString = JsonConvert.SerializeObject(swipeDetail);

            _sharedPreferences
                .Edit()
                .PutString(SWIPE_COUNT_DETAILS_KEY, swipeDetailAsJsonString)
                .Commit();
        }

        public bool CanSwipeMore(int maxSwapLimit)
        {
            var swipeDetailJsonString = _sharedPreferences.GetString(SWIPE_COUNT_DETAILS_KEY, null);

            if(swipeDetailJsonString == null)
            {
                return true;
            }
             
            var swipeDetail = JsonConvert.DeserializeObject<SwipeLimitDetails>(swipeDetailJsonString);

            return swipeDetail.CanSwipe(maxSwapLimit);
        }

        public int GetSwipeCountValue()
        {
            var swipeDetailJsonString = _sharedPreferences.GetString(SWIPE_COUNT_DETAILS_KEY, null);

            if (swipeDetailJsonString == null)
            {
                return 0;
            }

            var swipeDetail = JsonConvert.DeserializeObject<SwipeLimitDetails>(swipeDetailJsonString);

            return swipeDetail.GetSwipeCount();
        }
    }
}