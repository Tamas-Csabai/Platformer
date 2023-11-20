
using UnityEngine;

namespace Main.StateMachineSystem
{
    public class PatrolState : AgentState
    {

        public enum ExitCode
        {
            PlayerDetected = 0
        }

        [SerializeField] private Transform patrolPointGroup;
        [SerializeField] private bool detectPlayer = true;
        [SerializeField] private float playerDetectionRange = 3f;
        [SerializeField] private float patrolSpeed = 0.5f;

        private float _sqrPlayerDetectionRange;
        private int _currentTargetPatrolPointIndex = 0;

        private void Awake()
        {
            _sqrPlayerDetectionRange = playerDetectionRange * playerDetectionRange;
        }

        private void Start()
        {
            patrolPointGroup.name = "Patrol Point Group - " + _agentController.RootTransform.name;
            patrolPointGroup.SetParent(null, true);
        }

        protected override void Enter_Internal()
        {
            base.Enter_Internal();

            _agentController.OnDestinationReached += NextTargetPoint;
            _agentController.OnDestinationLost += NextTargetPoint;

            Transform closestPatrolPointTransform = patrolPointGroup.GetClosestInChildren(_agentController.RootTransform);
            _currentTargetPatrolPointIndex = closestPatrolPointTransform.GetSiblingIndex() - 1;

            NextTargetPoint();
        }

        protected override void Exit_Internal()
        {
            _agentController.OnDestinationReached -= NextTargetPoint;
            _agentController.OnDestinationLost -= NextTargetPoint;

            base.Exit_Internal();
        }

        public override void UpdateState()
        {
            if (detectPlayer && _agentController.IsPlayerInRangeSqr(_sqrPlayerDetectionRange))
                Exit((int)ExitCode.PlayerDetected);
        }

        private void NextTargetPoint()
        {
            _currentTargetPatrolPointIndex++;

            if (_currentTargetPatrolPointIndex >= patrolPointGroup.childCount)
                _currentTargetPatrolPointIndex = 0;

            Transform targetTransform = patrolPointGroup.GetChild(_currentTargetPatrolPointIndex);

            _agentController.SetDestination(targetTransform, patrolSpeed);
            _agentController.SetFocus(targetTransform);
        }

    }
}
