// Onur Ereren - July 2024
// Popcore case

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace PopsBubble
{
    public interface IPathMover
    {
        public UniTask MoveOnPath(Transform target, List<Vector2> waypoints);
    }
}
