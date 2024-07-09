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
        #region PREFAB REFERENCES
        
        public static GameObject BubblePrefab { get; private set; }
        public static GameObject HexCellPrefab { get; private set; }
        public static GameObject MergePointPrefab { get; private set; }
        #endregion
        
        #region TRANSFORM REFERENCES
        
        public static Transform ShootingPoint { get; private set; }
        public static Transform GhostBubble { get; private set; }
        public static Transform MoverTrail { get; private set; }
        
        #endregion
        
        #region SCRIPT REFERENCES
        public static HexGrid Grid { get; private set; }
        public static GameFlow GameFlow { get; private set; }
        public static PlayerInput PlayerInput { get; private set; }
        public static IShootIndicator ShootIndicator { get; private set; }
        public static IRaycaster ShootRaycaster { get; private set; }
        public static IShootValueCalculator ShootCalculator { get; private set; }
        public static BubblePool BubblePool { get; private set; }
        public static IPathMover PathMover { get; private set; }
        public static IPathDrawer PathDrawer { get; private set; }
        public static IChainCalculator ChainCalculator { get; private set; }
        public static IIslandCalculator IslandCalculator { get; private set; }
        
        #endregion
        
        public static void Initialize(GameObject bubblePrefab, GameObject hexCellPrefab, GameObject mergePointPrefab,
            Transform shootingPoint, Transform ghostBubble, Transform moverTrail,
            HexGrid grid, GameFlow flow, PlayerInput input, 
            IShootIndicator shootIndicator,
            BubblePool pool, IPathDrawer drawer)
        {
            BubblePrefab = bubblePrefab;
            HexCellPrefab = hexCellPrefab;
            MergePointPrefab = mergePointPrefab;
            
            ShootingPoint = shootingPoint;
            GhostBubble = ghostBubble;
            MoverTrail = moverTrail;
            
            Grid = grid;
            GameFlow = flow;
            PlayerInput = input;
            ShootIndicator = shootIndicator;
            BubblePool = pool;
            PathDrawer = drawer;

            PathMover = new BubbleTrailMover();
            ShootCalculator = new ShootValueCalculator(1, flow.LevelProfile.MaximumShootValue);
            ShootRaycaster = new ShootRaycaster();
            ChainCalculator = new ChainCalculator();
            IslandCalculator = new IslandCalculator();
        }
    }

}