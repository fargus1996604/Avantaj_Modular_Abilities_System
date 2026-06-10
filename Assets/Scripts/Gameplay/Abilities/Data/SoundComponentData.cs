using System;
using Gameplay.Core;
using Gameplay.Core.Abilities;
using UnityEngine;

namespace Gameplay.Abilities.Data
{
    [Serializable]
    public class SoundComponentData : IAbilityComponentData
    {
        public AudioClip Clip;
        public TargetType TargetType;
        public PlayTimeData PlayTime;

        public IAbilityAction CreateRuntimeAction()
        {
            return new SoundAbilityAction(Clip, TargetType, PlayTime);
        }
    }

    public class SoundAbilityAction : DelayedActionBase
    {
        private AudioClip _clip;
        private TargetType _targetType;

        public SoundAbilityAction(AudioClip clip, TargetType targetType, PlayTimeData playTime) : base(playTime)
        {
            _clip = clip;
            _targetType = targetType;
        }

        protected override void OnStart(IAbilityContext context)
        {
            if (_targetType == TargetType.Owner)
            {
                PlayOneShot(context.Owner);
            }
            else
            {
                foreach (var contextTarget in context.Targets)
                {
                    PlayOneShot(contextTarget);
                }
            }
        }

        private void PlayOneShot(IEntity entity)
        {
            var soundController = entity.GetComponentProvider<ISoundController>();
            if (soundController == null)
            {
                Debug.LogWarning($"SoundAbilityAction: SoundController is null on Entity:{entity.ID}");
                return;
            }

            soundController.PlayOneShot(_clip);
        }
    }
}