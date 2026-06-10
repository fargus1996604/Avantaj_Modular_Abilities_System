using System.Collections.Generic;

namespace Gameplay.Core
{
    public class DefaultRestrictionController : IEntityRestrictionController
    {
        private Dictionary<EntityRestrictionType, List<object>> _register = new();

        public void Register(EntityRestrictionType restrictionType, object owner)
        {
            if (_register.ContainsKey(restrictionType))
            {
                _register[restrictionType].Add(owner);
            }
            else
            {
                _register.Add(restrictionType, new List<object> { owner });
            }
        }

        public void Unregister(EntityRestrictionType restrictionType, object owner)
        {
            if (_register.ContainsKey(restrictionType))
            {
                _register[restrictionType].Remove(owner);
                if (_register[restrictionType].Count == 0)
                {
                    _register.Remove(restrictionType);
                }
            }
        }

        public bool HasRestriction(EntityRestrictionType restrictionType)
        {
            return _register.ContainsKey(restrictionType);
        }
    }
}