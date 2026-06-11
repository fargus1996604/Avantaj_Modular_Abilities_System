using System.Collections.Generic;
using Gameplay.Core.Targets;
using UnityEngine;

namespace Gameplay.Core.Abilities
{
    [CreateAssetMenu(fileName = "Ability", menuName = "Config/Ability")]
    public class AbilityConfig : ScriptableObject
    {
        [SerializeField]
        private string _abilityId;
        public string AbilityId => _abilityId;

        [SerializeField]
        private float _cooldown;
        public float Cooldown => _cooldown;

        [SerializeField]
        private TargetSelectorBase _targetSelector;
        public TargetSelectorBase TargetSelector => _targetSelector;

        [SerializeReference]
        private List<IAbilityComponentData> _abilities = new List<IAbilityComponentData>();

        public List<IAbilityAction> GetAbilityActions()
        {
            var result = new List<IAbilityAction>(_abilities.Count);

            foreach (var ability in _abilities)
            {
                result.Add(
                    ability.CreateRuntimeAction());
            }

            return result;
        }
    }
}