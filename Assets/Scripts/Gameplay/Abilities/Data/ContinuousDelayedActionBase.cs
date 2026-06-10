using Gameplay.Core.Abilities;
using UnityEngine;

namespace Gameplay.Abilities.Data
{
    public abstract class ContinuousDelayedActionBase : ITickableAction
    {
        private readonly PlayTimeData _playTime;
        private readonly DurationType _durationType;
        private readonly float _duration;
        
        private float _elapsedDelay;
        private float _elapsedDuration;
        private bool _isExecutionStarted;
        private bool _isFinished;

        protected ContinuousDelayedActionBase(PlayTimeData playTime, DurationType durationType, float duration)
        {
            _playTime = playTime;
            _durationType = durationType;
            _duration = duration;
        }

        public virtual void Execute(IAbilityContext context)
        {
            _elapsedDelay = 0f;
            _elapsedDuration = 0f;
            _isExecutionStarted = false;
            _isFinished = false;

            if (_playTime == null || _playTime.Type == PlayTimeType.Start || _playTime.Duration <= 0f)
            {
                TriggerStart(context);
            }
        }

        public virtual bool Tick(IAbilityContext context, float deltaTime)
        {
            if (_isFinished) return true;

            // 1. Process Delay Phase
            if (!_isExecutionStarted)
            {
                _elapsedDelay += deltaTime;
                if (_elapsedDelay >= _playTime.Duration)
                {
                    TriggerStart(context);
                    // If it is an Instant action, finalize immediately in the same frame
                    if (_durationType == DurationType.Instant)
                    {
                        TriggerEnd(context);
                        return true;
                    }
                }
                return false; // Yield execution until the next frame update
            }

            // 2. Process Active Continuous Duration Phase
            if (_durationType == DurationType.Instant)
            {
                TriggerEnd(context);
                return true;
            }

            OnTick(context, deltaTime);

            _elapsedDuration += deltaTime;
            if (_elapsedDuration >= _duration)
            {
                TriggerEnd(context);
                return true;
            }

            return false;
        }

        private void TriggerStart(IAbilityContext context)
        {
            _isExecutionStarted = true;
            OnContinuousStart(context);
        }

        private void TriggerEnd(IAbilityContext context)
        {
            _isFinished = true;
            OnContinuousEnd(context);
        }

        protected abstract void OnContinuousStart(IAbilityContext context);
        protected virtual void OnTick(IAbilityContext context, float deltaTime) { }
        protected abstract void OnContinuousEnd(IAbilityContext context);
    }
}
