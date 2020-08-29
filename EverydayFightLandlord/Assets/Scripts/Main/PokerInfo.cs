using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Main
{
    [System.Serializable]
    public class PokerInfo
    {
        //id号,加载图片用
        public int id;
        //牌值
        public int value;
        //扑克图
        public Sprite spr;
        //扑克花色
        public PokerType type;

        public PokerInfo Clone()
        {
            PokerInfo info = new PokerInfo();
            info.id = id;
            info.value = value;
            info.spr = spr;
            info.type = type;
            return info;
        }
    }
}
