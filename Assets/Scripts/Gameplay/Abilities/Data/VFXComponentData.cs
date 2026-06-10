using System;
using System.Collections.Generic;
using Gameplay.Core.Abilities;
using UnityEngine;

namespace Gameplay.Abilities.Data
{
    public enum DurationType
    {
        Instant,
        Continuous
    }

    public enum VFXRequestType
    {
        Spawn,
        Stop
    }

    [Serializable]
    public class VFXComponentData : IAbilityComponentData
    {
        public GameObject Prefab;
        public Vector3 Offset;
        public TargetType Target;
        public DurationType DurationType;
        public float Duration;
        public PlayTimeData PlayTime;

        public IAbilityAction CreateRuntimeAction()
        {
            return new VFXAction(Prefab, Offset, Target, DurationType, Duration, PlayTime);
        }
    }

    public class VFXAction : ContinuousDelayedActionBase
    {
        public static event Action<VFXSpawnArgs> HandleVFXEvent;

        private readonly GameObject _prefab;
        private readonly Vector3 _offset;
        private readonly TargetType _target;
        private readonly List<string> _guids = new();

        public VFXAction(GameObject prefab, Vector3 offset, TargetType target, 
            DurationType durationType, float duration, PlayTimeData playTime) 
            : base(playTime, durationType, duration)
        {
            _prefab = prefab;
            _offset = offset;
            _target = target;
        }

        protected override void OnContinuousStart(IAbilityContext context)
        {
            if (_target == TargetType.Owner)
            {
                Spawn(context.Owner);
            }
            else if (_target == TargetType.Enemy)
            {
                foreach (var target in context.Targets)
                {
                    Spawn(target);
                }
            }
        }

        protected override void OnContinuousEnd(IAbilityContext context)
        {
            foreach (var guid in _guids)
            {
                HandleVFXEvent?.Invoke(VFXSpawnArgs.RequestStop(guid));
            }
            _guids.Clear();
        }

        private void Spawn(IEntity entity)
        {
            if (entity == null) return;
            
            var newGuid = Guid.NewGuid().ToString();
            _guids.Add(newGuid);
            
            HandleVFXEvent?.Invoke(new VFXSpawnArgs(newGuid, VFXRequestType.Spawn, _prefab, _offset, entity));
        }
    }

    public struct VFXSpawnArgs
    {
        public string ID;
        public VFXRequestType RequestType;
        public GameObject Prefab;
        public Vector3 Offset;
        public IEntity Entity;

        public VFXSpawnArgs(string id, VFXRequestType requestType, GameObject prefab, Vector3 offset, IEntity entity)
        {
            ID = id;
            RequestType = requestType;
            Prefab = prefab;
            Offset = offset;
            Entity = entity;
        }

        public static VFXSpawnArgs RequestStop(string id)
        {
            return new VFXSpawnArgs(id, VFXRequestType.Stop, null, Vector3.zero, null);
        }
    }
}