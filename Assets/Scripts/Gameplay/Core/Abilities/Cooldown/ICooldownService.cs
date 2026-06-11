namespace Gameplay.Core.Abilities.Cooldown
{
    public interface ICooldownService
    {
        bool IsReady(string abilityId);
        void Register(string abilityId, float duration);
    }
}
