using Gameplay.Core;
using Gameplay.Core.Components;
using UnityEngine;

namespace Gameplay.Entities
{
    public class SoundComponent : MonoBehaviour, ISoundController
    {
        [SerializeField]
        private AudioSource _source;

        public void PlayOneShot(AudioClip clip)
        {
            _source.PlayOneShot(clip);
        }
    }
}