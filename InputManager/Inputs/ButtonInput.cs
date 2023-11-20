namespace Main.Input
{
    public class ButtonInput : InputAction
    {
        public System.Action OnDown;
        public System.Action OnUp;

        protected delegate bool GetInputMethod();
        protected GetInputMethod GetMethod;
        protected GetInputMethod GetDownMethod;
        protected GetInputMethod GetUpMethod;

        protected override void Setup()
        {
            GetMethod = Get_Internal;
            GetDownMethod = GetDown_Internal;
            GetUpMethod = GetUp_Internal;

            _inputAction.performed += Performed;
            _inputAction.canceled += Canceled;
        }

        public bool Get()
        {
            return GetMethod.Invoke();
        }

        protected virtual bool Get_Internal()
        {
            return _inputAction.IsPressed();
        }

        public bool GetDown()
        {
            return GetDownMethod.Invoke();
        }

        protected bool GetDown_Internal()
        {
            return _inputAction.WasPressedThisFrame();
        }
        
        public bool GetUp()
        {
            return GetUpMethod.Invoke();
        }

        protected bool GetUp_Internal()
        {
            return _inputAction.WasReleasedThisFrame();
        }
        
        protected void Canceled(UnityEngine.InputSystem.InputAction.CallbackContext callbackContext)
        {
            OnUp?.Invoke();
        }

        protected void Performed(UnityEngine.InputSystem.InputAction.CallbackContext callbackContext)
        {
            OnDown?.Invoke();
        }

    }
}

