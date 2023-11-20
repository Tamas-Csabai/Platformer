
using Main.DamageSystem;
using UnityEngine;
using Main.PlayerSystem;

namespace Main.StateMachineSystem
{
    public class AttackState : AgentState
    {

        public enum ExitCode
        {
            PlayerDetected = 0
        }

        [SerializeField] private Damager damager;
        [SerializeField] private float attackRange;

        private float _sqrAttackRange;

        private void Awake()
        {
            _sqrAttackRange = attackRange * attackRange;
        }

        protected override void Enter_Internal()
        {
            base.Enter_Internal();

            _agentController.SetFocus(_agentController.PlayerTransform);

            damager.StartDamageEntity(Player.Entity);
        }

        protected override void Exit_Internal()
        {
            base.Exit_Internal();

            damager.StopDamageEntity(Player.Entity);
        }

        public override void UpdateState()
        {
            if (!_agentController.IsPlayerInRangeSqr(_sqrAttackRange))
                Exit();
        }
    }
}
