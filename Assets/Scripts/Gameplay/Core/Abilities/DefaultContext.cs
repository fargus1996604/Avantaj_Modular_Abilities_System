using System.Collections.Generic;

namespace Gameplay.Core.Abilities
{
    public struct DefaultContext : IAbilityContext
    {
        private IEntity _owner;
        public IEntity Owner => _owner;

        private List<IEntity> _targets;
        public IReadOnlyCollection<IEntity> Targets => _targets;

        public DefaultContext(IEntity owner, List<IEntity> targets)
        {
            _owner = owner;
            _targets = targets;
        }
    }
}