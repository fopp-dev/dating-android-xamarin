using System;
using Android.App;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Developer.SEmojis.Helper;

namespace QuickDate.Helpers.Utils
{
    public class KeyboardService
    {
        public event EventHandler KeyboardIsShown;
        public event EventHandler KeyboardIsHidden;

        private InputMethodManager _inputMethodManager;

        private bool _wasShown = false;
        private readonly Activity _activity;
        private readonly View _mainView;

        public KeyboardService(Activity activity, View mainView)
        {
            _mainView = mainView;
            _activity = activity;
            GetInputMethodManager();
            SubscribeEvents();
        }

        public void OnGlobalLayout(object sender, EventArgs args)
        {
            GetInputMethodManager();
            if (!_wasShown && IsKeyboardShowing())
            { 
                KeyboardIsShown?.Invoke(this, EventArgs.Empty);
                _wasShown = true;
            }
            else if (_wasShown && !IsKeyboardShowing())
            {
                KeyboardIsHidden?.Invoke(this, EventArgs.Empty);
                _wasShown = false;
            }
        }

        public int CovertDpToPixel(int dp)
        {
            var displayMetrics = new DisplayMetrics();
            _activity.WindowManager.DefaultDisplay.GetMetrics(displayMetrics);
            return (int)(dp * displayMetrics.Density);
        }

        public void HideKeyboardIfShowing(EmojiconEditText emojiconEditText)
        {
            var currentFocus = _activity.CurrentFocus;
            if (currentFocus != null)
            {
                emojiconEditText.ClearFocus();
                _inputMethodManager.HideSoftInputFromWindow(currentFocus.WindowToken, HideSoftInputFlags.NotAlways);
            }
        }

        private bool IsKeyboardShowing()
        {

            var heightDiff = _mainView.RootView.Height - _mainView.Height;
            var isKeyboardShowing = heightDiff > CovertDpToPixel(200); //assume keyboard height

            return  isKeyboardShowing;
        }

        private void GetInputMethodManager()
        {
            if (_inputMethodManager == null || _inputMethodManager.Handle == IntPtr.Zero)
            {
                _inputMethodManager = (InputMethodManager)_activity.GetSystemService(Context.InputMethodService);
            }
        }

        private void SubscribeEvents()
        {
            _activity.Window.DecorView.ViewTreeObserver.GlobalLayout -= this.OnGlobalLayout;
            _activity.Window.DecorView.ViewTreeObserver.GlobalLayout += this.OnGlobalLayout;
        }
    }
}