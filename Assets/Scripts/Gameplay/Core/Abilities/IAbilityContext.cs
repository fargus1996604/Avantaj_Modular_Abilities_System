using System.Collections.Generic;

namespace Gameplay.Core.Abilities
{
    public interface IAbilityContext
    {
        IEntity Owner { get; }
        IReadOnlyCollection<IEntity> Targets { get; }
    }
}
