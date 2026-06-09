using System;
using Gameplay.Core;
using Gameplay.Core.Abilities;
using UnityEngine;

namespace Gameplay.Abilities.Data
{
    [Serializable]
    public class DashComponentData : IAbilityComponentData
    {
        public float Speed;
        public float Duration;

        public IAbilityAction CreateRuntimeAction()
        {
            return new DashAbilityAction(Speed, Duration);
        }
    }

    public class DashAbilityAction : ITickableAction
    {
        private readonly float _speed;
        private readonly float _duration;
        private float _elapsedTime;

        public DashAbilityAction(float speed, float duration)
        {
            _speed = speed;
            _duration = duration;
        }

        public void Execute(IAbilityContext context)
        {
            context.Owner.RestrictionController.Register(EntityRestrictionType.Input, this);
            _elapsedTime = 0f;
        }

        public bool Tick(IAbilityContext context, float deltaTime)
        {
            _elapsedTime += deltaTime;

            if (_elapsedTime >= _duration)
            {
                context.Owner.RestrictionController.Unregister(EntityRestrictionType.Input, this);
                return true;
            }

            Vector3 direction = context.Owner.Forward;
            Vector3 translation = direction * (_speed * deltaTime);

            if (context.Owner is IMovable movable)
            {
                movable.Move(translation);
            }

            return false;
        }
    }
}