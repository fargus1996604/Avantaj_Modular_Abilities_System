using Gameplay.Core.Abilities;

namespace Gameplay.Core.Components
{
    public interface IHealable : IEntityComponent
    {
        void Heal(int amount);
    }
}