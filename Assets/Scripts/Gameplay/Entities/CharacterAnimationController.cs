using Gameplay.Core;
using Gameplay.Core.Components;
using UnityEngine;

namespace Gameplay.Entities
{
    public class CharacterAnimationController : MonoBehaviour, IAnimationController
    {
        [SerializeField]
        private Animator _characterAnimator;

        private readonly int VELOCITY_INT_KEY = Animator.StringToHash("Velocity");

        public void SetTrigger(string trigger)
        {
            _characterAnimator.SetTrigger(trigger);
        }

        public void SetVelocity(float velocity)
        {
            _characterAnimator.SetFloat(VELOCITY_INT_KEY, velocity);
        }
    }
}