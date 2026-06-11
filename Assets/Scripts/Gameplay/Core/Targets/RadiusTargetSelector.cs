using System.Collections.Generic;
using Gameplay.Core.Abilities;
using UnityEngine;

namespace Gameplay.Core.Targets
{
    [CreateAssetMenu(fileName = "RadiusTargetSelector", menuName = "Config/Targets/RadiusTargetSelector")]
    public class RadiusTargetSelector : TargetSelectorBase
    {
        [SerializeField]
        private float _searchRadius = 10;

        [SerializeField]
        private LayerMask _enemyLayer;

        public override List<IEntity> SelectTargets(IEntity caster)
        {
            List<IEntity> validTargets = new List<IEntity>();
            Collider[] colliders = Physics.OverlapSphere(caster.Position, _searchRadius, _enemyLayer);
            foreach (var col in colliders)
            {
                if (col.TryGetComponent<IEntity>(out var enemy))
                {
                    validTargets.Add(enemy);
                }
            }

            return validTargets;
        }
    }
}