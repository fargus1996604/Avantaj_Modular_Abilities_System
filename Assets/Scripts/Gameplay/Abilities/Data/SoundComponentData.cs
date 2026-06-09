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

        public IAbilityAction CreateRuntimeAction()
        {
            return new SoundAbilityAction(Clip);
        }
    }

    public class SoundAbilityAction : IAbilityAction
    {
        private AudioClip _clip;

        public SoundAbilityAction(AudioClip clip)
        {
            _clip = clip;
        }

        public void Execute(IAbilityContext context)
        {
            if (context.Owner is ISoundController soundController)
            {
                soundController.PlayOneShot(_clip);
            }
        }
    }
}