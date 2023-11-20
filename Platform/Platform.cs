using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class Platform : MonoBehaviour
    {

        [SerializeField] private Transform[] targetPoints;
        [SerializeField] private float moveSpeed = 5f;

        private Coroutine move_Routine;
        private int _targetIndex;
        private Vector3 _moveDirection;

        public Vector3 Velocity => moveSpeed * _moveDirection;

        private void Start()
        {
            _targetIndex = -1;
            NextPoint();
        }

        private IEnumerator Move_Routine(Transform target)
        {
            while (true)
            {
                Vector3 targetDistance = target.position - transform.position;

                _moveDirection = targetDistance.normalized;

                transform.position += moveSpeed * Time.deltaTime * _moveDirection;

                if (targetDistance.magnitude < 0.1f)
                    NextPoint();

                yield return null;
            }
        }

        private void NextPoint()
        {
            _targetIndex++;

            if (_targetIndex >= targetPoints.Length)
                _targetIndex = 0;

            if (move_Routine != null)
                StopCoroutine(move_Routine);

            move_Routine = StartCoroutine(Move_Routine(targetPoints[_targetIndex]));
        }
    }
}
