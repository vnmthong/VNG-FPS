using PYDFramework;
using PYDFramework.Config;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VNGFPS
{
    public class TroopConfig : IConfigItem
    {
        public string id;
        public TroopStats[] troopStats;
        public class TroopStats
        {
            public string id;
            public int level;
            public string name;
            public string group;
            public int hp;
            public int atk;
            public int moveSpeed;
            public float atkSpeed;
            public int range;
            public int star;
            public float power;
        }

        public string GetId()
        {
            return id.ToString();
        }

        public void OnReadImpl(IConfigReader reader)
        {
            id = reader.ReadString();

            var levels = reader.ReadIntArr();
            var names = reader.ReadStringArr();
            var groups = reader.ReadStringArr();
            var hps = reader.ReadIntArr();
            var atks = reader.ReadIntArr();
            var speeds = reader.ReadIntArr();
            var atkSpeeds = reader.ReadFloatArr();
            var ranges = reader.ReadIntArr();
            var stars = reader.ReadIntArr();
            var powers = reader.ReadFloatArr();

            troopStats = new TroopStats[levels.Length];
            for(int i = 0; i< troopStats.Length;i++)
            {
                troopStats[i] = new TroopStats()
                {
                    id = id,
                    level = levels[i],
                    name = names[i],
                    group = groups[i],
                    hp = hps[i],
                    atk = atks[i],
                    moveSpeed = speeds[i],
                    atkSpeed = atkSpeeds[i],
                    range = ranges[i],
                    star = stars[i],
                    power = powers[i]
                };
            }    
        }

        public TroopStats GetConfigLevel(int level)
        {
            return troopStats.FirstOrDefault(e => e.level == level);
        }
    }

    public class TroopConfigTable : Configs<TroopConfig>
    {
        public override TextAsset Asset => Singleton<GameRunner>.instance.troopCfg;

        public TroopConfig.TroopStats GetConfigStat(string id, int level)
        {
            var cfg = GetConfig(id);
            return cfg.GetConfigLevel(level);
        }

        public TroopConfig.TroopStats GetConfigStat(string id)
        {
            var cfgs = itemList.SelectMany(e => e.troopStats).ToList();
            return cfgs.FirstOrDefault(e => id == $"{e.id}{e.level}");
        }

        public int GetMaxLevel(string troopId)
        {
            var cfg = GetConfig(troopId);
            return cfg.troopStats.Max(e => e.level);
        }
    }
}
