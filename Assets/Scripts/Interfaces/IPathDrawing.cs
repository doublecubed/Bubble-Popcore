// Onur Ereren - June 2024
// Popcore case

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PopsBubble
{
    public interface IPathDrawing
    {
        public void DrawPath(List<Vector2> points);

        public void ClearPath();
    }
}