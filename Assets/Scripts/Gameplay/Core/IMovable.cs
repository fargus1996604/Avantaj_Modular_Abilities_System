using UnityEngine;

namespace Gameplay.Core
{
    public interface IMovable
    {
        void Move(Vector3 delta);
        void LookAt(Quaternion quaternion);
    }
}