using Gameplay.Core.Abilities;

namespace Gameplay.Core.Components
{
    public interface IAnimationController : IEntityComponent
    {
        void SetTrigger(string trigger);
    }
}