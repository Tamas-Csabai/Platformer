using Main.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.StateMachineSystem
{
    public class StandState : PlayerState
    {

        [SerializeField] protected LayerMask groundMask;
        [SerializeField] private GameObject platformPrefab;

        [Header("Default Parameters")]
        [SerializeField] protected float defaultGravity = 9.8f;
        [SerializeField] protected float defaultMoveSpeed = 5f;
        [SerializeField] protected float defaultTurnSpeed = 60f;
        [SerializeField] protected int defaultJumpCount = 2;
        [SerializeField] protected float defaultJumpForce = 2f;

        // Horizontal
        private Vector3 _walkVelocity;
        private float _turnVelocity;
        private System.Action _turnAction;

        // Vertical
        private RaycastHit[] _groundHitsBuffer = new RaycastHit[1];
        private float _verticalVelocity;
        private bool _isGrounded;
        private int _jumpCount;

        // Platform
        private Vector3 _platformVelocity;
        private Transform _currentPlatformTransform;
        private Transform _platformAnchor;
        private Platform _currentPlatform;

        private Transform platformAnchor
        {
            get
            {
                if (_platformAnchor != null)
                    return _platformAnchor;

                return _platformAnchor = Instantiate(platformPrefab).transform;
            }
        }

        [field: Header("Current Values")]
        [field: SerializeField] public float CurrentGravity { get; set; }
        [field: SerializeField] public float CurrentMoveSpeed { get; set; }
        [field: SerializeField] public float CurrentTurnSpeed { get; set; }
        [field: SerializeField] public int CurrentJumpCount { get; set; }
        [field: SerializeField] public float CurrentJumpForce { get; set; }

        protected override void Awake()
        {
            base.Awake();

            CurrentGravity = defaultGravity;
            CurrentMoveSpeed = defaultMoveSpeed;
            CurrentTurnSpeed = defaultTurnSpeed;
            CurrentJumpCount = defaultJumpCount;
            CurrentJumpForce = defaultJumpForce;

            if (InputManager.IsSmoothTurn)
                _turnAction = SmoothRotation;
            else
                _turnAction = SnapRotation;
        }

        public override void UpdateState()
        {
            Walk();

            Fall();

            Jump();

            Turn();

            DrawDebugLines();

            Vector3 velocity = _walkVelocity;
            velocity.y = Time.deltaTime * _verticalVelocity;
            velocity += Time.deltaTime * _platformVelocity;

            _playerController.Move(velocity);

            _playerController.Turn(_turnVelocity);

            _playerController.SetHeight(_playerController.TrackingSpace.localPosition.y);
        }

        private bool GroundCastAll()
        {
            return Physics.SphereCastNonAlloc
                (
                origin: _playerController.BodyCenter.position, 
                radius: _playerController.CurrentRadius, 
                direction: Vector3.down, 
                results: _groundHitsBuffer, 
                maxDistance: _playerController.CurrentHeight / 2f - _playerController.CurrentRadius + _playerController.CurrentSkinWidth, 
                layerMask: groundMask,
                queryTriggerInteraction: QueryTriggerInteraction.Ignore
                ) > 0;
        }

        private void Walk()
        {
            Vector2 _walkInput = InputManager.Walk.Get().normalized;

            float inputRotation = Mathf.Atan2(_walkInput.x, _walkInput.y) * Mathf.Rad2Deg;

            Quaternion orientation = Quaternion.Euler((_playerController.Head.eulerAngles.y + inputRotation) * Vector3.up);

            Vector3 movement = orientation * (Time.deltaTime * CurrentMoveSpeed * _walkInput.magnitude * Vector3.forward);

            _walkVelocity.x = movement.x;
            _walkVelocity.y = 0f;
            _walkVelocity.z = movement.z;
        }

        private void Fall()
        {
            _isGrounded = GroundCastAll();

            if (_isGrounded)
            {
                if (_verticalVelocity < 0f)
                    Land();

                if (!_groundHitsBuffer[0].transform.gameObject.isStatic)
                {
                    if(_currentPlatformTransform != _groundHitsBuffer[0].transform)
                    {
                        _currentPlatformTransform = _groundHitsBuffer[0].transform;

                        platformAnchor.SetParent(_currentPlatformTransform, true);
                        platformAnchor.position = _groundHitsBuffer[0].point;

                        _currentPlatform = _groundHitsBuffer[0].transform.GetComponent<Platform>();

                        if (_currentPlatform == null)
                            _platformVelocity = Vector3.zero;
                    }

                    if (_currentPlatform != null)
                        _platformVelocity = _currentPlatform.Velocity;
                }
                else
                {
                    _currentPlatform = null;
                    _currentPlatformTransform = null;
                    _platformVelocity = Vector3.zero;
                    platformAnchor.SetParent(transform, true);
                    platformAnchor.localPosition = Vector3.zero;
                }
            }
            else
            {
                _verticalVelocity -= CurrentGravity * Time.deltaTime;
            }
        }

        private void Jump()
        {
            if (!InputManager.Jump.GetDown() || _jumpCount >= CurrentJumpCount)
                return;

            _jumpCount++;

            _verticalVelocity = CurrentJumpForce;
        }

        private void Land()
        {
            _verticalVelocity = 0f;
            _jumpCount = 0;
        }

        private void DrawDebugLines()
        {
            Vector3 start = _playerController.BodyCenter.position;
            Vector3 end = start + ((_playerController.CurrentHeight / 2f - _playerController.CurrentRadius + _playerController.CurrentSkinWidth) * Vector3.down);
            Debug.DrawLine(start, end, Color.yellow);
        }

        private void Turn()
        {
            _turnAction?.Invoke();
        }

        private void SnapRotation()
        {
            _turnVelocity = 0.0f;

            if (InputManager.Turn.GetWest())
                _turnVelocity = -Mathf.Abs(InputManager.SnapTurnAngle);

            if (InputManager.Turn.GetEast())
                _turnVelocity = Mathf.Abs(InputManager.SnapTurnAngle);
        }

        private void SmoothRotation()
        {
            _turnVelocity = InputManager.Turn.Get().x * Time.deltaTime * InputManager.SmoothTurnSpeed * CurrentTurnSpeed;
        }

    }
}
