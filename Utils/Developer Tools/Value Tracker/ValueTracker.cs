namespace ValueTracker
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class ValueTracker : MonoBehaviour
    {

        public delegate string ObjectLogDelegate<T>(T obj);
        public delegate string LogDelegate();

        private abstract class Tracker
        {
            public static readonly string Error = "Error";

            public string Name;

            public abstract string Log();
            public abstract string LogUnsafe();
        }

        private class ObjectTracker<T> : Tracker
        {
            public T Object;
            public ObjectLogDelegate<T> LogMethod;

            public ObjectTracker(string name, T obj, ObjectLogDelegate<T> logMethod)
            {
                Name = name;
                Object = obj;
                LogMethod = logMethod;
            }

            public override string Log()
            {
                return LogMethod.Invoke(Object);
            }

            public override string LogUnsafe()
            {
                return Log();
            }
        }

        private class SimpleTracker : Tracker
        {
            public LogDelegate LogMethod;

            public SimpleTracker(string name, LogDelegate logMethod)
            {
                Name = name;
                LogMethod = logMethod;
            }

            public override string Log()
            {
                string log = ""; 

                try
                {
                    log = LogMethod.Invoke();
                }
                catch
                {
                    log = Error;
                }

                return log;
            }

            public override string LogUnsafe()
            {
                return LogMethod.Invoke();
            }
        }

        private readonly string windowTitle = "Value Tracker";
        private Rect windowRect = new Rect(Screen.width - 350, Screen.height - 110, 350, 15);
        private string log;

        private static ValueTracker instance;
        private static List<Tracker> trackers = new List<Tracker>();

        private void Awake()
        {
            if (!Debug.isDebugBuild)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);

            SceneManager.sceneUnloaded += SceneUnloaded;
        }

        private void OnGUI()
        {
            if(trackers.Count > 0)
            {
                windowRect.height = (trackers.Count * 30f) + 15f;
                windowRect = GUI.Window(GetInstanceID(), windowRect, DrawWindow, windowTitle);
            }
        }

        private void DrawWindow(int windowID)
        {
            foreach(Tracker tracker in trackers)
            {
                log = tracker.Log();
                if (log.Equals(Tracker.Error))
                {
                    GUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));
                    
                    GUILayout.Label(tracker.Name + ": ", GUILayout.ExpandWidth(false));

                    if (GUILayout.Button(Tracker.Error, GUILayout.Width(70f), GUILayout.ExpandWidth(false)))
                        tracker.LogUnsafe();

                    GUILayout.EndHorizontal();
                }
                else
                    GUILayout.Label(tracker.Name + ": " + log);
            }
                
            GUI.DragWindow();
        }

        private static void ValidateWindow()
        {
            if (instance == null)
                instance = new GameObject("[" + nameof(ValueTracker) + "]", typeof(ValueTracker)).GetComponent<ValueTracker>();
        }

        private static void SceneUnloaded(Scene unloadedScene)
        {
            List<int> nullTrackerIndexes = new List<int>();

            for (int i = 0; i < trackers.Count; i++)
            {
                if (trackers[i] == null)
                    nullTrackerIndexes.Add(i);
            }

            foreach(int trackerIndex in nullTrackerIndexes)
                trackers.RemoveAt(trackerIndex);
        }

        public static void AddTracker<T>(string name, T obj, ObjectLogDelegate<T> logMethod)
        {
            if (!Debug.isDebugBuild) return;

            if (trackers.Find(x => x.Name.Equals(name)) == null)
                trackers.Add(new ObjectTracker<T>(name, obj, logMethod));
            else
                Debug.Log("<b>Value Tracker:</b> there is already a tracker named '" + name + "'!");

            ValidateWindow();
        }

        public static void AddTracker<T>(string name, T obj)
        {
            if (!Debug.isDebugBuild) return;

            if (trackers.Find(x => x.Name.Equals(name)) == null)
                trackers.Add(new ObjectTracker<T>(name, obj, DefaultLogMethod));
            else
                Debug.Log("<b>Value Tracker:</b> there is already a tracker named '" + name + "'!");

            ValidateWindow();
        }

        public static void AddTracker(string name, LogDelegate logMethod)
        {
            if (!Debug.isDebugBuild) return;

            if (trackers.Find(x => x.Name.Equals(name)) == null)
                trackers.Add(new SimpleTracker(name, logMethod));
            else
                Debug.Log("<b>Value Tracker:</b>Can't add tracker! There is already a tracker named '" + name + "'!");

            ValidateWindow();
        }

        public static void RemoveTracker(string name)
        {
            if (!Debug.isDebugBuild) return;

            Tracker tracker = trackers.Find(x => x.Name.Equals(name));

            if (tracker != null)
                trackers.Remove(tracker);
            else
                Debug.Log("<b>Value Tracker:</b> Can't remove tracker! there is no tracker added that named '" + name + "'!");

            ValidateWindow();
        }

        public static void RemoveAllTrackerOfObject<T>(T obj)
        {
            if (!Debug.isDebugBuild) return;

            trackers.RemoveAll(x => x is ObjectTracker<T> && ((ObjectTracker<T>)x).Object.Equals(obj));

            ValidateWindow();
        }

        public static void ClearAllTrackers()
        {
            if (!Debug.isDebugBuild) return;

            trackers.Clear();

            ValidateWindow();
        }

        private static string DefaultLogMethod<T>(T obj)
        {
            if (obj != null)
                return obj.ToString();

            return "null";
        }

    }
}
