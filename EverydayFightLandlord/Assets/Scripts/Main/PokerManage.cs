using Assets.Scripts.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;



namespace Assets.Scripts.Main
{
    /// <summary>
    /// 扑克管理类
    /// </summary>
    [System.Serializable]
    static class PokerManage
    {
        //全部扑克信息
        public static List<PokerInfo> listAll = new List<PokerInfo>();
        //底牌扑克
        public static List<Poker> listBack = new List<Poker>();
        //等待扑克
        public static List<Poker> waitPoker = new List<Poker>();
        //上一次出牌扑克
        public static List<Poker> lastPoker = new List<Poker>();
        //当前最大牌值
        public static int maxPokerValue = -1;

        /// <summary>
        /// 加载所有卡牌信息
        /// </summary>
        public static void LoadPoker()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml((ResourcesManage.dictionary["Data"] as TextAsset).text);
            XmlNodeList list = doc.SelectSingleNode("root/poker").ChildNodes;
            foreach (XmlElement item in list)
            {
                PokerInfo poker = new PokerInfo();
                poker.id = int.Parse(item.GetAttribute("id"));
                poker.value = int.Parse(item.GetAttribute("value"));
                poker.spr = ResourcesManage.CreateSprite(item.GetAttribute("id"));
                poker.type = (PokerType)int.Parse(item.GetAttribute("se"));
                listAll.Add(poker);
            }
        }

        /// <summary>
        /// 排序扑克
        /// </summary>
        public static void PokerSort(List<Poker> _list)
        {
            if (_list == null)
            {
                return;
            }
            #region 冒泡排序
            ////冒泡排序
            //for (int i = 0; i < pokerList.Count; i++)
            //{
            //    bool isOk = true;
            //    for (int j = 0; j < pokerList.Count - 1 - i; j++)
            //    {
            //        if (pokerList[j + 1].info.value > pokerList[j].info.value)
            //        {
            //            isOk = false;
            //            Poker temp = pokerList[j + 1];
            //            pokerList[j + 1] = pokerList[j];
            //            pokerList[j] = temp;
            //        }
            //    }
            //    if (isOk)
            //    {
            //        break;
            //    }
            //}
            #endregion
            ////排列大小  返回值小于0表示a小于b  值大于0 a大于b  值等于0 a等于b
            _list.Sort((a, b) => b.info.value < a.info.value ? -1 : 1);
            //排列花色s
            _list.Sort((a, b) =>
            {
                if (a.info.value == b.info.value)
                {
                    return (int)a.info.type < (int)b.info.type ? -1 : 1;
                }
                return -1;
            });
        }
    }
}
