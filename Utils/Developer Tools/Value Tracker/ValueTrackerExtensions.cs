namespace ValueTracker
{
    public static class ValueTrackerExtensions
    {

        public static T AddTracker<T>(this T o, string name) where T : class
        {
            ValueTracker.AddTracker(name, o);
            return o;
        }

        public static T AddTracker<T>(this T o, string name, ValueTracker.ObjectLogDelegate<T> logMethod) where T : class
        {
            ValueTracker.AddTracker(name, o, logMethod);
            return o;
        }

    }
}
