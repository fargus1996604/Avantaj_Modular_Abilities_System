using System;
using Gameplay.Core;
using Gameplay.Core.Abilities;

namespace Gameplay.Abilities.Data
{
    [Serializable]
    public class HealingAbilityData : IAbilityComponentData
    {
        public int Amount;

        public IAbilityAction CreateRuntimeAction()
        {
            return new HealingAbilityAction(Amount);
        }
    }

    public class HealingAbilityAction : IAbilityAction
    {
        private int _amount;

        public HealingAbilityAction(int amount)
        {
            _amount = amount;
        }

        public void Execute(IAbilityContext context)
        {
            if (context.Owner is IHealable healable)
            {
                healable.Heal(_amount);
            }
        }
    }
}
