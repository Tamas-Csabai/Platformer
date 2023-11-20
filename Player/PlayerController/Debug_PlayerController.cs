using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class Debug_PlayerController : MonoBehaviour
    {

        public Camera playerCamera;
        public float lookSpeed = 2.0f;
        public float lookXLimit = 45.0f;

        float rotationX = 0;
        bool cursorIsLocked;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            cursorIsLocked = true;
        }

        void Update()
        {

            if (UnityEngine.Input.GetKeyDown(KeyCode.C))
            {
                if (cursorIsLocked)
                {
                    cursorIsLocked = false;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else
                {
                    cursorIsLocked = true;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }

            rotationX += -UnityEngine.Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, UnityEngine.Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }
}
