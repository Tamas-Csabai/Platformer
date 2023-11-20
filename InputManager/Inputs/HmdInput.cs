namespace Main.Input
{
    public class HmdInput : InputAction
    {

        public System.Action OnPutHead;
        public System.Action OnRemoveHead;

        protected override void Setup()
        {
            _inputAction.started += Started;
            _inputAction.canceled += Canceled;
        }

        private void Started(UnityEngine.InputSystem.InputAction.CallbackContext callbackContext)
        {
            OnPutHead?.Invoke();
        }

        private void Canceled(UnityEngine.InputSystem.InputAction.CallbackContext callbackContext)
        {
            OnRemoveHead?.Invoke();
        }
    }
}