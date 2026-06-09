using System;
using Gameplay.Core;
using Gameplay.Core.Abilities;

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
                if (target is IDamageable damageable)
                {
                    damageable.TakeDamage(_damage);
                }
            }
        }
    }
}