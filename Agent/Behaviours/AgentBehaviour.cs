using Main.StateMachineSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.AgentSystem
{
    public abstract class AgentBehaviour : MonoBehaviour
    {

        private enum EvaluateImportance
        {
            Impoartant,
            Normal,
            NotImportant
        }

        [SerializeField] protected AgentStateMachine agentStateMachine;
        [SerializeField] private EvaluateImportance evaluateImportance;

        private Coroutine _evaluate_Routine;
        private float _evaluateInterval = 0.5f;
        private bool _isDisabled = false;

        protected virtual void Awake()
        {
            _evaluateInterval = evaluateImportance switch
            {
                EvaluateImportance.Impoartant => Consts.BEHAVIOUR_EVALUATE_INTERVAL_IMPORTANT,
                EvaluateImportance.Normal => Consts.BEHAVIOUR_EVALUATE_INTERVAL_NORMAL,
                EvaluateImportance.NotImportant => Consts.BEHAVIOUR_EVALUATE_INTERVAL_NOT_IMPORTANT,
                _ => 0.5f
            };
        }

        protected virtual void Start()
        {
            StartEvaluation();
        }

        protected virtual void OnEnable()
        {
            if (_isDisabled)
            {
                _isDisabled = false;
                StartEvaluation();
            }
        }

        protected virtual void OnDisable()
        {
            _isDisabled = true;

            StopEvaluation();
        }

        public virtual void Evaluate()
        {
            agentStateMachine.UpdateState();
        }

        protected void StartEvaluation()
        {
            if (_evaluate_Routine != null)
                StopCoroutine(_evaluate_Routine);

            _evaluate_Routine = StartCoroutine(Evaluate_Routine());
        }

        protected void StopEvaluation()
        {
            if (_evaluate_Routine != null)
                StopCoroutine(_evaluate_Routine);

            _evaluate_Routine = null;
        }

        protected IEnumerator Evaluate_Routine()
        {
            WaitForSeconds waitForEvaluate = new WaitForSeconds(_evaluateInterval);

            while (true)
            {
                Evaluate();
                yield return waitForEvaluate;
            }
        }

    }
}
