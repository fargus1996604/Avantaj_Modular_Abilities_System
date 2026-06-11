using System.Collections.Generic;
using System.Linq;
using Gameplay.Core;
using Gameplay.Core.Abilities;
using Gameplay.Core.Abilities.Cooldown;
using Gameplay.Entities;
using UnityEngine;

namespace Gameplay.Abilities
{
    [RequireComponent(typeof(BaseCharacterAdapter), typeof(AbilitiesHandler))]
    public class AbilityCastController : MonoBehaviour
    {
        private BaseCharacterAdapter _characterAdapter;
        protected BaseCharacterAdapter CharacterAdapter => _characterAdapter ??= GetComponent<BaseCharacterAdapter>();

        private AbilitiesHandler _abilitiesHandler;
        protected AbilitiesHandler AbilitiesHandler => _abilitiesHandler ??= GetComponent<AbilitiesHandler>();
        
        private IEntity _entity;
        private readonly ICooldownService _cooldowns = new CooldownService();

        private void Awake()
        {
            _entity = CharacterAdapter;
        }

        public void TryCastSlot(int index)
        {
            var configs = CharacterAdapter.AbilityConfigs;
            if (configs == null || index < 0 || index >= configs.Count) return;

            ExecuteCast(configs.ElementAt(index));
        }

        private void ExecuteCast(AbilityConfig config)
        {
            if (AbilitiesHandler == null)
            {
                return;
            }

            if (!_cooldowns.IsReady(config.AbilityId))
            {
                return;
            }

            _cooldowns.Register(config.AbilityId, config.Cooldown);

            List<IEntity> targetEnemies = new List<IEntity>();
            
            if (config.TargetSelector != null)
            {
                targetEnemies = config.TargetSelector.SelectTargets(_entity);
            }

            var context = new DefaultContext(_entity, targetEnemies);
            var activeAbility = new ActiveAbility(context, config.GetAbilityActions());
            AbilitiesHandler.RegisterAbility(activeAbility);
        }
    }
}