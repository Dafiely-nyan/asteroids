using System;
using UnityEngine;

namespace AudioManagment
{
    [Serializable]
    public class AudioTrack
    {
        public AudioClip AudioClip;
        public float Volume = 1;
        public float Pitch = 1;
        public bool Loop;
        public SoundType SoundType;
    }
}