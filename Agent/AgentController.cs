
using Main.PlayerSystem;
using System;
using System.Collections;
using UnityEngine;

namespace Main.AgentSystem
{
    public class AgentController : MonoBehaviour
    {

        [Header("References")]
        [SerializeField] private Transform rootTransform;
        [SerializeField] private AgentMover agentMover;

        public Action OnDestinationReached;
        public Action OnDestinationLost;

        public Transform PlayerTransform => Player.GameObject.transform;
        public Transform RootTransform => rootTransform;

        private void Awake()
        {
            agentMover.OnDestinationReached += DestinationReached;
            agentMover.OnDestinationLost += DestinationLost;
        }

        private void OnDestroy()
        {
            if(agentMover != null)
            {
                agentMover.OnDestinationReached -= DestinationReached;
                agentMover.OnDestinationLost -= DestinationLost;
            }
        }

        public void SetDestination(Transform targetTransform, float moveSpeed)
        {
            agentMover.SetDestination(targetTransform, moveSpeed);
        }

        public void SetDestination(Transform targetTransform)
        {
            agentMover.SetDestination(targetTransform);
        }

        public void SetFocus(Transform focusTransform, float turnSpeed)
        {
            agentMover.SetFocus(focusTransform, turnSpeed);
        }

        public void SetFocus(Transform focusTransform)
        {
            agentMover.SetFocus(focusTransform);
        }

        public bool IsPlayerInRangeSqr(float sqrRange)
        {
            if (Player.CheckRangeSqr(rootTransform, sqrRange))
                return true;

            return false;
        }

        public bool IsPlayerInRange(float range)
        {
            if (Player.CheckRange(rootTransform, range))
                return true;

            return false;
        }

        private void DestinationReached()
        {
            OnDestinationReached?.Invoke();
        }

        private void DestinationLost()
        {
            OnDestinationLost?.Invoke();
        }

    }
}
