using Gameplay.Core.Abilities;
using UnityEngine;

namespace Gameplay.Abilities.Data
{
    public abstract class DelayedActionBase : ITickableAction
    {
        private readonly PlayTimeData _playTime;
        private float _elapsedDelay;
        private bool _isStarted;

        protected DelayedActionBase(PlayTimeData playTime)
        {
            _playTime = playTime;
        }

        public virtual void Execute(IAbilityContext context)
        {
            _elapsedDelay = 0f;
            _isStarted = false;

            if (_playTime == null || _playTime.Type == PlayTimeType.Start || _playTime.Duration <= 0f)
            {
                TriggerStart(context);
            }
        }

        public virtual bool Tick(IAbilityContext context, float deltaTime)
        {
            if (_isStarted) return true;

            _elapsedDelay += deltaTime;
            if (_elapsedDelay >= _playTime.Duration)
            {
                TriggerStart(context);
                return true; 
            }

            return false;
        }

        private void TriggerStart(IAbilityContext context)
        {
            _isStarted = true;
            OnStart(context);
        }

        protected abstract void OnStart(IAbilityContext context);
    }
}
