using System;

namespace QuickDate.Helpers.Utils
{
    public class DoubleClickHandler
    {
        private readonly int _delayInMs;
        private DateTime _previousClickTime = DateTime.UtcNow;

        public DoubleClickHandler(int delayInMs = 200)
        {
            _delayInMs = delayInMs;
        }

        public void HandleDoubleClick(Action a)
        {
            if (CanClick(DateTime.UtcNow))
            {
                a.Invoke();
            }
        }

        private bool CanClick(DateTime newClickTime)
        {
            var diff = newClickTime.Subtract(_previousClickTime).TotalMilliseconds;
            _previousClickTime = newClickTime;
            return diff > _delayInMs;
        }
    }
}