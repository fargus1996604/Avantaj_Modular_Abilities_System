using System.Collections.Generic;
using Gameplay.Abilities;
using Gameplay.Core;
using Gameplay.Core.Abilities;
using UnityEngine;

namespace Gameplay.Entities
{
    [RequireComponent(typeof(CharacterController))]
    public class BaseCharacterAdapter : MonoBehaviour, IEntity, IAnimatable, IDamageable, IHealable, IMovable
    {
        private CharacterController _controller;
        protected CharacterController Controller => _controller ??= GetComponent<CharacterController>();

        private IEntityRestrictionController _restrictionController;
        public IEntityRestrictionController RestrictionController =>
            _restrictionController ??= new DefaultRestrictionController();

        public Vector3 Forward => transform.forward;
        public Vector3 Position => transform.position;

        [SerializeField]
        private CharacterAnimationController _animationController;

        [SerializeField]
        private List<AbilityConfig> _abilityConfigs;
        public IReadOnlyCollection<AbilityConfig> AbilityConfigs => _abilityConfigs;
        
        [SerializeField]
        private float _gravity = -9.81f;

        [SerializeField]
        private float _movementSpeed = 5f;

        [SerializeField]
        private float _rotationSpeed = 30f;

        private Vector3 _velocity;
        private Vector2 _moveAxis;

        private void Update()
        {
            bool canMove = !RestrictionController.HasRestriction(EntityRestrictionType.Movement)
                           && !RestrictionController.HasRestriction(EntityRestrictionType.Input);

            if (canMove && _moveAxis.sqrMagnitude > 0.01f)
            {
                _velocity = new Vector3(_moveAxis.x, 0f, _moveAxis.y) * _movementSpeed;
                if (!RestrictionController.HasRestriction(EntityRestrictionType.Rotation))
                {
                    var rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_velocity),
                        Time.deltaTime * _rotationSpeed);
                    LookAt(rotation);
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

            _velocity.y += _gravity * Time.deltaTime;
            
            var velocity = new Vector3(Controller.velocity.x, 0, Controller.velocity.z).normalized;
            _animationController.SetVelocity(velocity.magnitude);
            Move(_velocity * Time.deltaTime);
        }

        public void Move(Vector3 delta)
        {
            Controller.Move(delta);
        }

        public void LookAt(Quaternion quaternion)
        {
            transform.rotation = quaternion;
        }

        public void SetMoveAxis(Vector2 axis) => _moveAxis = axis;
        public void PlayAnimation(string animationName) => _animationController.SetTrigger(animationName);
        public void TakeDamage(int damage) => Debug.Log(damage);
        public void Heal(int amount) => Debug.Log(amount);
    }
}