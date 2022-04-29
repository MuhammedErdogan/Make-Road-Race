/**
 * Copyright (c) 2021-present Compactive Game Studio. All rights reserved.
 * 'CGameStudio' can not be copied and/or distributed without the express permission of Compactive Game Studio.
 */

using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace CGameStudio
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance = null;

        [SerializeField] Hashtable soundHash = new Hashtable();

        [SerializeField] AudioMixer audioMixer;
        [SerializeField] SoundAsset[] sounds;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            foreach (SoundAsset item in sounds)
            {
                soundHash.Add(item.name, item);
            }

            for (int i = 0; i < sounds.Length; i++)
            {
                GameObject sound = new GameObject("Sound" + i + sounds[i].name);
                sound.transform.SetParent(this.transform);
                sounds[i].SetSource(sound.AddComponent<AudioSource>());
            }
        }

        public void Play(string soundName)
        {
            SoundAsset sound = (SoundAsset)soundHash[soundName];
            sound.Play();
        }

        public void Mute(string soundName, bool value)
        {
            SoundAsset sound = (SoundAsset)soundHash[soundName];
            sound.Mute(value);
        }

        public void SetSoundVolume(string soundName, float volume)
        {
            SoundAsset sound = (SoundAsset)soundHash[soundName];
            sound.volume = volume;
        }

        public void SetMusicVolume(float musicLvl)
        {
            audioMixer.SetFloat("MusicVolume", musicLvl);
        }

        public void SetSFXVolume(float sfxLvl)
        {
            audioMixer.SetFloat("SFXVolume", sfxLvl);
        }

        public void Stop(string soundName)
        {
            SoundAsset sound = (SoundAsset)soundHash[soundName];
            sound.Stop();
        }
    }
}