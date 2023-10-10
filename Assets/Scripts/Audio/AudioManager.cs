﻿using System;
using System.Collections;
using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        public Sound[] sounds;

        
        private void Start()
        {
            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;

                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
                s.source.spatialBlend = s.spatialBlend;
                s.source.playOnAwake = false;
            }
        }
        
        public void Play(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s != null && !s.source.isPlaying)
            {
                if (s.fadeIn)
                {
                    StartCoroutine(StartFade(s.source, 5f, 0.4f));
                }
                s.source.Play();
            }
        }

        public void Play(Sound s)
        {
            if (s != null && !s.source.isPlaying)
            {
                if (s.fadeIn)
                {
                    StartCoroutine(StartFade(s.source, 5f, 1));
                }
                s.source.Play();
            }
        }

        public void ChangeVolume(string name, float newVolume)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s != null) s.source.volume = newVolume;
        }

        public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
        {
            float currentTime = 0;
            float start = audioSource.volume;

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
                yield return null;
            }
            yield break;
        }

        public void Stop(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            s?.source.Stop();
        }

        public void Stop(Sound s)
        {
            s?.source.Stop();
        }

        public void StopAll()
        {
            foreach (Sound s in sounds)
            {
                s?.source.Stop();
            }
        }
    }
}