using System.Collections.Generic;
using Gameplay.Core.Abilities;
using UnityEngine;

namespace Gameplay.Core.Targets
{
    [CreateAssetMenu(fileName = "ConeTargetSelector", menuName = "Config/Targets/ConeTargetSelector")]
    public class ConeTargetSelector : TargetSelectorBase
    {
        [SerializeField]
        private float _searchRadius = 10;

        [Range(0, 360)]
        [SerializeField]
        private float _viewAngle = 90;

        [SerializeField]
        private LayerMask _enemyLayer;

        public override List<IEntity> SelectTargets(IEntity caster)
        {
            List<IEntity> validTargets = new List<IEntity>();
            Collider[] colliders = Physics.OverlapSphere(caster.Position, _searchRadius, _enemyLayer);
            Vector3 forwardDirection = caster.Forward;
            foreach (var col in colliders)
            {
                if (col.TryGetComponent<IEntity>(out var enemy))
                {
                    Vector3 directionToEnemy = col.transform.position - caster.Position;
                    directionToEnemy.y = 0f;

                    float angleToEnemy = Vector3.Angle(forwardDirection, directionToEnemy);
                    if (angleToEnemy <= _viewAngle / 2f)
                    {
                        validTargets.Add(enemy);
                    }
                }
            }

            return validTargets;
        }
    }
}