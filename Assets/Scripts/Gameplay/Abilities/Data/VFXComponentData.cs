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

        public IAbilityAction CreateRuntimeAction()
        {
            return new VFXAction(Prefab, Offset, Target, DurationType, Duration);
        }
    }

    public class VFXAction : ITickableAction
    {
        public static event Action<VFXSpawnArgs> HandleVFXEvent;

        private GameObject _prefab;
        private Vector3 _offset;
        private TargetType _target;
        private DurationType _durationType;
        private float _duration;
        private float _elapsedTime;
        private List<string> _guids = new();

        public VFXAction(GameObject prefab, Vector3 offset, TargetType target, DurationType durationType,
            float duration)
        {
            _prefab = prefab;
            _offset = offset;
            _target = target;
            _durationType = durationType;
            _duration = duration;
        }

        public void Execute(IAbilityContext context)
        {
            if (context.Owner == null)
                return;

            if (_target == TargetType.Player)
            {
                var args = GetSpawnArgs(context.Owner);
                _guids.Add(args.ID);
                HandleVFXEvent?.Invoke(args);
            }
            else
            {
                foreach (var contextTarget in context.Targets)
                {
                    var args = GetSpawnArgs(contextTarget);
                    _guids.Add(args.ID);
                    HandleVFXEvent?.Invoke(args);
                }
            }
        }

        private VFXSpawnArgs GetSpawnArgs(IEntity target)
        {
            var newGuid = Guid.NewGuid().ToString();
            return new VFXSpawnArgs(newGuid, VFXRequestType.Spawn, _prefab, _offset, target);
        }

        public bool Tick(IAbilityContext context, float deltaTime)
        {
            if (_durationType == DurationType.Instant)
                return true;

            _elapsedTime += deltaTime;
            if (_elapsedTime >= _duration)
            {
                foreach (var guid in _guids)
                {
                    HandleVFXEvent?.Invoke(VFXSpawnArgs.RequestStop(guid));
                }

                return true;
            }

            return false;
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