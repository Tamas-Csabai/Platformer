
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.AgentSystem
{
    public class SimpleMover : AgentMover
    {

        private Coroutine _move_Routine;
        private Coroutine _turn_Routine;

        protected override void StartMove()
        {
            if (_move_Routine != null)
                StopCoroutine(_move_Routine);

            _move_Routine = StartCoroutine(Move_Routine());
        }

        protected override void StopMove()
        {
            if (_move_Routine != null)
                StopCoroutine(_move_Routine);

            _move_Routine = null;
        }

        protected override void StartTurn()
        {
            if (_turn_Routine != null)
                StopCoroutine(_turn_Routine);

            _turn_Routine = StartCoroutine(Turn_Routine());
        }

        protected override void StopTurn()
        {
            if (_turn_Routine != null)
                StopCoroutine(_turn_Routine);

            _turn_Routine = null;
        }

        private IEnumerator Move_Routine()
        {
            while (CurrentDestinationTransform != null)
            {
                Vector3 targetDistance = CurrentDestinationTransform.position - agentController.RootTransform.position;

                Move(targetDistance.normalized);

                _sqrTargetPointDetectionRange = CurrentMoveSpeed * Time.deltaTime + 0.01f;

                if (targetDistance.sqrMagnitude <= _sqrTargetPointDetectionRange)
                {
                    DestinationReached();
                    yield break;
                }

                yield return null;
            }

            DestinationLost();
        }

        private IEnumerator Turn_Routine()
        {
            while (CurrentFocusTransform != null)
            {
                TurnTowards(CurrentFocusTransform.position);

                yield return null;
            }
        }

        private void Move(Vector3 direction)
        {
            agentController.RootTransform.position += CurrentMoveSpeed * Time.deltaTime * direction;
        }

        private void TurnTowards(Vector3 targetPoint)
        {
            Vector3 direction = Vector3.ProjectOnPlane(targetPoint - agentController.RootTransform.position, Vector3.up).normalized;

            Debug.DrawLine(agentController.RootTransform.position, agentController.RootTransform.position + direction, Color.blue);

            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

            agentController.RootTransform.rotation = Quaternion.RotateTowards(agentController.RootTransform.rotation, targetRotation, CurrentTurnSpeed * Time.deltaTime);
        }

    }
}
