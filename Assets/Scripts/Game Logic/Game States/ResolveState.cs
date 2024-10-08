// Onur Ereren - June 2024
// Popcore case

// Resolves the grid after the bubble is placed.
// Calculates chains, maximum valued cells and island cells.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Lofelt.NiceVibrations;  
using DG.Tweening;

namespace PopsBubble
{
    public class ResolveState : GameState
    {
        #region REFERENCES
        
        private HexGrid _grid;
        
        private IChainCalculator _chainCalculator;
        private IIslandCalculator _islandCalculator;
        private IRaycaster _shootRaycaster;
        private BubblePool _pool;
        
        private CancellationToken _ct;

        private GameObject _mergePointPrefab;
        
        #endregion
        
        #region CONSTRUCTOR
        
        public ResolveState()
        {
            _grid = DependencyContainer.Grid;
            
            _chainCalculator = DependencyContainer.ChainCalculator;
            _islandCalculator = DependencyContainer.IslandCalculator;
            _shootRaycaster = DependencyContainer.ShootRaycaster;
            _pool = DependencyContainer.BubblePool;

            _ct = new CancellationToken();

            _mergePointPrefab = DependencyContainer.MergePointPrefab;
        }

        #endregion
        
        #region METHODS
        
        #region State Operation
        
        public override async void OnEnter()
        {
            await DetectAndMergeChains();

            await DetectAndDetachMaxedCells();
            
            DetectAndDetachIslands();

            OnStateComplete?.Invoke();
        }

        #endregion
        
        #region Internal

        private async UniTask DetectAndMergeChains()
        {
            ShootRaycastResult raycastResult = _shootRaycaster.ShootResult();

            HexCell startingCell = raycastResult.LandingCell;

            while (true)
            {
                ChainSearchResult firstChain = _chainCalculator.FindChain(startingCell);
                if (firstChain.ValueCells.Count <= 1) break;

                int mergedValue = MergedValue(firstChain.Value, firstChain.ValueCells.Count);

                // Get all the neighbours who have the final value after the merge
                List<HexCell> potentialMergeNeighbours =
                    firstChain.NeighbourCells.Where(cell => cell.Value == mergedValue).ToList();

                // If there are none, just merge the chain into the preferred cell and get out
                if (potentialMergeNeighbours.Count <= 0)
                {
                    await MergeCells(PreferredCell(firstChain.ValueCells), firstChain.ValueCells, mergedValue);
                    break;
                }

                // There are neighbours with merged value, so their chains will be calculated and the longest one will be selected.
                HexCell preferredNeighbour = LongestChainNeighbour(potentialMergeNeighbours);

                // Get the neighbours of the selected neighbour, to find the best target cell to merge into
                HexCell[] neighboursOfPreferred = _grid.NeighbourCells(preferredNeighbour);
                List<HexCell> firstChainCells = neighboursOfPreferred.Where(cell => cell != null && firstChain.ValueCells.Contains(cell)).ToList();

                HexCell mergeTargetForFirstChain = PreferredCell(firstChainCells);
                
                await MergeCells(mergeTargetForFirstChain, firstChain.ValueCells, mergedValue);

                // Set the starting cell, so that the while loop will start from here.
                startingCell = mergeTargetForFirstChain;

            }
        }

        private async UniTask DetectAndDetachMaxedCells()
        {
            List<HexCell> maxedCells =
                _grid.CellMap.Values.Where(cell => cell.Value >= GameVar.MaximumPowerValue).ToList();

            List<HexCell> cellsToPop = new List<HexCell>();
            List<UniTask> centralPopTasks = new List<UniTask>();
            
            foreach (HexCell cell in maxedCells)
            {
                centralPopTasks.Add(cell.Bubble.transform.DOScale(GameVar.MaxValueCellPopScale, GameVar.MaxValueCellPopDuration).WithCancellation(_ct));
                
                HexCell[] neighbourCells = _grid.NeighbourCells(cell);
                List<HexCell> liveCells = neighbourCells.Where(cell => cell != null && cell.Value > 0).ToList();
                liveCells.Add(cell);
                
                cellsToPop.AddRange(liveCells);
            }

            await UniTask.WhenAll(centralPopTasks);
            
            foreach (HexCell cell in cellsToPop)
            {
                cell.Clear();
            }
        }
        
        private void DetectAndDetachIslands()
        {
            List<HexCell> islandCells = _islandCalculator.CalculateIslandCells();

            if (islandCells.Count >= GameVar.IslandPopSoundTreshold)
            {
                AudioPlayer.PlayAudio("popSeries");
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);
            }
            
            foreach (HexCell cell in islandCells)
            {
                cell.Clear();
            }
        }
        
        private HexCell LongestChainNeighbour(List<HexCell> potentialNeighbours)
        {
            List<ChainSearchResult> potentialChains = _chainCalculator.BurstChain(potentialNeighbours);

            int maximumLength = potentialChains.Max(chain => chain.Length);
            List<ChainSearchResult> maximumChains = potentialChains.Where(chain => chain.Length == maximumLength).ToList();
                
            // In case there are multiple neighbours with the same chain length
            HexCell preferredNeighbour = maximumChains[0].StartingCell;
            if (maximumChains.Count > 1)
            {
                List<HexCell> allNeighbours = maximumChains.Select(chain => chain.StartingCell).ToList();
                preferredNeighbour = PreferredCell(allNeighbours);
            }

            return preferredNeighbour;
        }
 
        private async UniTask MergeCells(HexCell mergeTarget, List<HexCell> mergeCells, int newValue)
        {
            Vector2 targetPosition = _grid.CellPosition(mergeTarget);

            if (mergeCells.Contains(mergeTarget)) mergeCells.Remove(mergeTarget);
            
            List<UniTask> moveTasks = new List<UniTask>();
            List<Bubble> clearList = new List<Bubble>();
            
            for (int i = 0; i < mergeCells.Count; i++)
            {
                Bubble mergeBubble = mergeCells[i].Bubble;

                clearList.Add(mergeBubble);
                moveTasks.Add(mergeBubble.MergeUnder(mergeTarget));
                
                mergeCells[i].Clear();
            }

            if (mergeCells.Count >= GameVar.IslandPopSoundTreshold)
            {
                AudioPlayer.PlayAudio("popSeries");
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);
            }
            else
            {
                AudioPlayer.PlayAudio("pop");
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
            }

            
            await UniTask.WhenAll(moveTasks);


            
            foreach (Bubble bubble in clearList)
            {
                _pool.Recall(bubble);
            }
            
            mergeTarget.SwitchValue(newValue);
            mergeTarget.Bubble.SwitchValue(newValue);
            
            SpawnMergePoint(mergeTarget);
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

        private int MergedValue(int initialValue, int chainLength)
        {
            return initialValue + (chainLength - 1);
        }

        private HexCell PreferredCell(List<HexCell> cells)
        {
            return cells.OrderByDescending(cell => cell.Coordinates.y)
                .ThenBy(cell => cell.Coordinates.x)
                .FirstOrDefault();
        }

        private void SpawnMergePoint(HexCell cell)
        {
            GameObject point = Object.Instantiate(_mergePointPrefab);
            point.transform.position = _grid.CellPosition(cell);
            MergePoint pointScript = point.GetComponent<MergePoint>();
            pointScript.Initialize(cell.Value);
        }
        
        #endregion
        
        #endregion
        
    }
}
