using System.Collections.Generic;
using Gameplay.Core.Abilities;
using UnityEngine;

namespace Gameplay.Abilities
{
    public class AbilitiesHandler : MonoBehaviour
    {
        private List<ActiveAbility> _activeAbilities = new();

        private void Update()
        {
            for (int i = _activeAbilities.Count - 1; i >= 0; i--)
            {
                _activeAbilities[i].Update(Time.deltaTime);
                if (_activeAbilities[i].IsFinished)
                {
                    _activeAbilities.RemoveAt(i);
                }
            }
        }

        public void RegisterAbility(ActiveAbility ability)
        {
            _activeAbilities.Add(ability);
        }
    }
}