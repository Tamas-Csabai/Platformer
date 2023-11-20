using UnityEngine;
using NaughtyAttributes;
using System;
using Main.Input;

namespace Main
{
    public class PlayerController_Old : MonoBehaviour
    {

        public enum State
        {
            Stand,
            Crouch,
            Climb
        }

        [Header("References")]
        [SerializeField] public CharacterController characterController;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private Transform trackingSpace;

        [Header("Anchors")]
        [SerializeField] private Transform headAnchorStand;
        [SerializeField] private Transform headAnchorCrouch;
        [SerializeField] private Transform centerAnchorStand;
        [SerializeField] private Transform centerAnchorCrouch;

        [Header("Physics")]
        [SerializeField] private LayerMask groundMask;
        public float gravity = 9.8f;
        public float speed = 3f;
        public float crouchSpeed = 2f;
        public float jumpHeight = 3f;
        public float weight = 1f;
        public float turnSpeed = 5f;
        [Range(0f, 1f)] public float bounceForceDecay = 0.5f;
        [SerializeField, ReadOnly] private bool standing = true;

        [Header("State")]
        [SerializeField] float standHeight = 1.8f;
        [SerializeField] float crouchHeight = 0.4f;
        [SerializeField] float climbingHeight = 0.4f;
        [SerializeField] float standUpSpeed = 2f;
        [SerializeField] float resetHeightUpThreshold = 0.2f;
        [SerializeField] float resetHeightDownThreshold = 0.4f;

        [Header("Hacks")]
        [SerializeField] bool fly = false;
        [SerializeField] float flySpeed = 5f;

        private Vector3 movement;
        private Vector3 velocityBuffer = Vector3.zero;
        private IPlayerMover currentMover;
        private float vSpeed = 0.0f;
        private bool stop = false;
        private bool isFlying = false;
        private Transform characterControllerCenter;
        private RaycastHit[] jumpGroundHits = new RaycastHit[1];
        private bool isJumping = false;
        private Vector3 newCharacterControllerCenter;

        private bool onPlatform = false;
        private RaycastHit[] platformHits = new RaycastHit[1];
        private Vector3 platformPrevPosition;
        private Transform groundTransform;

        private float initialGravity;
        private float initialHeight;
        private float initialSlopeLimit;
        private float initialStepOffset;
        public Transform Head => cameraTransform;
        public Transform Body => characterControllerCenter;

        public State CurrentState { get; private set; } = State.Stand;
        public float DefaultSpeed { get; set; }
        public float DefaultCrouchSpeed { get; set; }
        public float DefaultJumpHeight { get; set; }

        #region Stand up Tween Fields
        private Vector3 startPosition;
        private Vector3 groundPosition;
        private float startStandingTime;
        private float standingLength;
        private bool standingUp = false;
        #endregion

        #region Input Actions
        private bool onFlyDown = false;
        private bool onCrouchDown = false;
        private bool onResetHeightDown = false;
        private bool onTurnWestDown = false;
        private bool onTurnEastDown = false;
        #endregion

        private void Awake()
        {
            initialGravity = gravity;
            initialHeight = characterController.height;
            initialSlopeLimit = characterController.slopeLimit;
            initialStepOffset = characterController.stepOffset;

            DefaultSpeed = speed;
            DefaultJumpHeight = jumpHeight;

            characterControllerCenter = new GameObject("BODY CENTER").transform;
            characterControllerCenter.transform.SetParent(transform);
            characterControllerCenter.transform.localPosition = Vector3.zero;

            InputManager.HMD.OnPutHead += ResetHeight;
            InputManager.Fly.OnDown += OnFlyDown;
            InputManager.Crouch.OnDown += OnCrouchDown;
            InputManager.ResetHeight.OnDown += OnResetHeight;
            InputManager.Turn.OnWest += OnTurnWest;
            InputManager.Turn.OnEast += OnTurnEast;
        }

        private void OnFlyDown() => onFlyDown = true;

        private void OnCrouchDown() => onCrouchDown = true;

        private void OnResetHeight() => onResetHeightDown = true;

        private void OnTurnWest() => onTurnWestDown = true;

        private void OnTurnEast() => onTurnEastDown = true;

        private void ResetInputActions()
        {
            onFlyDown = false;
            onCrouchDown = false;
            onResetHeightDown = false;
            onTurnWestDown = false;
            onTurnEastDown = false;
        }

