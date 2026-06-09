using UnityEngine;

namespace Gameplay.Core.Abilities
{
    public interface IEntity
    {
        Vector3 Forward { get; }
        Vector3 Position { get; }
        IEntityRestrictionController RestrictionController { get; }
    }
}