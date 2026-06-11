using System;
using Gameplay.Core;
using Gameplay.Core.Abilities;
using Gameplay.Core.Components;
using UnityEngine;

namespace Gameplay.Abilities.Data
{
    [Serializable]
    public class DamageComponentData : IAbilityComponentData
    {
        public int Damage;

        public IAbilityAction CreateRuntimeAction()
        {
            return new DamageAbilityAction(Damage);
        }
    }

    public class DamageAbilityAction : IAbilityAction
    {
        private int _damage;

        public DamageAbilityAction(int damage)
        {
            _damage = damage;
        }

        public void Execute(IAbilityContext context)
        {
            foreach (var target in context.Targets)
            {
                var damageable = target.GetComponentProvider<IDamageable>();
                if (damageable == null)
                {
                    Debug.LogWarning($"DamageAbility: damageable is null on Entity:{target.ID}");
                    continue;
                }

                damageable.TakeDamage(_damage);
            }
        }
    }
}