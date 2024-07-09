// Onur Ereren - July 2024
// Popcore case

// Monobehaviour part of audio system.
// It feeds the references and SFX to the static AudioPlayer class

using System.Collections.Generic;
using UnityEngine;

namespace PopsBubble
{
    public class AudioController : MonoBehaviour
    {
        #region REFERENCES
        
        private AudioSource _source;
        private Dictionary<string, AudioClip[]> _clips;

        #endregion
        
        #region VARIABLES
        
        [SerializeField] private AudioClip[] _pop;
        [SerializeField] private AudioClip[] _popSeries;

        [SerializeField] private AudioClip[] _swoosh;
        [SerializeField] private AudioClip[] _ping;
        [SerializeField] private AudioClip[] _jingle;
        
        #endregion
        
        #region MONOBEHAVIOUR
        
        private void Start()
        {
            _source = GetComponent<AudioSource>();
            GenerateLibrary();
            AudioPlayer.Initialize(_source, _clips);
        }

        #endregion
        
        #region METHODS
        
        private void GenerateLibrary()
        {
            _clips = new Dictionary<string, AudioClip[]>();
            _clips.Add("pop", _pop);
            _clips.Add("popSeries", _popSeries);
            _clips.Add("swoosh", _swoosh);
            _clips.Add("ping", _ping);
            _clips.Add("jingle", _jingle);
        }
        
        #endregion
    }
}
