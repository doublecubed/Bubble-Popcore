// Onur Ereren - July 2024
// Popcore case

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PopsBubble
{
    public static class AudioPlayer
    {
        private static AudioSource _source;
        
        private static Dictionary<string, AudioClip[]> _clips;

        public static void Initialize(AudioSource source, Dictionary<string, AudioClip[]> clips)
        {
            _source = source;
            _clips = clips;
        }

        public static void PlayAudio(string action)
        {
            if (_clips.ContainsKey(action))
            {
                int index = Random.Range(0, _clips[action].Length);
                
                _source.PlayOneShot(_clips[action][index]);
                //_source.PlayOneShot(_clips[action][0]);
            }
        }
    }
}
