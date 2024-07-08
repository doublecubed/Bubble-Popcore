// Onur Ereren - June 2024
// Popcore case

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.Serialization;
using System.Threading;
using Random = UnityEngine.Random;

namespace PopsBubble
{
    public class Bubble : MonoBehaviour
    {
        #region REFERENCES

        private HexGrid _grid;
        private GameFlow _gameFlow;
        private BubblePool _pool;
        private float _dropDuration;

        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private TextMeshPro _valueText;
        [SerializeField] private ParticleSystem _particles;
        [SerializeField] private Rigidbody2D _rigidbody;
        
        #endregion
        
        #region VARIABLES
        
        [SerializeField] private Color _defaultTextColor;
        [SerializeField] private int _deafultSortOrder;

        private readonly float _mergeUnderDuration = GameVar.BubbleMergeDuration * 0.5f;
        private readonly Color _mergeUnderColour = new Color(0f, 0f, 0f, 0f);
        
        private CancellationToken _ct;

        private bool _hasHitBottom;
        
        #endregion
       
        #region MONOBEHAVIOUR

        // This is a hack. Bubbles should not run their own updates. I should refactor this later
        private void Update()
        {
            if (_rigidbody.simulated && transform.position.y <= -1f && !_hasHitBottom)
            {
                HitBottom();
            }
        }

        #endregion
        
        #region METHODS
        
        #region Initialization
        
        // For first creation into the Pool
        public void SetReferences(GameFlow flow, BubblePool pool, HexGrid grid)
        {
            _grid = grid;
            _gameFlow = flow;
            _pool = pool;
            _dropDuration = GameVar.GridDropDuration;

            _ct = new CancellationToken();
        }
        
        // Every time it is dispensed from the pool
        public void Initialize(int value)
        {
            PaintBubble(value);
            _renderer.sortingOrder = _deafultSortOrder;
            _valueText.sortingOrder = _deafultSortOrder + 1;
            
            _valueText.color = _defaultTextColor;
            _valueText.text = GameVar.DisplayValue(value).ToString("F00");
        }

        public void ResetBubble()
        {
            _hasHitBottom = false;
            SwitchRigidbody(false);
            MakeVisible(true);
        }
        
        #endregion

        #region Merge & Pop
        
        // The main scope of the merge changes its value and colour.
        public void SwitchValue(int value)
        {
            _valueText.DOText(GameVar.DisplayValue(value).ToString("F00"), GameVar.BubbleValueSwitchDuration);
            _renderer.DOColor(_gameFlow.ColorByValue(value), GameVar.BubbleValueSwitchDuration);
            PaintBubble(value);
        }
        
        // Merged block goes under
        public async UniTask MergeUnder(HexCell targetCell)
        {
            _renderer.sortingOrder = 0;
            _valueText.sortingOrder = 1;
            _particles.Play();

            UniTask[] mergeTasks = new UniTask[2];
            mergeTasks[0] = _valueText.DOColor(_mergeUnderColour, _mergeUnderDuration).WithCancellation(_ct);
            mergeTasks[1] =  transform.DOMove(_grid.CellPosition(targetCell), GameVar.BubbleMergeDuration)
                .WithCancellation(_ct);
            await UniTask.WhenAll(mergeTasks);
        }
        
        public void Pop()
        {
            SwitchRigidbody(true);
            _rigidbody.AddForce(RandomForceDirection() * GameVar.BubblePopForce);
            _particles.Play();
            //_pool.Recall(this);
        }

        #endregion
        
        #region Internal
        
        public void PlayParticles()
        {
            _particles.Play();
        }
        
        private void PaintBubble(int value)
        {
            _renderer.color = _gameFlow.ColorByValue(value);
            ParticleSystem particles = _particles;
            ParticleSystem.MainModule main = particles.main;
            main.startColor = new ParticleSystem.MinMaxGradient(_gameFlow.ColorByValue(value));
        }

        private Vector2 RandomForceDirection()
        {
            float horizontal = Random.Range(GameVar.BubblePopLateralVectorMin, GameVar.BubblePopLateralVectorMax);
            bool goesLeft = Random.Range(0, 2) == 0;

            float vertical = Random.Range(GameVar.BubblePopVerticalVectorMin, GameVar.BubblePopVerticalVectorMax);

            return new Vector2((goesLeft ? -1f : 1f) * horizontal, vertical);
        }

        private async void HitBottom()
        {
            PlayParticles();
            MakeVisible(false);
            SwitchRigidbody(false);

            // This should be configured according to the particle system duration.
            await UniTask.Delay(1000);
            
            _pool.Recall(this);
        }

        private void MakeVisible(bool value)
        {
            _renderer.enabled = value;
            _valueText.enabled = value;
        }

        private void SwitchRigidbody(bool value)
        {
            _rigidbody.simulated = value;
        }
        
        
        #endregion
        
        #endregion
    }   
}