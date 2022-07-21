using System.Collections.Generic;
using UnityEngine;

namespace AudioManagment
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField]
        private List<AudioTrack> AudioTracks = new List<AudioTrack>();
        
        private readonly Dictionary<SoundType, AudioSource> _sounds = new Dictionary<SoundType, AudioSource>();

        private void Awake()
        {
            foreach (AudioTrack audioTrack in AudioTracks)
            {
                AudioSource audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.clip = audioTrack.AudioClip;
                audioSource.loop = audioTrack.Loop;
                audioSource.pitch = audioTrack.Pitch;
                audioSource.volume = audioTrack.Volume;
                
                _sounds.Add(audioTrack.SoundType, audioSource);
            }
        }

        public void Play(SoundType soundType)
        {
            if (_sounds.ContainsKey(soundType))
            {
                _sounds[soundType].Play();
            }
        }

        public void Stop(SoundType soundType)
        {
            if (_sounds.ContainsKey(soundType))
            {
                _sounds[soundType].Stop();
            }
        }
    }
}