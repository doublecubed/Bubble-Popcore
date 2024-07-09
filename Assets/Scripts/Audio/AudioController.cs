// Onur Ereren - July 2024
// Popcore case

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PopsBubble
{
    public class AudioController : MonoBehaviour
    {
        private AudioSource _source;
        private Dictionary<string, AudioClip[]> _clips;

        [SerializeField] private AudioClip[] _pop;
        [SerializeField] private AudioClip[] _popSeries;

        [SerializeField] private AudioClip[] _swoosh;
        [SerializeField] private AudioClip[] _ping;
        [SerializeField] private AudioClip[] _jingle;
        
        private void Start()
        {
            _source = GetComponent<AudioSource>();
            GenerateLibrary();
            AudioPlayer.Initialize(_source, _clips);
        }

        private void GenerateLibrary()
        {
            _clips = new Dictionary<string, AudioClip[]>();
            _clips.Add("pop", _pop);
            _clips.Add("popSeries", _popSeries);
            _clips.Add("swoosh", _swoosh);
            _clips.Add("ping", _ping);
            _clips.Add("jingle", _jingle);
        }
    }
}
