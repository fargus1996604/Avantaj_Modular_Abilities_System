using System;
using Gameplay.Core;
using Gameplay.Core.Abilities;
using Gameplay.Core.Components;
using UnityEngine;

namespace Gameplay.Abilities.Data
{
    [Serializable]
    public class AnimationComponentData : IAbilityComponentData
    {
        public string AnimationName;

        public IAbilityAction CreateRuntimeAction()
        {
            return new AnimationAbilityAction(AnimationName);
        }
    }

    public class AnimationAbilityAction : IAbilityAction
    {
        private string _animationName;

        public AnimationAbilityAction(string animationName)
        {
            _animationName = animationName;
        }

        public void Execute(IAbilityContext context)
        {
            var animationController = context.Owner.GetComponentProvider<IAnimationController>();
            if (animationController == null)
            {
                Debug.LogWarning($"AnimationAbility: animationController is null on Entity:{context.Owner.ID}");
                return;
            }

            animationController.SetTrigger(_animationName);
        }
    }
}