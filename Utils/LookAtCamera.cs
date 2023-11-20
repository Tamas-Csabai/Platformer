using Main.PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class LookAtCamera : MonoBehaviour
    {

        private Quaternion _defaultLookRotation;

        private void Awake()
        {
            _defaultLookRotation = transform.localRotation;
        }

        private void Update()
        {
            transform.LookAt(Player.Controller.Head);
        }

        public void ResetToDefault()
        {
            transform.localRotation = _defaultLookRotation;
        }

    }
}
