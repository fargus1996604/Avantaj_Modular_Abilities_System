using UnityEngine;

namespace Gameplay.Core
{
    public interface ISoundController
    {
        void PlayOneShot(AudioClip clip);
    }
}