using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.StateMachineSystem
{
    public abstract class State : MonoBehaviour
    {

        public Action OnEnter;
        public Action<int> OnExit;

        public void Enter()
        {
            Enter_Internal();

            OnEnter?.Invoke();
        }

        public void Exit(int exitCode = 0)
        {
            Exit_Internal();

            OnExit?.Invoke(exitCode);
        }

        protected virtual void Enter_Internal() { }

        protected virtual void Exit_Internal() { }

        public abstract void UpdateState();
    }
}
