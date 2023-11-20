
using Main.AgentSystem;

namespace Main.StateMachineSystem
{
    public abstract class AgentState : State
    {

        protected AgentController _agentController;

        public void Initalize(AgentController agentController)
        {
            _agentController = agentController;
        }

    }
}
