using PYDFramework;
using PYDFramework.Config;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VNGFPS
{
    public class LevelData
    {
        public string id;
        public int slotId;
    }

    public class LevelConfig : IConfigItem
    {
        public int level;
        public int winReward;
        public int winBonus;
        public int offline;
        public int randomReward;
        public string boosterData;
        public LevelData[] levelDatas;

        public string GetId()
        {
            return level.ToString();
        }

        public void OnReadImpl(IConfigReader reader)
        {
            level = reader.ReadInt();
            winReward = reader.ReadInt();
            winBonus = reader.ReadInt();
            offline = reader.ReadInt();
            randomReward = reader.ReadInt();
            boosterData = reader.ReadString();

            var levelStr = reader.ReadString();
            var stages = levelStr.Split('_');

            levelDatas = new LevelData[stages.Length];

            for (int i = 0; i < levelDatas.Length; i++)
            {
                var stage = stages[i];
                var infos = stage.Split('|');
                var id = infos[0];
                var slotId = int.Parse(infos[1]);

                levelDatas[i] = new LevelData()
                {
                    id = id,
                    slotId = slotId
                };
            }

        }
    }

    public class LevelConfigTable : Configs<LevelConfig>
    {
        public override TextAsset Asset => Singleton<GameRunner>.instance.levelCfg;

        public LevelConfig GetConfig(int id)
        {
            return GetConfig(id.ToString());
        }
    }
}
