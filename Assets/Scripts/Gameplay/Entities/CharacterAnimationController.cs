using Gameplay.Core;
using UnityEngine;

namespace Gameplay.Entities
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimationController : MonoBehaviour, IAnimationController
    {
        private Animator _characterAnimator;
        protected Animator CharacterAnimator => _characterAnimator ??= GetComponent<Animator>();

        private readonly int VELOCITY_INT_KEY = Animator.StringToHash("Velocity");

        public void SetTrigger(string trigger)
        {
            CharacterAnimator.SetTrigger(trigger);
        }

        public void SetVelocity(float velocity)
        {
            CharacterAnimator.SetFloat(VELOCITY_INT_KEY, velocity);
        }
    }
}