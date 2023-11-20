
using Main.AgentSystem;
using UnityEngine;


namespace Main.StateMachineSystem
{
    public class AgentStateMachine : StateMachine<AgentState>
    {

        [SerializeField] private AgentController agentController;

        private AgentState[] _agentStates;

        protected override void Awake()
        {
            _agentStates = GetComponentsInChildren<AgentState>();

            for (int i = 0; i < _agentStates.Length; i++)
                _agentStates[i].Initalize(agentController);

            base.Awake();
        }

    }
}
