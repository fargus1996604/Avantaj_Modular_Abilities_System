using Gameplay.Core;
using Gameplay.Core.Abilities;
using Gameplay.Core.Components;
using UnityEngine;

namespace Gameplay.Entities
{
    [RequireComponent(typeof(CharacterController), typeof(CharacterAnimationController), typeof(IEntity))]
    public class MovementComponent : MonoBehaviour, IMovable
    {
        private CharacterController _controller;
        protected CharacterController Controller => _controller ??= GetComponent<CharacterController>();

        private CharacterAnimationController _animationController;

        protected CharacterAnimationController AnimationController =>
            _animationController ??= GetComponent<CharacterAnimationController>();

        private IEntity _entity;
        protected IEntity Entity => _entity ??= GetComponent<IEntity>();

        [SerializeField]
        private float _movementSpeed = 5f;

        [SerializeField]
        private float _rotationSpeed = 30f;

        [SerializeField]
        private float _gravity = -9.81f;

        private Vector3 _velocity;
        private Vector2 _moveAxis;

        private void Update()
        {
            if (_moveAxis.sqrMagnitude > 0.001f)
            {
                _velocity = new Vector3(_moveAxis.x, 0f, _moveAxis.y) * _movementSpeed;

                if (!Entity.RestrictionController.HasRestriction(EntityRestrictionType.Rotation))
                {
                    var targetRotation = Quaternion.LookRotation(_velocity);
                    LookAt(Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed));
                }
            }
            else
            {
                _velocity = Vector3.zero;
            }

            if (Controller.isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }

            _velocity.y += _gravity;
            var velocity = new Vector3(Controller.velocity.x, 0, Controller.velocity.z).normalized;
            AnimationController.SetVelocity(velocity.magnitude);
            Move(_velocity * Time.deltaTime);
        }

        private bool HasMovementRestricted()
        {
            return Entity.RestrictionController.HasRestriction(EntityRestrictionType.Movement) ||
                   Entity.RestrictionController.HasRestriction(EntityRestrictionType.Rotation);
        }

        public void SetMoveAxis(Vector2 axis) => _moveAxis = axis;
        public void Move(Vector3 delta) => Controller.Move(delta);
        public void LookAt(Quaternion quaternion) => transform.rotation = quaternion;
    }
}