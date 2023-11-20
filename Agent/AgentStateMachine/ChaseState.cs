
using UnityEngine;

namespace Main.StateMachineSystem
{
    public class ChaseState : AgentState
    {

        public enum ExitCode
        {
            PlayerLost = 0,
            PlayerReached = 1
        }

        [SerializeField] private float chaseSpeed = 0.7f;
        [SerializeField] private float playerDetectionRange = 3f;

        private float _sqrPlayerDetectionRange;

        private void Awake()
        {
            _sqrPlayerDetectionRange = playerDetectionRange * playerDetectionRange;
        }

        public override void UpdateState()
        {
            if (!_agentController.IsPlayerInRangeSqr(_sqrPlayerDetectionRange))
                Exit((int)ExitCode.PlayerLost);
        }

        protected override void Enter_Internal()
        {
            base.Enter_Internal();

            _agentController.OnDestinationReached += TargetReached;
            _agentController.OnDestinationLost += TargetLost;

            _agentController.SetDestination(_agentController.PlayerTransform, chaseSpeed);
            _agentController.SetFocus(_agentController.PlayerTransform);
        }

        protected override void Exit_Internal()
        {
            _agentController.OnDestinationReached -= TargetReached;
            _agentController.OnDestinationLost -= TargetLost;

            base.Exit_Internal();
        }

        private void TargetReached()
        {
            Exit((int)ExitCode.PlayerReached);
        }

        private void TargetLost()
        {
            Exit((int)ExitCode.PlayerLost);
        }

    }
}
