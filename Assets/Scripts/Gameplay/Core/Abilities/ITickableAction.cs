namespace Gameplay.Core.Abilities
{
    public interface ITickableAction : IAbilityAction
    {
        bool Tick(IAbilityContext context, float deltaTime);
    }
}