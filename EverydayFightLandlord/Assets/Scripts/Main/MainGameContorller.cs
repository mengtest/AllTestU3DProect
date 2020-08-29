using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using Assets.Scripts.Main;
using Assets.Scripts.Login;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MainGameContorller : MonoBehaviour
{
    //玩家
    public Player[] player;
    public GameObject Btn_Start;
    public GameObject Btn_ReStart;
    public GameObject View_GrabLandLord;
    public GameObject View_OutAndPassPoker;
    public GameObject View_EnterPlayerInfo;
    public GameObject View_GameOverView;
    public PlayerDataManage playerDataManage;
    //游戏状态
    public GameState state = GameState.Empty;
    //当前操作玩家下标
    int curPlayer = -1;

    //时钟
    Transform timeCanvas;
    //时钟显示
    Text ShowTime;
    //当前轮牌最大玩家下标
    int maxPlayerIndex = -1;
    //等待时间
    float waitTime = 10f;
    //计数时间
    float time = 0;
    //底分
    int baseScore = 2;
    //倍数
    int MultipleCount = 2;

    /// <summary>游戏状态枚举
    /// </summary>
    public enum GameState
    {
        /// <summary>
        /// 空状态
        /// </summary>
        Empty,
        /// <summary>
        /// 抢地主
        /// </summary>
        GrabLandLord,
        /// <summary>
        /// 游戏开始
        /// </summary>
        GameStart,
        /// <summary>
        /// 出牌
        /// </summary>
        OutPoker,
        /// <summary>
        /// 游戏结束
        /// </summary>
        GameOver
    }
    void Start()
    {
        SceneManager.LoadSceneAsync("GameUI", LoadSceneMode.Additive);
        timeCanvas = GameObject.Find("TimeCanvas").transform;
        ShowTime = timeCanvas.Find("TimeImage/TimeText").GetComponent<Text>();
        playerDataManage = PlayerDataManage.GetInstance();
        //播放背景音乐
        AudioSound.CreateSoundPlay("music", true, true);

        //初始化游戏
        InitializeGame();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ClickBtnOutPokerEvent();
        }
        switch (state)
        {
            case GameState.Empty:
                break;
            case GameState.GameStart:
                //游戏开始发牌(协程动画)
                StartCoroutine(DealPoker());
                //开启发牌协程后为空等待
                state = GameState.Empty;
                break;
            ///抢地主阶段
            case GameState.GrabLandLord:
                timeCanvas.GetComponent<Canvas>().enabled = true;
                ShowTime.text = (int)(waitTime - time + 1) + "";
                //当前(上一把)没有人是赢家
                if (curPlayer == -1)
                {
                    //随机玩家
                    //curPlayer = Random.Range(0, player.Length);
                    curPlayer = 0;
                }
                timeCanvas.SetParent(player[curPlayer].transform.Find("Time"));
                time += Time.deltaTime;
                //电脑随机选择抢不抢地主
                if (player[curPlayer].playerType != PlayerType.player)
                {
                    if (time >= 1)
                    {
                        //有几率选择抢地主
                        if (Random.Range(0, 3) < 1.5f)
                        {
                            ClickBtnIsGrabEvent();
                        }
                        else
                        {
                            ClickBtnNoGrabEvent();
                        }
                        time = 0;
                    }
                }
                else
                {
                    //玩家抢地主按钮控制
                    View_GrabLandLord.SetActive(true);
                }
                //超过等待时间自动不抢
                if (time >= waitTime)
                {
                    ClickBtnNoGrabEvent();
                }
                break;

            ///出牌阶段
            case GameState.OutPoker:
                timeCanvas.SetParent(player[curPlayer].transform.Find("Time"));
                //如果当前玩家是最大
                if (maxPlayerIndex == curPlayer)
                {
                    //牌值为空,为准备下一次随意出牌
                    PokerManage.maxPokerValue = -1;
                    //牌型为空,为准备下一次随意出牌
                    PokerRules.SelectPokerAct = null;
                    //上次轮最大牌为空
                    PokerManage.lastPoker.Clear();
                    ShowTime.text = "oo";
                }
                else
                {
                    ShowTime.text = (int)(waitTime - time + 1) + "";
                }

                time += Time.deltaTime;
                //如果玩家是电脑
                if (player[curPlayer].playerType != PlayerType.player)
                {
                    //等待时间
                    if (time > 0.5f)
                    {
                        //如果是同一家不出牌
                        if (player[maxPlayerIndex].roleType == player[curPlayer].roleType && curPlayer != maxPlayerIndex)
                        {
                            ClickBtnPassPokerEvent();
                        }
                        //电脑找牌
                        else if (PokerRules.SelectPoker(player[curPlayer].pokerList, PokerManage.maxPokerValue, PokerManage.lastPoker))
                        {
                            ClickBtnOutPokerEvent();
                        }
                        else
	                    {
                            ClickBtnPassPokerEvent();
	                    }
                        time = 0;
                    }
                }
                else
                {
                    //玩家准备出牌,显示出牌按钮
                    View_OutAndPassPoker.SetActive(true);
                }
                //等待时间自动过牌 并且 当前最大没有时间限制
                if (time >= waitTime && maxPlayerIndex != curPlayer)
                {
                    //等待时间内未出牌时,过牌
                    ClickBtnPassPokerEvent();
                }
                break;
            case GameState.GameOver:
                GameOver();
                break;
            default:
                break;
        }
    }

    /// <summary>发牌协程
    /// </summary>
    IEnumerator DealPoker()
    {
        //记录已出现牌
        bool[] ifPoker = new bool[54];

        //玩家下标
        int playerIndex = 0;

        //随机卡牌下标
        int pokerIndex = Random.Range(0, ifPoker.Length);

        //发完数
        int isCount = 0;

        //未发完时至底牌时循环
        while (isCount < ifPoker.Length - 3)
        {
            //是否已出现
            if (ifPoker[pokerIndex])
            {
                //出现时重新随机
                pokerIndex = Random.Range(0, ifPoker.Length);
            }
            else
            {
                //创建一张模板扑克
                GameObject go = Instantiate(ResourcesManage.dictionary["TemplatePoker"] as GameObject);
                Poker poker = go.GetComponent<Poker>();

                //添加信息(克隆函数防止更改实际扑克信息)
                poker.info = PokerManage.listAll[pokerIndex].Clone();
                //测试显示所有牌
                //poker.isVisible = true;

                //主视角设为可见可选中
                if (player[playerIndex].operateType == OperateType.Protagonist)
                {
                    poker.isVisible = true;
                    go.AddComponent<BoxCollider2D>().size = new Vector2(1.05f, 1.5f);
                }

                //添加到玩家中,设为玩家手牌子物体
                player[playerIndex].pokerList.Add(poker);
                go.transform.SetParent(player[playerIndex].transform.Find("HandPoker"));

                //DoTween动画,朝自己位置移动
                Vector3 ver = player[playerIndex].OffsetPos(player[playerIndex].pokerList.Count - 1);
                go.transform.DOLocalMove(ver, 0.5f);

                //卡牌设为已出现
                ifPoker[pokerIndex] = true;

                //发完一张
                isCount++;

                //下一个玩家
                playerIndex = (++playerIndex) % 3;

                //播放音效
                AudioSound.CreateSoundPlay("fapai");
                yield return new WaitForSeconds(0.05f);
            }
        }
        //遍历已出现牌组,找寻未出现扑克
        for (int i = 0; i < ifPoker.Length; i++)
        {
            if (!ifPoker[i])
            {
                //创建一张模板扑克
                GameObject go = Instantiate(ResourcesManage.dictionary["TemplatePoker"] as GameObject);
                //添加信息
                go.GetComponent<Poker>().info = PokerManage.listAll[i].Clone();
                //添加进底牌组
                PokerManage.listBack.Add(go.GetComponent<Poker>());
            }
        }
        //发牌完毕等待一秒
        yield return new WaitForSeconds(1f);

        //初始化显示底牌
        InsShowBackPoker();

        //显示玩家手牌
        for (int i = 0; i < player.Length; i++)
        {
            player[i].PokerSort();
            player[i].ShowPoker();
        }

        //发牌完毕为抢地主状态(只有当前状态为空才会进入抢地主,防止重复发牌时,重复进入抢地主状态)
        if (state == GameState.Empty)
        {
            state = GameState.GrabLandLord;
        }

        //当前玩家最大 开启 抢地主按钮
        View_GrabLandLord.SetActive(maxPlayerIndex == -1);
    }

    /// <summary>初始化游戏物体
    /// </summary>
    public void InitializeGame()
    {
        //按钮委托
        GameObject.Find("Btn_Close").GetComponent<Button>().onClick.AddListener(() => { PlayBtnSound(); ClickBtnCloseEnvet(); });
        GameObject.Find("Btn_Setting").GetComponent<Button>().onClick.AddListener(() => { PlayBtnSound(); ClickBtnSettingEvent(); });
        GameObject.Find("Btn_OutPoker").GetComponent<Button>().onClick.AddListener(() => { PlayBtnSound(); ClickBtnOutPokerEvent(); });
        GameObject.Find("Btn_PassPoker").GetComponent<Button>().onClick.AddListener(() => { PlayBtnSound(); ClickBtnPassPokerEvent(); });
        GameObject.Find("Btn_TripPoker").GetComponent<Button>().onClick.AddListener(() => { PlayBtnSound(); ClickBtnTripPokerEnvet(); });
        GameObject.Find("Btn_IsGrab").GetComponent<Button>().onClick.AddListener(() => { PlayBtnSound(); ClickBtnIsGrabEvent(); });
        GameObject.Find("Btn_NoGrab").GetComponent<Button>().onClick.AddListener(() => { PlayBtnSound(); ClickBtnNoGrabEvent(); });
        GameObject.Find("Btn_ReGameStart").GetComponent<Button>().onClick.AddListener(() => { PlayBtnSound(); ClickBtnReGameStartEvent(); });
        GameObject.Find("Btn_GameStart").GetComponent<Button>().onClick.AddListener(() => { PlayBtnSound(); ClickBtnGameStartEvent(); });
        GameObject.Find("Btn_SetPlayerInfo").GetComponent<Button>().onClick.AddListener(() => { PlayBtnSound(); ClickBtnSetPlayerInfoEvent(); });
        GameObject.Find("Left_HeadImage").GetComponent<Button>().onClick.AddListener(() => { PlayBtnSound(); ClickBtnLeftHeadImageEvent(); });
        GameObject.Find("Right_HeadImage").GetComponent<Button>().onClick.AddListener(() => { PlayBtnSound(); ClickBtnRightHeadImageEvent(); });
        GameObject.Find("Btn_Test_DealPoker").GetComponent<Button>().onClick.AddListener(() => { PlayBtnSound(); ClickBtnDealPokerEvent(); });
        GameObject.Find("Btn_Test_GameOver").GetComponent<Button>().onClick.AddListener(() =>
        {
            PlayBtnSound();
            //出牌按钮隐藏
            View_OutAndPassPoker.SetActive(false);
            //设为当前最大
            maxPlayerIndex = curPlayer;
            //游戏结束
            state = GameState.GameOver;
        });
        Btn_Start.SetActive(false);
        View_GrabLandLord.SetActive(false);
        View_OutAndPassPoker.SetActive(false);
        Btn_ReStart.SetActive(false);
        View_EnterPlayerInfo.SetActive(false);
        View_GameOverView.SetActive(false);

        //当前玩家信息为空,让玩家设置信息,否则显示开始按钮,设置完成设游戏为抢地主状态
        if (playerDataManage.curPlayer != null && playerDataManage.curPlayer.name == "")
        {
            View_EnterPlayerInfo.SetActive(true);
        }
        else
        {
            Btn_Start.SetActive(true);
            InitializePlayersInfo();
        }
    }

    /// <summary>点击按钮时的播放音效
    /// </summary>
    public void PlayBtnSound()
    {
        AudioSound.CreateSoundPlay("button");
    }

    /// <summary>初始化玩家信息及电脑信息的显示
    /// </summary>
    public void InitializePlayersInfo()
    {
        //显示玩家信息及随机电脑信息
        for (int i = 0; i < player.Length; i++)
        {
            //主视角位选择当前角色
            if (player[i].operateType == OperateType.Protagonist)
            {
                player[i].info = new PlayerData(playerDataManage.curPlayer);
            }
            else
            {
                //边位随机选择角色
                int index = Random.Range(0, playerDataManage.list.Count);
                player[i].info = new PlayerData(playerDataManage.list[index]);
            }
            player[i].ShowPlayerInfo();
        }
    }

    /// <summary>显示底牌
    /// </summary>
    public void InsShowBackPoker()
    {
        if (PokerManage.listBack == null)
        {
            return;
        }
        //父物体
        GameObject parent = GameObject.Find("BackPoker");
        for (int i = 0; i < PokerManage.listBack.Count; i++)
        {
            //获取脚本物体
            GameObject go = PokerManage.listBack[i].gameObject;
            //设为底牌子物体
            go.transform.SetParent(parent.transform);
            //底牌位置偏移
            go.transform.localPosition = new Vector3(i * 1.5f, 0, 0);
            //缩放
            go.transform.localScale = Vector3.one;
            if (i > 2)
            {
                go.transform.localScale = Vector3.zero;
            }
        }
    }

    /// <summary>清空底牌
    /// </summary>
    public void ClearBackPoker()
    {
        if (PokerManage.listBack == null)
        {
            return;
        }
        for (int i = 0; i < PokerManage.listBack.Count; i++)
        {
            Destroy(PokerManage.listBack[i].gameObject);
        }
        //列表数据清空
        PokerManage.listBack.Clear();
    }

    /// <summary>抢地主事件
    /// </summary>
    public void ClickBtnIsGrabEvent()
    {
        AudioSound.CreateSoundPlay("jiaodizhu");
        //临时牌组,用于显示后的选择切换操作
        Poker[] tempPoker = new Poker[PokerManage.listBack.Count];

        //当前最大,出牌时优先
        maxPlayerIndex = curPlayer;

        //遍历底牌,添加数据显示到手牌
        for (int i = 0; i < PokerManage.listBack.Count; i++)
        {
            GameObject go = Instantiate(PokerManage.listBack[i].gameObject);
            Poker poker= go.GetComponent<Poker>();
            tempPoker[i] = poker;
            go.AddComponent<BoxCollider2D>().size = new Vector2(1.05f, 1.5f);
            //添加进手牌
            go.transform.SetParent(player[curPlayer].transform.Find("HandPoker"));
            player[curPlayer].pokerList.Add(go.GetComponent<Poker>());
            //底牌可见
            PokerManage.listBack[i].isVisible = true;
        }
        //地主设定
        player[curPlayer].roleType = RoleType.landlord;

        //角色分配
        for (int i = 0; i < player.Length; i++)
        {
            player[i].roleType = i != maxPlayerIndex ? RoleType.peasant : RoleType.landlord;
            player[i].transform.Find("UI/HeadImage/RoleType").GetComponent<Text>().text = player[i].roleType == RoleType.landlord ? "地主" : "农民";

            if (player[i].operateType != OperateType.Protagonist)
            {
                player[i].transform.Find("UI/SurplusCount").GetComponent<Text>().text = "剩余: " + player[i].pokerList.Count + "张";
            }
        }
        player[curPlayer].PokerSort();
        player[curPlayer].ShowPoker();

        //主视角选择及显示切换
        if (player[curPlayer].operateType == OperateType.Protagonist)
        {
            for (int i = 0; i < tempPoker.Length; i++)
            {
                tempPoker[i].isVisible = true;
                tempPoker[i].ChangeSelect();
            }
        }

        //进入轮牌
        state = GameState.OutPoker;
        View_GrabLandLord.SetActive(false);
    }

    /// <summary>不抢地主事件
    /// </summary>
    public void ClickBtnNoGrabEvent()
    {
        AudioSound.CreateSoundPlay("bujiao");
        //下一个玩家
        curPlayer = ++curPlayer % player.Length;
        View_GrabLandLord.SetActive(false);
    }

    /// <summary>开始按钮事件
    /// </summary>
    public void ClickBtnGameStartEvent()
    {
        Btn_Start.SetActive(false);
        state = GameState.GameStart;
    }

    /// <summary>重新开始按钮事件
    /// </summary>
    public void ClickBtnReGameStartEvent()
    {
        GameObject.Find("Multiple").GetComponent<Text>().text = "倍数：" + MultipleCount;
        Btn_ReStart.SetActive(false);
        View_GameOverView.SetActive(false);
        //清除玩家手牌及显示
        for (int i = 0; i < player.Length; i++)
        {
            player[i].transform.Find("UI/HeadImage/RoleType").GetComponent<Text>().text = "";
            player[i].ClearHandPoker();
            player[i].ClearPlayerLastPoker();
        }
        //清除底牌
        ClearBackPoker();
        //清除等待牌
        PokerManage.waitPoker.Clear();
        //清除上一次轮牌
        PokerManage.lastPoker.Clear();
        //清除最大牌
        PokerManage.maxPokerValue = -1;
        //游戏重新开始
        state = GameState.GameStart;
    }

    /// <summary>游戏结束操作
    /// </summary>
    public void GameOver()
    {
        //关闭时钟显示
        timeCanvas.GetComponent<Canvas>().enabled = false;
        Btn_ReStart.SetActive(true);
        View_GameOverView.SetActive(true);

        for (int i = 0; i < player.Length; i++)
        {
            player[i].ShowPoker(true);
            //显示名字
            View_GameOverView.transform.Find("Player" + i).GetComponent<Text>().text = player[i].info.name;
            //显示角色
            View_GameOverView.transform.Find("Player" + i + "_type").GetComponent<Text>().text = player[i].roleType == RoleType.landlord ? "地主" : "农民";

            //得分
            int score = 0;
            //分数符号
            string scoreF = "";
            //角色是否胜利判断是加分是减分
            scoreF = player[maxPlayerIndex].roleType == player[i].roleType ? "+" : "-";
            //是农民还是地主,农民只有倍分一半,地主全部倍分
            score = player[i].roleType == RoleType.peasant ? (baseScore * MultipleCount) / 2 : baseScore * MultipleCount;
            //显示
            Text scoreText = View_GameOverView.transform.Find("Player" + i + "_Score").GetComponent<Text>();
            scoreText.text = scoreF + score;
            //数据中当前分加分和减分操作
            int curScore = int.Parse(player[i].info.score);
            player[i].info.score = scoreF == "+" ? (curScore + score).ToString() : (curScore - score).ToString();

            //显示信息
            player[i].ShowPlayerInfo();
        }
        //仅更新当前角色信息
        playerDataManage.UpdatePlayerInfoToXml(player[0].info);
        //倍数初始化
        MultipleCount = 2;
        AudioSound.CreateSoundPlay(player[maxPlayerIndex].operateType==OperateType.Protagonist ? "win" : "fail");
        state = GameState.Empty;
    }

    /// <summary>发牌按钮事件
    /// </summary>
    public void ClickBtnDealPokerEvent()
    {
        StartCoroutine(DealPoker());
    }

    /// <summary>出牌事件
    /// </summary>
    public void ClickBtnOutPokerEvent()
    {
        //是否符合出牌规则并出牌
        if (player[curPlayer].IFOutPoker())
        {
            AudioSound.CreateSoundPlay("outcard1");
            OutPokerEffects();
            //出牌按钮隐藏
            View_OutAndPassPoker.SetActive(false);
            //任意一方出完全部牌
            if (player[curPlayer].pokerList.Count == 0)
            {
                //设为当前最大
                maxPlayerIndex = curPlayer;
                //游戏结束
                state = GameState.GameOver;
                return;
            }
            //显示扑克
            player[curPlayer].ShowPoker();
            //设为当前最大
            maxPlayerIndex = curPlayer;
            //下一个玩家
            curPlayer = ++curPlayer % player.Length;
            //重置等待时间
            time = 0;
        }
    }

    /// <summary>出牌成功时的特殊牌型动画显示
    /// </summary>
    public void OutPokerEffects()
    {
        string fileName = "";
        //炸弹
        if (PokerRules.SelectPokerAct == PokerRules.SelectJokerBoom || PokerRules.SelectPokerAct == PokerRules.SelectBoom)
        {
            fileName = PokerRules.SelectPokerAct == PokerRules.SelectJokerBoom ? "wangzha" : "zhadan";
            Instantiate(ResourcesManage.dictionary["Anime_Boom"] as GameObject);
            //倍数动画
            GameObject combo = Instantiate(ResourcesManage.dictionary["Anime_Combo"] as GameObject);
            Vector3 ver = combo.transform.localPosition;
            combo.transform.DOLocalJump(new Vector3(ver.x, ver.y + 0.2f, ver.z), 2, 0, 0.5f);
            MultipleCount *= 2;
            GameObject.Find("Multiple").GetComponent<Text>().text = "倍数：" + MultipleCount;
            AudioSound.CreateSoundPlay("double");
            AudioSound.CreateSoundPlay("bomb");
        }
        //飞机带翅膀
        else if (PokerRules.SelectPokerAct == PokerRules.SelectTripleStraightAnd)
        {
            fileName = "feiji";
            Instantiate(ResourcesManage.dictionary["Anime_Triple"] as GameObject);
        }
        //顺子
        else if (PokerRules.SelectPokerAct == PokerRules.SelectStraight)
        {
            fileName = "shunzi";
            Instantiate(ResourcesManage.dictionary["Anime_Straight"] as GameObject);
        }
        //双顺
        else if (PokerRules.SelectPokerAct == PokerRules.SelectDoubleStraight)
        {
            fileName = "liandui";
            Instantiate(ResourcesManage.dictionary["Anime_Straight"] as GameObject);
        }
        //飞机不带
        else if (PokerRules.SelectPokerAct == PokerRules.SelectTripleStraight)
        {
            fileName = "feiji";
            Instantiate(ResourcesManage.dictionary["Anime_Triple"] as GameObject);
        }
        //三带二
        else if (PokerRules.SelectPokerAct == PokerRules.SelectThreeAndTwo)
        {
            fileName = "sandaiyidui";
        }
        //三带一
        else if (PokerRules.SelectPokerAct == PokerRules.SelectThreeAndOne)
        {
            fileName = "sandaiyi";
        }
        //三不带
        else if (PokerRules.SelectPokerAct == PokerRules.SelectOnlyThree)
        {
            fileName = "sange";
        }
        else
        {
            //对子和单张
            fileName = PokerRules.SelectPokerAct == PokerRules.SelectDouble ? "dui" : "";

            int index = 3;
            foreach (PokerValueType item in System.Enum.GetValues(typeof(PokerValueType)))
            {
                if (PokerManage.maxPokerValue == (int)item)
                {
                    fileName += index;
                }
                index++;
            }
            if (player[curPlayer].pokerList.Count == 2)
            {
                fileName = "baojing2";
                AudioSound.CreateSoundPlay("alert");
            } 
            if (player[curPlayer].pokerList.Count == 1)
            {
                fileName = "baojing1";
                AudioSound.CreateSoundPlay("alert");
            }
        }
        AudioSound.CreateSoundPlay(fileName);
    }

    /// <summary>"过牌"按钮事件
    /// </summary>
    public void ClickBtnPassPokerEvent()
    {
        //当前玩家最大跳出 不可过牌
        if (curPlayer == maxPlayerIndex)
        {
            player[curPlayer].ShowPoker();
            return;
        }
        AudioSound.CreateSoundPlay("buyao" + Random.Range(1, 4));

        //清空玩家上一次出牌
        player[curPlayer].ClearPlayerLastPoker();

        //创建出牌显示信息模板
        GameObject go = Instantiate(ResourcesManage.dictionary["TemplatePokerShow"] as GameObject);
        //放置在玩家出牌显示中
        go.transform.SetParent(player[curPlayer].transform.Find("LastPoker"));
        go.transform.localScale = Vector3.one;
        //图为当前扑克
        go.GetComponent<Image>().sprite = ResourcesManage.CreateSprite("Pass");
        //添加进出牌显示扑克组中
        player[curPlayer].outShowPokers.Add(go);

        //过牌时重新显示
        player[curPlayer].ShowPoker();
        //下一个玩家
        curPlayer = ++curPlayer % player.Length;
        time = 0;
        View_OutAndPassPoker.SetActive(false);
    }

    /// <summary>"提示"按钮事件
    /// </summary>
    public void ClickBtnTripPokerEnvet()
    {
        if (PokerRules.SelectPoker(player[curPlayer].pokerList, PokerManage.maxPokerValue, PokerManage.lastPoker))
        {
            if (PokerManage.waitPoker.Count == 0)
            {
            }
            for (int i = 0; i < PokerManage.waitPoker.Count; i++)
            {
                PokerManage.waitPoker[i].ChangeSelect();
            }
            PokerManage.waitPoker.Clear();
        }
        else
        {
            ClickBtnPassPokerEvent();
        }
    }

    /// <summary>"设置"按钮事件
    /// </summary>
    public void ClickBtnSettingEvent()
    {
        if (GameObject.Find("Setting").transform.localScale==Vector3.zero)
        {
            GameObject.Find("Setting").transform.DOScale(Vector3.one, 0.25f);
        }
    }

    /// <summary>设置人物界面,点击确定时的"按钮"事件
    /// </summary>
    public void ClickBtnSetPlayerInfoEvent()
    {
        string name = GameObject.Find("InputField_Name").GetComponent<InputField>().text;
        if (name == "")
        {
            LoadGameContorller.TripsShow("请输入名字!");
            return;
        }
        //获取选择图片内容
        Sprite spr = GameObject.Find("SelectHeadImage").GetComponent<Image>().sprite;
        //玩家赋值更新xml
        playerDataManage.curPlayer.name = name;
        playerDataManage.curPlayer.imageRef = spr.name;
        playerDataManage.UpdateCurPlayerInfoToXml();
        View_EnterPlayerInfo.SetActive(false);
        Btn_Start.SetActive(true);
        InitializePlayersInfo();
        //空状态
        state = GameState.Empty;
    }

    /// <summary>头像选择时,图片向左按钮事件
    /// </summary>
    public void ClickBtnLeftHeadImageEvent()
    {
        ResourcesManage.headImageIndex = ResourcesManage.headImageIndex - 1 < 0 ? 0 : ResourcesManage.headImageIndex - 1;
        View_EnterPlayerInfo.transform.Find("Text/InputField_Name/Image/SelectHeadImage").GetComponent<Image>().sprite = ResourcesManage.headImage[ResourcesManage.headImageIndex];
    }
    
    /// <summary>头像选择时,图片向右按钮事件
    /// </summary>
    public void ClickBtnRightHeadImageEvent()
    {
        ResourcesManage.headImageIndex = ResourcesManage.headImageIndex + 1 > ResourcesManage.headImage.Count - 1 ? ResourcesManage.headImage.Count - 1 : ResourcesManage.headImageIndex + 1;
        View_EnterPlayerInfo.transform.Find("Text/InputField_Name/Image/SelectHeadImage").GetComponent<Image>().sprite = ResourcesManage.headImage[ResourcesManage.headImageIndex];
    }

    /// <summary>"关闭"按钮事件
    /// </summary>
    public void ClickBtnCloseEnvet()
    {
        Application.Quit();
    }

}