using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class DebugLogger : MonoBehaviour
    {

        public string Message;
        public Object Object;

        public void Log(string message)
        {
            Debug.Log(message);
        }

        public void Log()
        {
            Debug.Log(Message, Object == null ? this : Object);
        }

    }
}
