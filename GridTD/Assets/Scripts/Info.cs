using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    [System.Serializable]
    public class TurretInfo
    {
        public string name;
        public string desc;
        public int lv;
        public int attack;
        public float attackSpeed;
        public float money;
        public TurretType type;
        public TurretInfo()
        {
            lv = 1;
        }
        public void UpLv()
        {
            lv++;
            money = money * 2;
            attack = lv * 5;
        }
    }
    [System.Serializable]
    public enum GridType
    {
        /// <summary>
        /// 空格子
        /// </summary>
        NullGrid,
        /// <summary>
        /// 道路
        /// </summary>
        Road,
        /// <summary>
        /// 障碍物
        /// </summary>
        Barrier,
        /// <summary>
        /// 炮塔
        /// </summary>
        Turret,
    }
    [System.Serializable]
    public enum TurretType
    {
        /// <summary>
        /// 单发
        /// </summary>
        singleShot,
        /// <summary>
        /// 多发
        /// </summary>
        doubleShot,
    }

    [System.Serializable]
    public class MonsterInfo
    {
        public string name;
        public int hp;
        public float speed;

        public MonsterInfo()
        {

        }
    }
}
