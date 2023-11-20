using System.Collections;
using UnityEngine;

namespace Main
{
    public abstract class RoutineBehaviour : MonoBehaviour
    {

        private Coroutine _routine;
        
        protected abstract IEnumerator Routine();

        public void StartRoutine(bool forceRestart = true)
        {
            if (_routine != null)
            {
                if (forceRestart)
                {
                    StopCoroutine(_routine);

                    _routine = StartCoroutine(Routine());
                }
            }
            else
            {
                _routine = StartCoroutine(Routine());
            }
        }

        public void StopRoutine()
        {
            if (_routine != null)
                StopCoroutine(_routine);

            _routine = null;
        }

    }
}
