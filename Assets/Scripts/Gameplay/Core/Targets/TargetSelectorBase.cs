using System;
using System.Collections.Generic;
using Gameplay.Core.Abilities;
using UnityEngine;

namespace Gameplay.Core.Targets
{
    [Serializable]
    public abstract class TargetSelectorBase : ScriptableObject, ITargetSelector
    {
        public abstract List<IEntity> SelectTargets(IEntity caster);
    }
}