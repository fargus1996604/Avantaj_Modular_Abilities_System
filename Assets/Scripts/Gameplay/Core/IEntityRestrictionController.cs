namespace Gameplay.Core
{
    public interface IEntityRestrictionController
    {
        void Register(EntityRestrictionType restrictionType, object owner);
        void Unregister(EntityRestrictionType restrictionType, object owner);
        bool HasRestriction(EntityRestrictionType restrictionType);
    }
}