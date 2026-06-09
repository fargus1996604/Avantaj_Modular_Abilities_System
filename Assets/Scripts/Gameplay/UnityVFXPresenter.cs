using System.Collections.Generic;
using Gameplay.Abilities.Data;
using Unity.Mathematics;
using UnityEngine;

namespace Gameplay
{
    public class UnityVFXPresenter : MonoBehaviour
    {
        private Dictionary<string, GameObject> _cache = new Dictionary<string, GameObject>();

        private void OnEnable()
        {
            VFXAction.HandleVFXEvent += HandleSpawnRequest;
        }

        private void OnDisable()
        {
            VFXAction.HandleVFXEvent -= HandleSpawnRequest;
        }

        private void HandleSpawnRequest(VFXSpawnArgs args)
        {
            switch (args.RequestType)
            {
                case VFXRequestType.Spawn:
                    Spawn(args);
                    break;
                case VFXRequestType.Stop:
                    Stop(args);
                    break;
            }
        }

        private void Spawn(VFXSpawnArgs args)
        {
            if (args.Entity is MonoBehaviour unityTarget)
            {
                var effect = Instantiate(args.Prefab, unityTarget.transform);
                effect.transform.localRotation = quaternion.identity;
                effect.transform.localPosition = args.Offset;
                _cache[args.ID] = effect;
            }
        }

        private void Stop(VFXSpawnArgs args)
        {
            if (_cache.TryGetValue(args.ID, out GameObject effect))
            {
                if (effect != null)
                {
                    Destroy(effect);
                }

                _cache.Remove(args.ID);
            }
        }
    }
}