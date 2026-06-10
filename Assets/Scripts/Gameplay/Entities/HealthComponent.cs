using Gameplay.Core;
using UnityEngine;
using Input = UnityEngine.Windows.Input;

namespace Gameplay.Entities
{
    public class HealthComponent : MonoBehaviour, IDamageable, IHealable
    {
        [SerializeField]
        private int _currentHealth = 100;

        public int CurrentHealth => _currentHealth;

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, 100);
            Debug.Log($"Damage: {damage}");
        }

        public void Heal(int amount)
        {
            _currentHealth += amount;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, 100);
            Debug.Log($"Healing to {amount}");
        }
    }
}