// Onur Ereren - July 2024
// Popcore case

// Controls UI functions.

using Lofelt.NiceVibrations;
using UnityEngine;

namespace PopsBubble
{
    public class UIController : MonoBehaviour
    {
        #region REFERENCES

        private HexGrid _grid;

        #endregion

        #region MONOBEHAVIOUR

        private void Awake()
        {
            // Set Screensize for Windows build
            #if UNITY_STANDALONE_WIN
                        Screen.SetResolution(562, 1000, false);
                        Screen.fullScreen = false;
            #endif
        }

        private void Start()
        {
            _grid = DependencyContainer.Grid;
        }

        #endregion
        
        #region METHODS
        
        #region Button Methods

        public async void ScrambleButton()
        {
            PlayButtonFeedback();
            await _grid.ScrambleGrid();
        }

        public async void PushGridUp()
        {
            PlayButtonFeedback();
            await _grid.MoveGridUp();
        }
        
        #endregion

        #region Feedback

        private void PlayButtonFeedback()
        {
            AudioPlayer.PlayAudio("ping");
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.Selection);
        }
        
        #endregion
        
        #endregion
        
    }
}
