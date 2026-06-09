namespace Gameplay.Core.Abilities
{
    public interface IAbilityComponentData
    {
        IAbilityAction CreateRuntimeAction();
    }
}
