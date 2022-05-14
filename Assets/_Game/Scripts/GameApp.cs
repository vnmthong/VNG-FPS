using PYDFramework;
using PYDFramework.MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VNGFPS
{
    public class GameApp : AppBase
    {
        public ModelManager models => Singleton<ModelManager>.instance;
        public ConfigManager configs => Singleton<ConfigManager>.instance;
        public ResourceManager resourceManager => Singleton<ResourceManager>.instance;
        public AudioManager audioManager => Singleton<AudioManager>.instance;

        public override void OnInit()
        {
            Application.targetFrameRate = 60;

            Singleton<GameApp>.Set(this);
            Singleton<ConfigManager>.Set(new ConfigManager());
            Singleton<ModelManager>.Set(new ModelManager());

            configs.Init();
            models.Init();
        }
    }
}
