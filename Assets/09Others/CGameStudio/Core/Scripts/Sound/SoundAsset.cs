/**
 * Copyright (c) 2021-present Compactive Game Studio. All rights reserved.
 * 'CGameStudio' can not be copied and/or distributed without the express permission of Compactive Game Studio.
 */

using UnityEngine;
using UnityEngine.Audio;

namespace CGameStudio
{
    [System.Serializable]
    public class SoundAsset
    {
        [Header("Sound")]
        public AudioMixerGroup audioMixerGroup;
        private AudioSource source;
        public AudioClip clip;
        public string name;

        [Header("Preferences")]
        [Range(0f, 1f)] public float volume = 1f;
        [Range(0f, 3f)] public float pitch = 1f;
        public bool loop = false;
        public bool mute = false;
        public bool playOnAwake = false;

        public void SetSource(AudioSource _source)
        {
            source = _source;
            source.clip = clip;
            source.pitch = pitch;
            source.volume = volume;
            source.playOnAwake = playOnAwake;
            source.loop = loop;
            source.mute = mute;
            source.outputAudioMixerGroup = audioMixerGroup;
        }

        public void Mute(bool mute)
        {
            source.mute = mute;
        }

        public void Play()
        {
            source.Play();
        }

        public void Stop()
        {
            source.Stop();
        }
    }
}