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
        private IMovable _movable;

        public DashAbilityAction(float speed, float duration)
        {
            _speed = speed;
            _duration = duration;
        }

        public void Execute(IAbilityContext context)
        {
            _elapsedTime = 0f;
            _movable = context.Owner.GetComponentProvider<IMovable>();
            context.Owner.RestrictionController.Register(EntityRestrictionType.Input, this);
        }

        public bool Tick(IAbilityContext context, float deltaTime)
        {
            if (_movable == null)
            {
                Debug.LogWarning($"AimAbilityAction: movable is null on Entity:{context.Owner.ID}");
                return true;
            }

            _elapsedTime += deltaTime;

            if (_elapsedTime >= _duration)
            {
                context.Owner.RestrictionController.Unregister(EntityRestrictionType.Input, this);
                return true;
            }

            var direction = context.Owner.Forward;
            var translation = direction * (_speed * deltaTime);

            _movable.Move(translation);
            return false;
        }
    }
}