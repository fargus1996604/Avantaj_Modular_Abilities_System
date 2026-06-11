using Gameplay.Core.Abilities;

namespace Gameplay.Core.Components
{
    public interface IDamageable : IEntityComponent
    {
        void TakeDamage(int damage);
    }
}
