using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PYDFramework
{
    public abstract class AudioManagerBase<T>: MonoBehaviour
    {
        public AudioSource sourceEffect;
        public AudioSource sourceMusic;

        private Dictionary<T, AudioConfig> _resourcePathDic = new Dictionary<T, AudioConfig>();

        protected bool RegisterAudio(T type, AudioConfig audioConfig)
        {
            if (HasAudio(type))
            {
                Debug.LogErrorFormat("Popup {0} is dupplicate", type);
                return false;
            }
            _resourcePathDic.Add(type, audioConfig);
            return true;
        }

        public virtual void PlayEffect(T type)
        {
            if (!_resourcePathDic.TryGetValue(type, out AudioConfig audioConfig))
            {
                Debug.LogErrorFormat("Audio {0} is not found", type);
                return;
            }

            var audioPrefabs = Resources.Load<AudioClip>(audioConfig.path);
            sourceEffect.PlayOneShot(audioPrefabs, audioConfig.volume);
        }

        public virtual void PlayMusic(T type)
        {
            if (!_resourcePathDic.TryGetValue(type, out AudioConfig audioConfig))
            {
                Debug.LogErrorFormat("Audio {0} is not found", type);
                return;
            }

            var audioPrefabs = Resources.Load<AudioClip>(audioConfig.path);
            sourceMusic.clip = audioPrefabs;
            sourceMusic.volume = audioConfig.volume;
            sourceMusic.Play();
        }

        public virtual void StopMusic()
        {
            sourceMusic.Stop();
        }

        private bool HasAudio(T type)
        {
            if (_resourcePathDic.ContainsKey(type))
                return true;
            return false;
        }
    }
}
