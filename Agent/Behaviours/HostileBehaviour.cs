
using Main.StateMachineSystem;
using UnityEngine;

namespace Main.AgentSystem
{
    public class HostileBehaviour : AgentBehaviour
    {

        [Header("Agent States")]
        [SerializeField] private PatrolState patrolState;
        [SerializeField] private ChaseState chaseState;
        [SerializeField] private AttackState attackState;

        protected override void Awake()
        {
            base.Awake();

            patrolState.OnExit += ExitPatrol;
            chaseState.OnExit += ExitChase;
            attackState.OnExit += ExitAttack;
        }

        private void OnDestroy()
        {
            if(patrolState != null)
                patrolState.OnExit -= ExitPatrol;

            if (chaseState != null)
                chaseState.OnExit -= ExitChase;

            if(attackState != null)
                attackState.OnExit -= ExitAttack;
        }

        private void ExitPatrol(int exitCode)
        {
            agentStateMachine.NextState(chaseState);
        }

        private void ExitChase(int exitCode)
        {
            switch ((ChaseState.ExitCode)exitCode)
            {
                case ChaseState.ExitCode.PlayerLost:
                    agentStateMachine.NextState(patrolState);
                    break;

                case ChaseState.ExitCode.PlayerReached:
                    agentStateMachine.NextState(attackState);
                    break;
            }
        }

        private void ExitAttack(int exitCode)
        {
            agentStateMachine.NextState(chaseState);
        }

    }
}