        private void OnDestroy()
        {
            InputManager.HMD.OnPutHead -= ResetHeight;
            InputManager.Fly.OnDown -= OnFlyDown;
            InputManager.Crouch.OnDown -= OnCrouchDown;
            InputManager.ResetHeight.OnDown -= OnResetHeight;
            InputManager.Turn.OnWest -= OnTurnWest;
            InputManager.Turn.OnEast -= OnTurnEast;
        }

        private void Update()
        {

            if (Debug.isDebugBuild)
            {
                if (onFlyDown)
                    fly = !fly;

                if (fly)
                {
                    if (!isFlying)
                    {
                        isFlying = true;
                        gravity = 0f;
                        vSpeed = 0f;
                    }
                    Fly();
                }
                else if (isFlying)
                {
                    isFlying = false;
                    gravity = initialGravity;
                }
            }

            if (stop && InputManager.CanMove && characterController.isGrounded)
            {
                stop = false;
            }
            if (!stop && !InputManager.CanMove)
            {
                stop = true;
            }

            newCharacterControllerCenter.Set(cameraTransform.localPosition.x, characterController.center.y, cameraTransform.localPosition.z);
            characterController.center = newCharacterControllerCenter;
            characterControllerCenter.transform.localPosition = characterController.center;

            if (onCrouchDown)
                SwitchState();

            if (onResetHeightDown)
                ResetHeight();

            ClampHeight();

            if (standing)
            {
                //SetHeight(head.localPosition.y - characterController.skinWidth);
            }

            if (!stop)
            {
                CalculateMovement();
                if (!InputManager.CanTurningWhileClimbing)
                {
                    if (InputManager.IsSmoothTurn)
                        SmoothRotation();
                    else
                        SnapRotation();
                }
            }
            else if (stop && InputManager.CanMove)
            {
                FreeFall();
                if (InputManager.CanTurningWhileClimbing)
                {
                    if (InputManager.IsSmoothTurn)
                        SmoothRotation();
                    else
                        SnapRotation();
                }
            }

            if (InputManager.CanTurningWhileClimbing)
            {
                if (InputManager.IsSmoothTurn)
                    SmoothRotation();
                else
                    SnapRotation();
            }

            if (standingUp)
            {
                float distCovered = (Time.time - startStandingTime) * standUpSpeed;
                float fractionOfDistance = distCovered / standingLength;
                transform.position = Vector3.Lerp(startPosition, groundPosition, fractionOfDistance);
                if (transform.position == groundPosition)
                {
                    characterController.center = new Vector3(cameraTransform.localPosition.x, cameraTransform.localPosition.y - (initialHeight / 2) + characterController.skinWidth, cameraTransform.localPosition.z);
                    SetHeight(cameraTransform.localPosition.y - characterController.skinWidth);
                    standingUp = false;
                    standing = true;
                }
            }

            ResetInputActions();
        }

        public void NullifyVelocity()
        {
            velocityBuffer = Vector3.zero;
            vSpeed = 0;
        }


#if UNITY_EDITOR
        [ContextMenu("Toggle Fly")]
        public void ToggleFly()
        {
            fly = !fly;
        }
#endif


        public void Fly()
        {
            if (InputManager.Jump.Get())
                characterController.Move(Vector3.up * flySpeed * Time.deltaTime);

            if (InputManager.Crouch.Get())
                characterController.Move(-Vector3.up * flySpeed * Time.deltaTime);
        }

        private void FreeFall()
        {
            characterController.stepOffset = 0f;

            velocityBuffer.y -= gravity * Time.deltaTime;

            //Quaternion orientation = CalculateOrientation();
            Vector3 movement = Vector3.zero;
            //float input = InputManager.Walk.Get().magnitude;
            //bool turbo = InputManager.ResetHeight.GetDown();
            movement += CalculateOrientation() * (InputManager.Walk.Get().magnitude * speed * Vector3.forward);

            //fall
            characterController.Move((velocityBuffer + movement) * Time.deltaTime);

            if (!characterController.isGrounded && !Physics.Raycast(cameraTransform.position, -Vector3.up, characterController.height + characterController.skinWidth + 0.2f, groundMask))
            {
                //bounce physics
                Vector3 bottomPoint = characterControllerCenter.transform.position + Vector3.up * -characterController.height * 0.5f;
                Vector3 topPoint = bottomPoint + Vector3.up * characterController.height;
                float velocityBufferMagnitude = velocityBuffer.magnitude;

                RaycastHit[] hits = Physics.CapsuleCastAll(bottomPoint, topPoint, characterController.radius, velocityBuffer.normalized, characterController.skinWidth + characterController.minMoveDistance, groundMask);

                Vector3 bounceSumDirection = Vector3.zero;
                if (hits != null && hits.Length > 0)
                {
                    for (int i = 0; i < hits.Length; i++)
                    {
                        bounceSumDirection += Vector3.Reflect(velocityBuffer, hits[i].normal);
                    }
                    velocityBuffer = bounceSumDirection.normalized * velocityBufferMagnitude * bounceForceDecay;
                }
            }
            else
            {
                //ending freefall upon landing
                velocityBuffer.y = 0f;
                stop = false;
            }

            characterController.stepOffset = initialStepOffset;
        }

