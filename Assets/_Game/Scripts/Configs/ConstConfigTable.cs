using PYDFramework;
using PYDFramework.Config;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace VNGFPS
{
    public class ConstConfig : IConfigItem
    {
        public string key;
        public string value;

        public string GetId()
        {
            return key;
        }

        public void OnReadImpl(IConfigReader reader)
        {
            key = reader.ReadString();
            value = reader.ReadString();
        }

        public int IntVal()
        {
            int.TryParse(value, out int val);
            return val;
        }

        public float FloatVal()
        {
            float.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out float val);
            return val;
        }

        public bool BoolVal()
        {
            bool.TryParse(value, out bool val);
            return val;
        }
    }

    public class ConstConfigTable : Configs<ConstConfig>
    {
        public override TextAsset Asset => Singleton<GameRunner>.instance.constCfg;
    }
}
