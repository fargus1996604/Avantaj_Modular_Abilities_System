using Gameplay.Core.Abilities;
using UnityEngine;

namespace Gameplay.Core.Components
{
    public interface ISoundController : IEntityComponent
    {
        void PlayOneShot(AudioClip clip);
    }
}