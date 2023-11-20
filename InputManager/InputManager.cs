using UnityEngine;
using UnityEngine.XR;

namespace Main.Input
{
    public class InputManager : MonoBehaviour, ISetup
    {
        private static MainControllerActions mainControllerActions;
        private static float grabDownThreshold = 0.6f;
        private static float grabUpThreshold = 0.4f;
        private static float triggerDownThreshold = 0.55f;
        private static float triggerUpThreshold = 0.4f;
        private static float thumbstickThreshold = 0.9f;
        private static bool canMove = true;
        private static bool canUse = true;

        public static bool CanMove { get => canMove; set { canMove = value; CanMoveChanged?.Invoke(value); } }
        public static bool CanUse { get => canUse; set { canUse = value; CanUseChanged?.Invoke(value); } }
        public static bool IsSmoothTurn { get; set; } = true;
        public static bool CanTurningWhileClimbing { get; set; } = true;
        public static float SnapTurnAngle { get; set; } = 45f;
        public static float SnapTurnThreshold
        {
            get => thumbstickThreshold;
            set
            {
                thumbstickThreshold = value;
                leftThumbstick.threshold = value;
                rightThumbstick.threshold = value;
            }
        }
        public static float SmoothTurnSpeed { get; set; } = 5f;
        public static float GrabDownThreshold
        {
            get => grabDownThreshold;
            set
            {
                grabDownThreshold = value;
                leftGrab.downThreshold = value;
                rightGrab.downThreshold = value;
            }
        }
        public static float GrabUpThreshold
        {
            get => grabUpThreshold;
            set
            {
                grabUpThreshold = value;
                leftGrab.upThreshold = value;
                rightGrab.upThreshold = value;
            }
        }
        public static float TriggerDownThreshold
        {
            get => triggerDownThreshold;
            set
            {
                triggerDownThreshold = value;
                leftTrigger.downThreshold = value;
                rightTrigger.downThreshold = value;
            }
        }
        public static float TriggerUpThreshold
        {
            get => triggerUpThreshold;
            set
            {
                triggerUpThreshold = value;
                leftTrigger.upThreshold = value;
                rightTrigger.upThreshold = value;
            }
        }

        public static System.Action<bool> CanMoveChanged;
        public static System.Action<bool> CanUseChanged;

        #region Inputs
        private static readonly HmdInput hmd = new();
        private static readonly ThumbstickInput leftThumbstick = new();
        private static readonly ThumbstickInput rightThumbstick = new();
        private static readonly TriggerInput leftGrab = new();
        private static readonly TriggerInput rightGrab = new();
        private static readonly TriggerInput leftTrigger = new();
        private static readonly TriggerInput rightTrigger = new();
        private static readonly ButtonInput leftThumbstickClick = new();
        private static readonly ButtonInput rightThumbstickClick = new();
        private static readonly ButtonInput buttonA = new();
        private static readonly ButtonInput buttonB = new();
        private static readonly ButtonInput buttonX = new();
        private static readonly ButtonInput buttonY = new();
        private static readonly ButtonInput leftMenu = new();
        private static readonly ButtonInput rightMenu = new();
        #endregion

        #region Input Actions
        public static HmdInput HMD => hmd;
        public static ThumbstickInput Walk => leftThumbstick;
        public static ButtonInput ResetHeight => leftThumbstickClick;
        public static ButtonInput Jump => buttonA;
        public static ButtonInput Crouch => buttonB;
        public static ButtonInput Menu => buttonY;
        public static TriggerInput LeftGrab => leftGrab;
        public static TriggerInput RightGrab => rightGrab;
        public static TriggerInput LeftTrigger => leftTrigger;
        public static TriggerInput RightTrigger => RightTrigger;
        public static ThumbstickInput Turn => rightThumbstick;
        public static ButtonInput Fly => buttonX;
        public static ThumbstickInput Levitate => rightThumbstick;
        public static UnityEngine.InputSystem.InputAction LeftHaptic => mainControllerActions.LeftHand.Haptic;
        public static UnityEngine.InputSystem.InputAction RightHaptic => mainControllerActions.RightHand.Haptic;
        #endregion

        void ISetup.Setup()
        {
            mainControllerActions = new MainControllerActions();
            mainControllerActions.Enable();

            hmd.Initialize(mainControllerActions.HMD.hmdPresence);
            leftThumbstick.Initialize(mainControllerActions.LeftHand.Primary2DAxis);
            rightThumbstick.Initialize(mainControllerActions.RightHand.Primary2DAxis);
            leftGrab.Initialize(mainControllerActions.LeftHand.Grip);
            rightGrab.Initialize(mainControllerActions.RightHand.Grip);
            leftTrigger.Initialize(mainControllerActions.LeftHand.Trigger);
            rightTrigger.Initialize(mainControllerActions.RightHand.Trigger);
            leftThumbstickClick.Initialize(mainControllerActions.LeftHand.Primary2DAxisClick);
            rightThumbstickClick.Initialize(mainControllerActions.RightHand.Primary2DAxisClick);
            buttonA.Initialize(mainControllerActions.RightHand.PrimaryButton);
            buttonB.Initialize(mainControllerActions.RightHand.SecondaryButton);
            buttonX.Initialize(mainControllerActions.LeftHand.PrimaryButton);
            buttonY.Initialize(mainControllerActions.LeftHand.SecondaryButton);
            leftMenu.Initialize(mainControllerActions.LeftHand.Menu);
            rightMenu.Initialize(mainControllerActions.RightHand.Menu);

            if(XRDevice.refreshRate >= 60f && XRDevice.refreshRate <= 120f)
                Time.fixedDeltaTime = XRDevice.refreshRate;
        }
    }
}