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

namespace PopsBubble
{
    public class Bubble : MonoBehaviour
    {
        private GameFlow _gameFlow;
        private BubblePool _pool;
        private HexGrid _grid;
        private float _dropDuration;

        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private TextMeshPro _valueText;
        [SerializeField] private ParticleSystem _particles;
        
        [SerializeField] private Color _defaultTextColor;

        [SerializeField] private int _deafultSortOrder;
        
        public void SetReferences(GameFlow flow, BubblePool pool, HexGrid grid)
        {
            _gameFlow = flow;
            _pool = pool;
            _grid = grid;
            _dropDuration = GameVar.GridDropDuration;
        }
        
        
        public void Initialize(int value)
        {
            Paint(value);
            _renderer.sortingOrder = _deafultSortOrder;
            _valueText.sortingOrder = _deafultSortOrder + 1;
            
            _valueText.color = _defaultTextColor;
            _valueText.text = GameVar.DisplayValue(value).ToString("F00");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _particles.Play();
            }
        }

        public void SwitchValue(int value)
        {
            _valueText.DOText(GameVar.DisplayValue(value).ToString("F00"), GameVar.BubbleValueSwitchDuration);
            _renderer.DOColor(_gameFlow.ColorByValue(value), GameVar.BubbleValueSwitchDuration);
            Paint(value);
        }
        
        public void SendToBack()
        {
            _renderer.sortingOrder = 0;
            _valueText.sortingOrder = 1;
            _valueText.DOColor(new Color(0f, 0f, 0f, 0f), 0.1f);
        }

        public void Pop()
        {
            _particles.Play();
            _pool.Recall(this);
        }

        public void Detach()
        {
            Pop();
        }

        public void PlayParticles()
        {
            _particles.Play();
        }
        
        private void Paint(int value)
        {
            _renderer.color = _gameFlow.ColorByValue(value);
            ParticleSystem particles = _particles;
            ParticleSystem.MainModule main = particles.main;
            main.startColor = new ParticleSystem.MinMaxGradient(_gameFlow.ColorByValue(value));
        }
    }   
}