        public void Jump(float height)
        {
            if (!isFlying && !isJumping && (characterController.isGrounded || Physics.SphereCastNonAlloc(cameraTransform.position, characterController.radius, -Vector3.up, jumpGroundHits, characterController.height + 0.5f, groundMask, QueryTriggerInteraction.Ignore) > 0))
            {
                isJumping = true;
                vSpeed = height;
            }
        }

        private void CalculateMovement()
        {
            //Quaternion orientation = CalculateOrientation();

            //Vector2 walkInput = InputManager.Walk.Get();

            //float walk = walkInput.magnitude * speed;

            //bool turbo = InputManager.ResetHeight.Get();

            //float inputSpeed = (walk * (turbo ? turboSpeed : speed));
            //float maxSpeed = turboSpeed * 1.5f;

            movement = CalculateOrientation() * (InputManager.Walk.Get().magnitude * (CurrentState == State.Stand ? speed : crouchSpeed) * Vector3.forward);

            if (characterController.isGrounded)
            {
                vSpeed = 0f;
                isJumping = false;
            }

            bool jump = InputManager.Jump.Get();

            if (jump)
            {
                if (CurrentState == State.Crouch)
                    Stand();
                else
                    Jump(jumpHeight);
            }


            if (Physics.RaycastNonAlloc(cameraTransform.position, -Vector3.up, platformHits, characterController.height + characterController.skinWidth + 0.1f, groundMask, QueryTriggerInteraction.Ignore) > 0)
            {
                if (!platformHits[0].transform.gameObject.isStatic)
                {
                    if (!onPlatform || groundTransform != platformHits[0].transform)
                    {
                        onPlatform = true;
                        groundTransform = platformHits[0].transform;
                        platformPrevPosition = platformHits[0].transform.position;
                    }
                    else
                    {
                        transform.position += groundTransform.position - platformPrevPosition;
                        platformPrevPosition = groundTransform.position;
                    }
                }
            }
            else
            {
                onPlatform = false;
                groundTransform = null;
            }

            if (vSpeed > 0f && Physics.Raycast(transform.position + characterController.center, Vector3.up, (characterController.height / 2f) + 0.2f, groundMask, QueryTriggerInteraction.Ignore))
                vSpeed = 0f;

            vSpeed -= gravity * Time.deltaTime;

            movement.y = vSpeed;

            characterController.Move(movement * Time.deltaTime);
        }

        private Quaternion CalculateOrientation()
        {
            float rotation = Mathf.Atan2(InputManager.Walk.Get().x, InputManager.Walk.Get().y);
            rotation *= Mathf.Rad2Deg;

            Vector3 orientationEuler = new Vector3(0, cameraTransform.eulerAngles.y + rotation, 0);
            return Quaternion.Euler(orientationEuler);
        }

        private void SnapRotation()
        {
            float snapValue = 0.0f;

            if (onTurnWestDown)
                snapValue = -Mathf.Abs(InputManager.SnapTurnAngle);

            if (onTurnEastDown)
                snapValue = Mathf.Abs(InputManager.SnapTurnAngle);

            transform.RotateAround(cameraTransform.position, Vector3.up, snapValue);
        }

        private void SmoothRotation()
        {
            transform.RotateAround(cameraTransform.position, Vector3.up, InputManager.Turn.Get().x * Time.deltaTime * InputManager.SmoothTurnSpeed * turnSpeed);
        }

        public void RotateAround(float angle)
        {
            transform.RotateAround(cameraTransform.position, Vector3.up, angle);
        }

