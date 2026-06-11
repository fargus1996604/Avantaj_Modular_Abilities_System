using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Core.Abilities.Cooldown
{
    public class CooldownService : ICooldownService
    {
        private readonly Dictionary<string, float> _cooldowns = new();

        public bool IsReady(string abilityId)
        {
            return !_cooldowns.TryGetValue(abilityId, out var endTime)
                   || Time.time >= endTime;
        }

        public void Register(string abilityId, float duration)
        {
            _cooldowns[abilityId] = Time.time + duration;
        }
    }
}
