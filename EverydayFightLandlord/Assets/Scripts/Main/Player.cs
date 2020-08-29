using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets;
using DG.Tweening;
using UnityEngine.UI;
using Assets.Scripts.Main;
using Assets.Scripts.Login;

public class Player : MonoBehaviour 
{
    public PlayerData info;
    //玩家扑克组
    public List<Poker> pokerList = new List<Poker>();
    //玩家已出,显示在桌面的扑克组
    public List<GameObject> outShowPokers = new List<GameObject>();
    //角色类别
    public RoleType roleType = RoleType.noRole;
    //玩家类别
    public PlayerType playerType;
    //操作类型
    public OperateType operateType;

	// Use this for initialization
	void Start () 
    {

	}

	// Update is called once per frame
	void Update () 
    {

	}

    /// <summary>
    /// 排序扑克
    /// </summary>
    public void PokerSort()
    {
        if (pokerList == null)
        {
            return;
        }
        PokerManage.PokerSort(pokerList);
    }


    /// <summary>
    /// 显示扑克
    /// </summary>
    /// <param name="isVisible">扑克是否可见,默认为false</param>
    public void ShowPoker(bool isVisible = false)
    {
        if (pokerList == null)
        {
            return;
        }
        for (int i = 0; i < pokerList.Count; i++)
        {
            if (isVisible)
            {
                //扑克设为可见
                pokerList[i].isVisible = isVisible;
            }
            //初始化完成(可以被点击)
            pokerList[i].isOk = true;
            //当前状态没有被选中
            pokerList[i].isSelect = false;
            pokerList[i].transform.localScale = Vector3.one;
            pokerList[i].transform.localPosition = OffsetPos(i);
        }
    }

    /// <summary>
    /// 显示玩家信息
    /// </summary>
    /// 
    public void ShowPlayerInfo()
    {
        transform.Find("UI/HeadImage").GetComponent<Image>().sprite = ResourcesManage.GetHeadImage(info.imageRef);
        transform.Find("UI/HeadImage/Name").GetComponent<Text>().text = info.name;
        transform.Find("UI/HeadImage/Name/Socre").GetComponent<Text>().text = info.score;
    }

    /// <summary>
    /// 出牌判断
    /// </summary>
    /// <returns></returns>
    public bool IFOutPoker()
    {
        if (PokerManage.waitPoker == null)
        {
            return false;
        }
        //表示当前是空等待,可以添加牌组(电脑牌组跳过)
        if (PokerManage.waitPoker.Count == 0)
        {
            //从手牌中确定是否有扑克选中
            for (int i = 0; i < pokerList.Count; i++)
            {
                if (pokerList[i].isSelect)
                {
                    //选中添加至等待扑克
                    PokerManage.waitPoker.Add(pokerList[i]);
                    pokerList[i].isSelect = false;
                }
            }
        }

        //等待扑克为空跳出,无选中
        if (PokerManage.waitPoker.Count <= 0)
        {
            return false;
        }

        //返回扑克最大值
        int value = 0;
        //判断是否符合出牌规则
        if (PokerRules.IsOutPokerRule(PokerManage.waitPoker, ref value))
        {
            //判断数目是否相同,或者扑克值比较大,怀疑是炸弹
            if (PokerManage.waitPoker.Count == PokerManage.lastPoker.Count || value >= 100)
            {
                //判断值是否比上一次牌值大
                if (value > PokerManage.maxPokerValue)
                {
                    OutPoker(value);
                    return true;
                }
            }
            //不相同判断是否为第一次出牌
            else if (PokerManage.maxPokerValue == -1)
            {
                OutPoker(value);
                return true;
            }
        }
        //当不符合出牌规则时清空等待扑克组
        PokerManage.waitPoker.Clear();
        //重新显示扑克
        ShowPoker();
        return false;
    }

    /// <summary>
    /// 出牌数据及显示操作
    /// </summary>
    /// <param name="_value"></param>
    void OutPoker(int _value)
    {
        //出牌前清除玩家上一轮显示
        ClearPlayerLastPoker();
        //清除上一轮最大出牌扑克组
        PokerManage.lastPoker.Clear();
        //设为当前轮牌值最大
        PokerManage.maxPokerValue = _value;
        //从已判断完毕的等待扑克组中遍历
        for (int i = 0; i < PokerManage.waitPoker.Count; i++)
        { 
            //删除等待扑克的显示(从玩家显示出的手牌里删除物体)
            Destroy(PokerManage.waitPoker[i].gameObject);

            //添加数据进上一轮(当前轮)最大扑克组
            PokerManage.lastPoker.Add(PokerManage.waitPoker[i]);

            //创建出牌显示信息模板
            GameObject go = Instantiate(ResourcesManage.dictionary["TemplatePokerShow"] as GameObject);

            //放置在玩家出牌显示中
            go.transform.SetParent(transform.Find("LastPoker"));
            go.transform.localScale = Vector3.one;

            //图为当前扑克
            go.GetComponent<Image>().sprite = PokerManage.waitPoker[i].info.spr;

            //添加进出牌显示扑克组中
            outShowPokers.Add(go);

            //从玩家手牌数据中删除
            pokerList.Remove(PokerManage.waitPoker[i]);
        }

        if (operateType != OperateType.Protagonist)
        {
            //非主角位显示剩余扑克
            transform.Find("UI/SurplusCount").GetComponent<Text>().text = "剩余: " + pokerList.Count + "张";
        }

        //清空等待扑克
        PokerManage.waitPoker.Clear();
    }

    /// <summary>
    /// 清除玩家自己上一轮的出牌显示
    /// </summary>
    public void ClearPlayerLastPoker()
    {
        //已出扑克显示中是否有物体
        if (outShowPokers != null && outShowPokers.Count != 0)
        {
            for (int i = 0; i < outShowPokers.Count; i++)
            {
                //删除其中物体
                Destroy(outShowPokers[i].gameObject);
            }
            //清空数据
            outShowPokers.Clear();
        }
    }

    /// <summary>
    /// 清除手牌数据及显示
    /// </summary>
    public void ClearHandPoker()
    {
        if (pokerList != null && pokerList.Count != 0)
        {
            for (int i = 0; i < pokerList.Count; i++)
            {
                Destroy(pokerList[i].gameObject);
            }
            pokerList.Clear();
        }
    }

    /// <summary>
    /// 偏移坐标
    /// </summary>
    /// <param name="_index">扑克下标</param>
    /// <returns></returns>
    public Vector3 OffsetPos(int _index)
    {
        //临时坐标
        Vector3 temp = Vector3.zero;

        //z轴方向为负,防止碰撞体重叠导致扑克无法选中或重复选中
        temp.z = -_index;

        switch (operateType)
        {
            case OperateType.Empty:
                break;
            case OperateType.Protagonist:
                //主视角位x轴,偏移位0.2
                temp.x = _index * 0.2f;
                break;
            case OperateType.Stranger:
                //边位是Y轴,偏移位0.3
                temp.y = -(_index * 0.3f);
                break;
            default:
                break;
        }
        return temp;
    }
}
