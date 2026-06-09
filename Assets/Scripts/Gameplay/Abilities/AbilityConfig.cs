using System.Collections.Generic;
using System.Linq;
using Gameplay.Core.Abilities;
using UnityEngine;

namespace Gameplay.Abilities
{
    [CreateAssetMenu(fileName = "New Ability Config", menuName = "Create New Ability Config")]
    public class AbilityConfig : ScriptableObject
    {
        [SerializeReference]
        private List<IAbilityComponentData> _abilities = new List<IAbilityComponentData>();

        public List<IAbilityAction> GetAbilityActions()
        {
            return _abilities.Select(a => a.CreateRuntimeAction()).ToList();
        }
    }
}