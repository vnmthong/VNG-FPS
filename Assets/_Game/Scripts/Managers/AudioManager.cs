using PYDFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VNGFPS
{
    public class AudioManager : AudioManagerBase<AudioType>
    {
        private AudioType _currentMusicKey;
        private GameApp _app => Singleton<GameApp>.instance;
        private void Awake()
        {
            Singleton<AudioManager>.Set(this);
        }

        private void OnDestroy()
        {
            Singleton<AudioManager>.Unset(this);
        }

        private void Start()
        {
            Init();
        }

        public void Init()
        {
            //Demo
            //RegisterAudio(AudioType.Background, new AudioConfig("Audio/Background", 1f));
            //RegisterAudio(AudioType.Button, new AudioConfig("Audio/Click", 0.8f));
        }

        public override void PlayMusic(AudioType type)
        {
            _currentMusicKey = type;
            if (!_app.models.settingModel.isMusic)
                return;

            base.PlayMusic(type);
        }

        public override void PlayEffect(AudioType type)
        {
            if (!_app.models.settingModel.isMusic)
                return;

            base.PlayEffect(type);
        }

        public void SetEnableMusic(bool isEnable)
        {
            if (isEnable)
                PlayMusic(_currentMusicKey);
            else
                StopMusic();
        }
    }
}
