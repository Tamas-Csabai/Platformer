namespace Main.Input
{
    using UnityEngine;

    public class ThumbstickInput : InputAction
    {
        public float threshold = 0.6f;
        public System.Action<Vector2> OnChanged;
        public System.Action OnWest;
        public System.Action OnEast;
        public System.Action OnNorth;
        public System.Action OnSouth;

        private Vector2 value;
        private bool isWest = false;
        private bool isEast = false;
        private bool isNorth = false;
        private bool isSouth = false;

        private bool westOnDown = false;
        private bool eastOnDown = false;
        private bool northOnDown = false;
        private bool southOnDown = false;

        protected override void Setup()
        {
            _inputAction.performed += Performed;
        }

        public virtual Vector2 Get()
        {
            return _inputAction.ReadValue<Vector2>();
        }

        protected void GetPositiveAxis(float value, ref bool isOn, ref bool onDown, System.Action action)
        {
            if (!isOn)
            {
                if (value >= threshold)
                {
                    isOn = true;
                    action?.Invoke();
                    onDown = true;
                    return;
                }
            }
            else
            {
                if (value < threshold)
                    isOn = false;
            }

            onDown = false;
        }

        protected void GetNegativeAxis(float value, ref bool isOn, ref bool onDown, System.Action action)
        {
            if (!isOn)
            {
                if (value <= -threshold)
                {
                    isOn = true;
                    action?.Invoke();
                    onDown = true;
                    return;
                }
            }
            else
            {
                if (value > -threshold)
                    isOn = false;
            }

            onDown = false;
        }
        
        public bool GetWest()
        {
            return westOnDown;
        }

        public bool GetEast()
        {
            return eastOnDown;
        }

        public bool GetNorth()
        {
            return northOnDown;
        }

        public bool GetSouth()
        {
            return southOnDown;
        }
        
        private void Performed(UnityEngine.InputSystem.InputAction.CallbackContext callbackContext)
        {
            value = callbackContext.ReadValue<Vector2>();
            OnChanged?.Invoke(value);
            GetNegativeAxis(value.x, ref isWest, ref westOnDown, OnWest);
            GetPositiveAxis(value.x, ref isEast, ref eastOnDown, OnEast);
            GetPositiveAxis(value.y, ref isNorth, ref northOnDown, OnNorth);
            GetNegativeAxis(value.y, ref isSouth, ref southOnDown, OnSouth);
        }

    }
}
