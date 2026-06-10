namespace Gameplay.Core.Abilities
{
    public interface IAbilityAction
    {
        void Execute(IAbilityContext context);
    }
}