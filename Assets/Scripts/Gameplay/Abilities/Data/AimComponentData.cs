using System;
using System.Linq;
using Gameplay.Core;
using Gameplay.Core.Abilities;
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

        public AimAbilityAction(float rotationTime)
        {
            _rotationTime = rotationTime;
        }

        public void Execute(IAbilityContext context)
        {
            _elapsedTime = 0f;
            context.Owner.RestrictionController.Register(EntityRestrictionType.Rotation, this);
            if (context.Owner is MonoBehaviour ownerMono)
            {
                _startRotation = ownerMono.transform.rotation;
            }
        }

        public bool Tick(IAbilityContext context, float deltaTime)
        {
            _elapsedTime += deltaTime;
            if (context.Targets != null && context.Targets.Count > 0 && context.Owner is IMovable movableOwner)
            {
                var target = context.Targets.First();
                var owner = context.Owner;
                Vector3 direction = target.Position - owner.Position;
                direction.y = 0f;
                if (direction.sqrMagnitude > 0.001f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    float progress = _rotationTime > 0f ? Mathf.Clamp01(_elapsedTime / _rotationTime) : 1f;
                    Quaternion currentRotation = Quaternion.Slerp(_startRotation, targetRotation, progress);
                    movableOwner.LookAt(currentRotation);
                }
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