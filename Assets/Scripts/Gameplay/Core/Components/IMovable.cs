using Gameplay.Core.Abilities;
using UnityEngine;

namespace Gameplay.Core.Components
{
    public interface IMovable : IEntityComponent
    {
        void Move(Vector3 delta);
        void LookAt(Quaternion quaternion);
        void SetMoveAxis(Vector2 axis);
    }
}