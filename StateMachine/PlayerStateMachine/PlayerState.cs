
using Main.PlayerSystem;

namespace Main.StateMachineSystem
{
    public abstract class PlayerState : State
    {

        protected PlayerController _playerController;

        protected virtual void Awake()
        {
            _playerController = Player.Controller;
        }

    }
}
