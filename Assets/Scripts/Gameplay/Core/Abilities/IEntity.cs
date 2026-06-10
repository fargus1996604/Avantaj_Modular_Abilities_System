using UnityEngine;

namespace Gameplay.Core.Abilities
{
    public interface IEntity
    {
        public string ID { get; }
        Vector3 Forward { get; }
        Vector3 Position { get; }
        Quaternion Rotation { get; }
        IEntityRestrictionController RestrictionController { get; }
        T GetComponentProvider<T>() where T : class;
    }
}