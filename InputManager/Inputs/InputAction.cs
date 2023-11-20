namespace Main.Input
{
    public abstract class InputAction
    {
        protected UnityEngine.InputSystem.InputAction _inputAction;

        public void Initialize(UnityEngine.InputSystem.InputAction inputAction)
        {
            _inputAction = inputAction;
            Setup();
        }

        protected abstract void Setup();

    }
}