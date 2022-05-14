using Newtonsoft.Json;
using PYDFramework.MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VNGFPS
{
    public class TroopGameModel : Model<GameApp>
    {
        public static EventTypeBase dataChangedEvent = new EventTypeBase(nameof(TroopGameModel) + ".dataChanged");

        public TroopGameModel(string id, int level, int slotId, int maxHp, TeamType teamType) : base(dataChangedEvent)
        {
            _id = id;
            _level = level;
            _slotId = slotId;
            _maxHp = maxHp;
            _hp = maxHp;
            _team = teamType;
        }

        [JsonProperty] private string _id;
        [JsonProperty] private int _level;
        [JsonProperty] private int _maxHp;
        [JsonProperty] private int _hp;
        [JsonProperty] private int _slotId;
        [JsonProperty] private TeamType _team;
        public TeamType enermyTeam => team == TeamType.Hero ? TeamType.Devil : TeamType.Hero;

        public TeamType team 
        {
            get => _team; 
            set
            {
                if (_team == value)
                    return;

                _team = value;
                RaiseDataChanged(nameof(team));
            }
        }
        public string Id
        {
            get => _id;
            set
            {
                if (_id == value)
                    return;

                _id = value;
                RaiseDataChanged(nameof(Id));
            }
        }

        public int Level
        {
            get => _level;
            set
            {
                if (_level == value)
                    return;

                _level = value;
                RaiseDataChanged(nameof(Level));
            }
        }

        public int MaxHp
        {
            get => _maxHp;
        }

        public int Hp
        {
            get => _hp;
            set
            {
                if (_hp == value)
                    return;

                _hp = value;
                RaiseDataChanged(nameof(Hp));
            }
        }

        public int SlotId
        {
            get => _slotId;
            set
            {
                if (_slotId == value)
                    return;

                _slotId = value;
                RaiseDataChanged(nameof(SlotId));
            }
        }

        public void ResetData(TroopConfig.TroopStats troopStats)
        {
            _maxHp = troopStats.hp;
            Hp = MaxHp;
        }

        public int GetGoldPerHp(int gold)
        {
            return MaxHp / gold;
        }
    }
}
