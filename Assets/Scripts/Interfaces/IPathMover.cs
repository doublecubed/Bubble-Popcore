// Onur Ereren - July 2024
// Popcore case

using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace PopsBubble
{
    public interface IPathMover
    {
        public UniTask MoveOnPath(List<Vector2> waypoints);

        public void ResetPosition();
    }
}
