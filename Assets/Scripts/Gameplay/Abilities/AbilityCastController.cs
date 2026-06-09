using System.Collections.Generic;
using System.Linq;
using Gameplay.Core.Abilities;
using Gameplay.Entities;
using UnityEngine;

namespace Gameplay.Abilities
{
    [RequireComponent(typeof(BaseCharacterAdapter), typeof(AbilitiesHandler))]
    public class AbilityCastController : MonoBehaviour
    {
        private BaseCharacterAdapter _characterAdapter;
        protected BaseCharacterAdapter CharacterAdapter => _characterAdapter ??= GetComponent<BaseCharacterAdapter>();

        private AbilitiesHandler _abilitiesHandler;
        protected AbilitiesHandler AbilitiesHandler => _abilitiesHandler ??= GetComponent<AbilitiesHandler>();

        [SerializeField]
        private float _searchRadius = 10f;

        [SerializeField]
        private LayerMask _enemyLayer;

        [SerializeField] 
        [Range(0f, 360f)]
        private float _viewAngle = 90f;

        private IEntity _entity;

        private void Awake()
        {
            _entity = CharacterAdapter;
        }

        public void TryCastSlot(int index)
        {
            var configs = CharacterAdapter.AbilityConfigs;
            if (configs == null || index < 0 || index >= configs.Count) return;

            ExecuteCast(configs.ElementAt(index));
        }

        private void ExecuteCast(AbilityConfig config)
        {
            if (AbilitiesHandler == null)
            {
                return;
            }

            List<IEntity> targetEnemies = FindAllEnemiesInCone();
            var context = new DefaultContext(_entity, targetEnemies);
            var activeAbility = new ActiveAbility(context, config.GetAbilityActions());
            AbilitiesHandler.RegisterAbility(activeAbility);
        }
        
        private List<IEntity> FindAllEnemiesInCone()
        {
            List<IEntity> validTargets = new List<IEntity>();
            Collider[] colliders = Physics.OverlapSphere(transform.position, _searchRadius, _enemyLayer);
            Vector3 forwardDirection = transform.forward;
            foreach (var col in colliders)
            {
                if (col.TryGetComponent<IEntity>(out var enemy))
                {
                    Vector3 directionToEnemy = col.transform.position - transform.position;
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

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _searchRadius);

            Vector3 forward = transform.forward;
            Vector3 leftRayDirection = Quaternion.Euler(0, -_viewAngle / 2f, 0) * forward;
            Vector3 rightRayDirection = Quaternion.Euler(0, _viewAngle / 2f, 0) * forward;

            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(transform.position, leftRayDirection * _searchRadius);
            Gizmos.DrawRay(transform.position, rightRayDirection * _searchRadius);
        }
    }
}