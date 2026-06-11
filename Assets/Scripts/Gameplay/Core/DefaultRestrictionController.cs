using System.Collections.Generic;

namespace Gameplay.Core
{
    public class DefaultRestrictionController : IEntityRestrictionController
    {
        private readonly Dictionary<EntityRestrictionType, HashSet<object>> _register = new();

        public void Register(EntityRestrictionType restrictionType, object owner)
        {
            if (!_register.TryGetValue(restrictionType, out var owners))
            {
                owners = new HashSet<object>();
                _register.Add(restrictionType, owners);
            }

            owners.Add(owner);
        }

        public void Unregister(EntityRestrictionType restrictionType, object owner)
        {
            if (!_register.TryGetValue(restrictionType, out var owners))
            {
                return;
            }

            owners.Remove(owner);

            if (owners.Count == 0)
            {
                _register.Remove(restrictionType);
            }
        }

        public bool HasRestriction(EntityRestrictionType restrictionType)
        {
            return _register.ContainsKey(restrictionType);
        }
    }
}