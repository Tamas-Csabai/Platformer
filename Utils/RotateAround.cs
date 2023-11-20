using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class RotateAround : MonoBehaviour
    {

        [SerializeField] private Transform centerTransform;
        [SerializeField] private Vector3 axis;
        [SerializeField] private float speedInAngle = 180f;

        private void Update()
        {
            transform.RotateAround(centerTransform.position, axis, speedInAngle * Time.deltaTime);
        }

    }
}
