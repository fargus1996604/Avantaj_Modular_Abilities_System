using System.Collections.Generic;
using Gameplay.Core.Abilities;

namespace Gameplay.Core.Targets
{
    public interface ITargetSelector
    {
        List<IEntity> SelectTargets(IEntity caster);
    }
}