        public void Move(Vector3 vector)
        {
            if (InputManager.CanMove && !isFlying)
            {
                characterController.Move(vector);
            }
        }

        public void ForceMove(Vector3 vector)
        {
            characterController.Move(vector);
        }

        public void AddMover(IPlayerMover mover)
        {
            if (currentMover == null)
            {
                SuspendWalking();
                currentMover = mover;
            }
            else
            {
                IPlayerMover prevMover = currentMover;
                currentMover = mover;
                prevMover.Deactivate();
            }

        }

        public void RemoveMover(IPlayerMover mover)
        {
            if (currentMover == mover)
            {
                currentMover = null;
                UnsuspendWalking();
            }
        }

        public void ClearMover()
        {
            RemoveMover(currentMover);
        }

        public void MovePlayer(IPlayerMover mover, Vector3 velocity)
        {
            if (currentMover == mover)
                characterController.Move(velocity);
        }

        public void SuspendWalking()
        {
            InputManager.CanMove = false;
            if (standingUp)
                standingUp = false;

            characterController.slopeLimit = 0f;
            characterController.stepOffset = 0f;
            characterController.height = climbingHeight;
            characterController.center = cameraTransform.localPosition;
            standing = false;
        }

        public void UnsuspendWalking()
        {
            velocityBuffer = characterController.velocity / weight;
            InputManager.CanMove = true;
            characterController.slopeLimit = initialSlopeLimit;
            characterController.stepOffset = initialStepOffset;
            if (!standingUp)
                StandUp();
        }

        private void SetHeight(float height)
        {

            float headHeight = Mathf.Clamp(height, climbingHeight, 2.5f);
            characterController.height = headHeight;

            Vector3 newCenter = Vector3.zero;
            newCenter.y = characterController.height / 2;
            newCenter.y += characterController.skinWidth;

            newCenter.x = cameraTransform.localPosition.x;
            newCenter.z = cameraTransform.localPosition.z;

            characterController.center = newCenter;
        }

        public void ResetHeight()
        {
            switch (CurrentState)
            {
                case State.Stand:
                    Stand();
                    break;
                case State.Crouch:
                    Crouch();
                    break;
            }
        }

        public void ClampHeight()
        {
            if (CurrentState == State.Stand)
            {
                if (Head.position.y - headAnchorStand.position.y > resetHeightUpThreshold || Head.position.y - headAnchorStand.position.y < -resetHeightDownThreshold)
                    Stand();
            }
            else if (CurrentState == State.Crouch)
            {
                if (Head.position.y - headAnchorCrouch.position.y > resetHeightUpThreshold || Head.position.y - headAnchorCrouch.position.y < -resetHeightDownThreshold)
                    Crouch();
            }
        }

        private void SwitchState()
        {
            if (isFlying)
                return;

            switch (CurrentState)
            {
                case State.Stand:
                    Crouch();
                    break;
                case State.Crouch:
                    Stand();
                    break;
            }
        }

        [Button]
        private void Stand()
        {
            if (Physics.Raycast(transform.position + characterController.center, Vector3.up, standHeight - (crouchHeight / 2) + 0.1f, groundMask, QueryTriggerInteraction.Ignore))
                return;

            CurrentState = State.Stand;
            trackingSpace.transform.position += Vector3.up * (headAnchorStand.transform.position.y - cameraTransform.transform.position.y);
            characterController.center = centerAnchorStand.localPosition;
            characterController.height = standHeight;
        }

        [Button]
        private void Crouch()
        {
            CurrentState = State.Crouch;
            trackingSpace.transform.position += Vector3.up * (headAnchorCrouch.transform.position.y - cameraTransform.transform.position.y);
            characterController.height = crouchHeight;
            characterController.center = centerAnchorCrouch.localPosition;
        }

        private void StandUp()
        {
            RaycastHit hit;
            if (Physics.Raycast(cameraTransform.position, -Vector3.up, out hit, initialHeight + characterController.skinWidth, groundMask))
            {
                startPosition = transform.position;
                groundPosition = new Vector3(transform.position.x, hit.point.y, transform.position.z);
                startStandingTime = Time.time;
                standingLength = Vector3.Magnitude(startPosition - groundPosition);
                standingUp = true;
            }
            else
            {
                standing = true;
            }
        }

        public void MoveTransform(Vector3 position)
        {
            transform.position = position + (transform.position - (transform.position + characterController.center));
        }

    }
}


