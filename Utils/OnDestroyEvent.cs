using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class OnDestroyEvent : MonoBehaviour
    {

        public Action OnEvent;

        private void OnDestroy()
        {
            OnEvent?.Invoke(); 
        }

    }
}
