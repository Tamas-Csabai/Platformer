
using Main.StateMachineSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.PlayerSystem
{
    public class PlayerController : MonoBehaviour
    {

        [Header("References")]
        [SerializeField] private CharacterController characterController;
        [SerializeField] private CapsuleCollider bodyCapsuleCollider;
        [SerializeField] private PlayerStateMachine playerStateMachine; 
        [SerializeField] private Transform originTransform;
        [SerializeField] private Transform headTransform;
        [SerializeField] private Transform bodyCenterTransform;
        [SerializeField] private Transform trackingSpaceTransform;

        [Header("Player States")]
        [SerializeField] private StandState standState;

        [Header("Parameters")]
        [SerializeField] private float defaultHeight = 1.7f;
        [SerializeField] private float minHeight = 1f;
        [SerializeField] private float defaultRadius = 0.1f;
        [SerializeField] private float defaultSkinWidth = 0.08f;

        public Transform Origin => originTransform;
        public Transform Head => headTransform;
        public Transform BodyCenter => bodyCenterTransform;
        public Transform TrackingSpace => trackingSpaceTransform;
        public float CurrentHeight { get; private set; }
        public float CurrentRadius { get; private set; }
        public float CurrentSkinWidth { get; private set; }

        private void Awake()
        {
            CurrentHeight = defaultHeight;
            CurrentRadius = defaultRadius;
            CurrentSkinWidth = defaultSkinWidth;

            characterController.height = CurrentHeight;
            characterController.radius = CurrentRadius;
            characterController.skinWidth = CurrentSkinWidth - 0.01f;

            bodyCapsuleCollider.height = CurrentHeight;

            playerStateMachine.ChangeState(standState);
        }

        private void Update()
        {
            playerStateMachine.UpdateState();

            bodyCenterTransform.position = trackingSpaceTransform.position + (CurrentHeight / 2f * Vector3.down);
            characterController.center = bodyCenterTransform.localPosition;
        }

        public void Move(Vector3 velocity)
        {
            characterController.Move(velocity);
        }

        public void Turn(float angle)
        {
            originTransform.RotateAround(Head.position, Vector3.up, angle);
        }

        public void SetHeight(float height)
        {
            CurrentHeight = Mathf.Clamp(height, minHeight, 2.5f);
            characterController.height = CurrentHeight;
            bodyCapsuleCollider.height = CurrentHeight;
        }

    }
}
