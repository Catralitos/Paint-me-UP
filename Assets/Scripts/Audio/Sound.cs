﻿using UnityEngine;

namespace Audio
{
    [System.Serializable]
    public class Sound
    {
        public string name;

        public AudioClip clip;

        [Range(0f, 1f)]
        public float volume;

        [Range(0.0f, 1.0f)]
        public float spatialBlend;

        [Range(.1f, 3f)]
        public float pitch;

        public bool loop;

        public bool fadeIn;

        //public float maxDist = 24;

        [HideInInspector]
        public AudioSource source;

    }
}