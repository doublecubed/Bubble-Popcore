// Onur Ereren - July 2024
// Popcore case

// Static part of the audio system. Tasked with playing the audio clips.
// If there are multiple clips appointed for a keyword, it randomly selects a clip.

using System.Collections.Generic;
using UnityEngine;

namespace PopsBubble
{
    public static class AudioPlayer
    {
        #region REFERENCES
        
        private static AudioSource _source;
        
        #endregion
        
        #region VARIABLES
        
        private static Dictionary<string, AudioClip[]> _clips;

        #endregion
        
        #region INITIALIZATION
        
        public static void Initialize(AudioSource source, Dictionary<string, AudioClip[]> clips)
        {
            _source = source;
            _clips = clips;
        }

        #endregion
        
        #region METHODS
        
        public static void PlayAudio(string action)
        {
            if (_clips.ContainsKey(action))
            {
                int index = Random.Range(0, _clips[action].Length);
                
                _source.PlayOneShot(_clips[action][index]);
            }
        }
        
        #endregion
    }
}
