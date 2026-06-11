using System;
using System.Linq;
using Gameplay.Core;
using Gameplay.Core.Abilities;
using Gameplay.Core.Components;
using UnityEngine;

namespace Gameplay.Abilities.Data
{
    [Serializable]
    public class AimComponentData : IAbilityComponentData
    {
        public enum AimType
        {
            LockToTarget,
            Free
        }

        public AimType TypeAim;
        public float RotationTime;

        public IAbilityAction CreateRuntimeAction()
        {
            if (TypeAim == AimType.Free)
            {
                return null;
            }

            return new AimAbilityAction(RotationTime);
        }
    }

    public class AimAbilityAction : ITickableAction
    {
        private readonly float _rotationTime;
        private float _elapsedTime;
        private Quaternion _startRotation;
        private IMovable _movable;

        public AimAbilityAction(float rotationTime)
        {
            _rotationTime = rotationTime;
        }

        public void Execute(IAbilityContext context)
        {
            _elapsedTime = 0f;
            _startRotation = context.Owner.Rotation;
            _movable = context.Owner.GetComponentProvider<IMovable>();
            context.Owner.RestrictionController.Register(EntityRestrictionType.Rotation, this);
        }

        public bool Tick(IAbilityContext context, float deltaTime)
        {
            if (_movable == null)
            {
                context.Owner.RestrictionController.Unregister(EntityRestrictionType.Rotation, this);
                Debug.LogWarning($"AimAbilityAction: movable is null on Entity:{context.Owner.ID}");
                return true;
            }

            if (context.Targets == null || context.Targets.Count == 0)
            {
                context.Owner.RestrictionController.Unregister(EntityRestrictionType.Rotation, this);
                return true;
            }

            _elapsedTime += deltaTime;
            var target = context.Targets.First();
            var owner = context.Owner;
            Vector3 direction = target.Position - owner.Position;
            direction.y = 0f;
            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                float progress = _rotationTime > 0f ? Mathf.Clamp01(_elapsedTime / _rotationTime) : 1f;
                Quaternion currentRotation = Quaternion.Slerp(_startRotation, targetRotation, progress);
                _movable.LookAt(currentRotation);
            }

            if (_elapsedTime >= _rotationTime)
            {
                context.Owner.RestrictionController.Unregister(EntityRestrictionType.Rotation, this);
                return true;
            }

            return false;
        }
    }
}