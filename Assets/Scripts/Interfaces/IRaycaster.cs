// Onur Ereren - July 2024
// Popcore case

using UnityEngine;

namespace PopsBubble
{
    public interface IRaycaster
    {
        public ShootRaycastResult ShootRaycast(Vector2 direction);

        public ShootRaycastResult ShootResult();

        public void ClearShootResult();
    }
}
