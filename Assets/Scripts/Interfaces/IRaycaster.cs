// Onur Ereren - July 2024
// Popcore case

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PopsBubble
{
    public interface IRaycaster
    {
        public ShootRaycastResult ShootRaycast(Vector2 direction);
    }
}
