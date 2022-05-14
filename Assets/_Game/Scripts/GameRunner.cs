using PYDFramework;
using PYDFramework.MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VNGFPS
{
    public class GameRunner : AppRunner<GameApp>
    {
        private GameApp myGame;
        public TextAsset constCfg;
        protected override GameApp CreateApp()
        {
            Singleton<GameRunner>.Set(this);
            myGame = new GameApp();
            return myGame;
        }

        public GameApp GetApp()
        {
            return myGame;
        }

        private void OnDestroy()
        {
            Singleton<GameRunner>.Unset(this);
        }
    }
}