using System;
using System.Collections.Generic;
using Gameplay.Abilities;
using Gameplay.Core;
using Gameplay.Core.Abilities;
using UnityEngine;

namespace Gameplay.Entities
{
    [RequireComponent(typeof(CharacterController))]
    public class BaseCharacterAdapter : MonoBehaviour, IEntity
    {
        public string ID => gameObject.name;
        public Vector3 Forward => transform.forward;
        public Vector3 Position => transform.position;
        public Quaternion Rotation => transform.rotation;

        private IEntityRestrictionController _restrictionController;
        
        public IEntityRestrictionController RestrictionController =>
            _restrictionController ??= new DefaultRestrictionController();
        
        [SerializeField]
        private List<AbilityConfig> _abilityConfigs;

        public IReadOnlyList<AbilityConfig> AbilityConfigs => _abilityConfigs;

        private readonly Dictionary<Type, object> _componentsHub = new();

        private void Awake()
        {
            RegisterComponentProviders();
        }

        public void RegisterComponentProviders()
        {
            foreach (var component in GetComponents<MonoBehaviour>())
            {
                _componentsHub[component.GetType()] = component;
                foreach (var interfaceType in component.GetType().GetInterfaces())
                {
                    _componentsHub[interfaceType] = component;
                }
            }
        }

        public T GetComponentProvider<T>() where T : class
        {
            if (_componentsHub.TryGetValue(typeof(T), out var provider))
            {
                return provider as T;
            }

            return null;
        }
    }
}