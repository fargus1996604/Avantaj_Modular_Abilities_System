using System;
using Gameplay.Core;
using Gameplay.Core.Abilities;

namespace Gameplay.Abilities.Data
{
    [Serializable]
    public class LockInputComponentData : IAbilityComponentData
    {
        public float Duration;

        public IAbilityAction CreateRuntimeAction()
        {
            return new LockInputAbilityAction(Duration);
        }
    }

    public class LockInputAbilityAction : ITickableAction
    {
        private readonly float _duration;
        private float _elapsedTime;

        public LockInputAbilityAction(float duration)
        {
            _duration = duration;
        }

        public void Execute(IAbilityContext context)
        {
            _elapsedTime = 0f;

            context.Owner.RestrictionController.Register(EntityRestrictionType.Movement, this);
            context.Owner.RestrictionController.Register(EntityRestrictionType.Rotation, this);
        }

        public bool Tick(IAbilityContext context, float deltaTime)
        {
            _elapsedTime += deltaTime;

            if (_elapsedTime >= _duration)
            {
                context.Owner.RestrictionController.Unregister(EntityRestrictionType.Movement, this);
                context.Owner.RestrictionController.Unregister(EntityRestrictionType.Rotation, this);
                return true;
            }

            return false;
        }
    }
}