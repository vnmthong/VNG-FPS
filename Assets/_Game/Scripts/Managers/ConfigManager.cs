using PYDFramework.Config;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VNGFPS
{
    public class ConfigManager : ConfigManagerBase
    {
        //Configs
        public ConstConfigTable constConfigs;
        public TroopConfigTable troopConfigs;
        public LevelConfigTable levelConfigs;

        public void Init()
        {
            //Register
            Register(out constConfigs);
            Register(out troopConfigs);
            Register(out levelConfigs);

            LoadConfigs();
        }
    }
}
