using System;
using Gameplay.Core;
using Gameplay.Core.Abilities;

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
            if (context.Owner is IAnimatable animationController)
            {
                animationController.PlayAnimation(_animationName);
            }
        }
    }
}