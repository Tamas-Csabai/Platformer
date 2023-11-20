namespace Main.Input
{
    public class TriggerInput : InputAction
    {
        private float value;
        private bool isDown = false;

        public float downThreshold = 0.8f;
        public float upThreshold = 0.6f;
        public System.Action<float> OnPulling;
        public System.Action OnDown;
        public System.Action OnUp;

        protected override void Setup()
        {
            _inputAction.performed += Performed;
        }

        public virtual float Get()
        {
            return _inputAction.ReadValue<float>();
        }

        /*
        public bool GetDown()
        {
            return isDown;
        }
        */

        private bool GetDown(float value)
        {
            if (!isDown && value >= downThreshold)
            {
                isDown = true;
                OnDown?.Invoke();
                return true;
            }

            return false;
        }

        /*
        public bool GetUp()
        {
            return !isDown;
        }
        */

        private bool GetUp(float value)
        {
            if (isDown && value < upThreshold)
            {
                isDown = false;
                OnUp?.Invoke();
                return true;
            }

            return false;
        }

        private void Performed(UnityEngine.InputSystem.InputAction.CallbackContext callbackContext)
        {
            value = _inputAction.ReadValue<float>();
            OnPulling?.Invoke(value);
            GetDown(value);
            GetUp(value);
        }
    }
}
