using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.AgentSystem
{
    public abstract class AgentMover : MonoBehaviour
    {

        [SerializeField] protected AgentController agentController;

        [Header("Parameters")]
        [SerializeField] protected float targetPointDetectionRange = 0.5f;
        [SerializeField] protected float defaultMoveSpeed = 0.5f;
        [SerializeField] protected float defaultTurnSpeed = 180f;

        protected float _sqrTargetPointDetectionRange;

        public Action OnDestinationReached;
        public Action OnDestinationLost;

        public Transform CurrentDestinationTransform { get; private set; }
        public Transform CurrentFocusTransform { get; private set; }
        [field: SerializeField] public float CurrentMoveSpeed { get; private set; }
        [field: SerializeField] public float CurrentTurnSpeed { get; private set; }

        protected abstract void StartMove();
        protected abstract void StopMove();
        protected abstract void StartTurn();
        protected abstract void StopTurn();

        protected virtual void Awake()
        {
            _sqrTargetPointDetectionRange = targetPointDetectionRange * targetPointDetectionRange;

            CurrentMoveSpeed = defaultMoveSpeed;
            CurrentTurnSpeed = defaultTurnSpeed;
        }

        public virtual void SetDestination(Transform targetTransform, float moveSpeed)
        {
            CurrentDestinationTransform = targetTransform;
            CurrentMoveSpeed = moveSpeed;

            StartMove();
        }

        public virtual void SetDestination(Transform targetTransform)
        {
            SetDestination(targetTransform, defaultMoveSpeed);
        }

        public virtual void SetFocus(Transform focusTransform, float turnSpeed)
        {
            CurrentFocusTransform = focusTransform;
            CurrentTurnSpeed = turnSpeed;

            StartTurn();
        }

        public virtual void SetFocus(Transform focusTransform)
        {
            SetFocus(focusTransform, defaultTurnSpeed);
        }

        public virtual void ClearDestination()
        {
            CurrentDestinationTransform = null;

            StopMove();
        }

        public virtual void ClearFocus()
        {
            CurrentFocusTransform = null;

            StopTurn();
        }

        protected virtual void DestinationLost()
        {
            ClearDestination();

            OnDestinationLost?.Invoke();
        }

        protected virtual void DestinationReached()
        {
            ClearDestination();

            OnDestinationReached?.Invoke();
        }
    }
}
