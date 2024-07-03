// Onur Ereren - June 2024
// Popcore case

using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace PopsBubble
{
    public class ResolveState : GameState
    {
        private HexGrid _grid;
        
        private IChainCalculator _chainCalculator;
        private IIslandCalculator _islandCalculator;
        private IRaycaster _shootRaycaster;
        private BubbleRaycaster _raycaster;

        public ResolveState()
        {
            _grid = DependencyContainer.Grid;
            
            _chainCalculator = DependencyContainer.ChainCalculator;
            _islandCalculator = DependencyContainer.IslandCalculator;
            _shootRaycaster = DependencyContainer.ShootRaycaster;
            _raycaster = DependencyContainer.BubbleRaycaster;
        }
        

        public override async void OnEnter()
        {
            ShootRaycastResult raycastResult = _shootRaycaster.ShootResult();
            HexCell startingCell = raycastResult.LandingCell;

            while (startingCell != null)
            {
                ChainSearchResult chainResult = _chainCalculator.FindChain(startingCell);
                startingCell = await MergeChain(chainResult);
            }

            List<HexCell> islandCells = _islandCalculator.CalculateIslandCells();

            await DetachIslands(islandCells);
            
            //await _raycaster.CalculatePop();
            
            OnStateComplete?.Invoke();
        }


        private async UniTask<HexCell> MergeChain(ChainSearchResult searchResult)
        {
            return null;
        }

        private async UniTask DetachIslands(List<HexCell> islandCells)
        {
            
        }

        private void PaintChain(ChainSearchResult result)
        {
            foreach (HexCell cell in result.ValueCells)
            {
                if (cell.Bubble == null)
                {
                    Debug.Log($"There's no bubble in {cell.Coordinates}");
                    continue;
                }
                
                cell.Bubble.GetComponent<SpriteRenderer>().color = Color.green;
            }

            foreach (HexCell cell in result.NeighbourCells)
            {
                if (cell.Bubble == null)
                {
                    Debug.Log($"There's no bubble in {cell.Coordinates}");
                    continue;
                }
                
                cell.Bubble.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
        }
        
    }
}
