// Onur Ereren
// June 2024

// A very crude Dependency Injection system. This is the container.
// CentralReference feeds this with objects references from the scene.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PopsBubble
{
    public static class DependencyContainer
    {
        public static Transform ShootingPoint { get; private set; }
        
        public static HexGrid Grid { get; private set; }
        public static GameFlow GameFlow { get; private set; }
        
        public static PlayerInput PlayerInput { get; private set; }
        
        public static IShootIndicator ShootIndicator { get; private set; }
        
        public static BubbleRaycaster BubbleRaycaster { get; private set; }
        public static IRaycaster ShootRaycaster { get; private set; }
        public static IShootValueCalculator ShootCalculator { get; private set; }
        
        public static BubblePool BubblePool { get; private set; }
        
        public static IPathMover PathMover { get; private set; }
        
        public static IPathDrawer PathDrawer { get; private set; }
        
        public static void Initialize(Transform shootingPoint, HexGrid grid, GameFlow flow, PlayerInput input, 
            IShootIndicator shootIndicator, BubbleRaycaster raycaster,
            BubblePool pool, IPathMover mover, IPathDrawer drawer)
        {
            ShootingPoint = shootingPoint;
            
            Grid = grid;
            GameFlow = flow;
            PlayerInput = input;
            ShootIndicator = shootIndicator;
            BubbleRaycaster = raycaster;
            BubblePool = pool;
            PathMover = mover;
            PathDrawer = drawer;
            
            ShootCalculator = new ShootCalculator(1, flow.LevelProfile.MaximumShootValue);
            ShootRaycaster = new ShootRaycaster();
        }
    }

}