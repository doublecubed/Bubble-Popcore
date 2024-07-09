// Onur Ereren - July 2024
// Popcore case

// Floating number that appears after a merge happens
// Has no pool. Destroys itself at the end

using TMPro;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace PopsBubble
{    
    public class MergePoint : MonoBehaviour
    {
        #region REFERENCES
        
        [Header("References")]
        [SerializeField] private TextMeshPro _valueText;
        private Transform _valueTransform;
        
        [Header("Colours")]
        [SerializeField] private Color _textStartingColour;
        [SerializeField] private Color _textEndColour;

        [Header("Fade Easing")]
        [SerializeField] private AnimationCurve _fadeCurve;
        
        #endregion

        #region MONOBEHAVIOUR

        private async void OnEnable()
        {
            _valueTransform = _valueText.transform;
            await BeginAscend();
        }

        #endregion
        
        #region METHODS
        
        public void Initialize(int value)
        {
            _valueText.text = GameVar.DisplayValue(value).ToString();
            _valueText.color = _textStartingColour;
        }

        private async UniTask BeginAscend()
        {
            CancellationToken _ct = this.GetCancellationTokenOnDestroy();

            UniTask[] riseTasks = new UniTask[3];
            riseTasks[0] = _valueTransform.DOMoveY(GameVar.MergePointRiseDistance, GameVar.MergePointRiseDuration)
                .SetRelative().SetEase(_fadeCurve).WithCancellation(_ct);
            riseTasks[1] = _valueText.transform
                .DOScale(Vector2.one * GameVar.MergePointRiseScaleIncrease, GameVar.MergePointRiseDuration)
                .SetEase(_fadeCurve).WithCancellation(_ct);
            riseTasks[2] = _valueText.DOColor(_textEndColour, GameVar.MergePointRiseDuration)
                .SetEase(_fadeCurve).WithCancellation(_ct);

            await UniTask.WhenAll(riseTasks);

            Destroy(this.gameObject);
            
        }

        private void DestroySelf()
        {
            Destroy(this.gameObject);
        }
        
        
        #endregion
    }
}