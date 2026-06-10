using System;
using Gameplay.Core;
using Gameplay.Core.Abilities;
using UnityEngine;

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
            var healer = context.Owner.GetComponentProvider<IHealable>();
            if (healer == null)
            {
                Debug.LogWarning($"HealingAbility: healer is null on Entity:{context.Owner.ID}");
                return;
            }
            healer.Heal(_amount);
        }
    }
}