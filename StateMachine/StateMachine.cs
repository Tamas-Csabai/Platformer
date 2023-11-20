
using UnityEngine;

namespace Main.StateMachineSystem
{
    public abstract class StateMachine<T> : MonoBehaviour where T : State
    {

        [SerializeField] protected T defaultState;

        protected T _currentState;

        protected virtual void Awake()
        {
            _currentState = defaultState;
        }

        protected virtual void Start()
        {
            _currentState.Enter();
        }

        public void UpdateState()
        {
            _currentState.UpdateState();
        }

        public void ChangeState(T newState)
        {
            _currentState.Exit();

            _currentState = newState;

            _currentState.Enter();
        }

        public void NextState(T nextState)
        {
            _currentState = nextState;

            _currentState.Enter();
        }

    }
}
