using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class FxxkFruit : MonoBehaviour {

    ///都可以[HideInInspector]隐藏

    public float MinCreateSpace = 0.1f;
    public float MaxCreateSpace = 5f;
    public float DropSpace = -5f;
    public bool IsGameStart = false;
    public bool IsGameOver = false;
    public float maxScore = 0f;
    public float addScore = 0f;
    public float curScore = 0f;
    public float addScorePlus = 50f;
    public float difficultyTimer = 1f;
    public float difficulty = 1f;
    public int combo = 1;
    public int maxCombo = 10;
    public int count = 0;
    public float accelerationValue = 0.2f;
    public float ShiftPower = 1f;
    public float RandomMinPoxX = -2f;
    public float RandomMaxPoxX = 2f;
    //物体刷新计数
    public float intervalTime = 0;
    //刷新的物体
    public GameObject random;

    public Color32 color;

    public Slider Hp;
    public Image Power;
    public Text HistoryMaxScore;
    public Text CurScore;
    public GameObject CurScoreObj;
    public Text Score;
    public Text Combo;
    public GameObject GameWindows;
    public GameObject StartGame;
    public GameObject QuitGame;
    public GameObject ReGame;
    public ArrayList list;


    public bool IsOpenLine = false;
	// Use this for initialization
	void Start ()
    {
        list = new ArrayList();
        ///历史最高分显示
        HistoryMaxScore.text = PlayerPrefs.GetInt("HistoryMaxScore", 0) + "";
    }

    public Image LockPower;
    public Image LenssenRandom;

	// Update is called once per frame
    void Update()
    {
        ///游戏结束判断(生命为空并且没有游戏结束时)
        if (Hp.value <= 0 && !IsGameOver)
        {
            IsGameOver = true;
            Time.timeScale = 0;
            ///物件的隐藏及显示
            GameWindows.SetActive(true);
            CurScoreObj.SetActive(true);
            StartGame.SetActive(false);
            QuitGame.SetActive(true);
            ReGame.SetActive(true);

            ///当破记录时设置当前分为最高分
            if ((int)maxScore > PlayerPrefs.GetInt("HistoryMaxScore", 0))
            {
                PlayerPrefs.SetInt("HistoryMaxScore", (int)maxScore);
            }

            ///显示最高分
            HistoryMaxScore.text = PlayerPrefs.GetInt("HistoryMaxScore") + "";

            ///显示当前获得分
            CurScore.text = (int)maxScore + "";

            ClearGameObjectsOnTag("Donga");
            return;
        }
        ///游戏没有开始
        if (!IsGameStart)
        {
            return;
        }
        ///游戏结束了
        if (IsGameOver)
        {
            CurScore.text = (int)maxScore + "";
            return;
        }
        ///游戏难度计时及增加
        difficultyTimer += Time.deltaTime;
        if (difficultyTimer >= 5)
        {
            color = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);
            difficulty++;
            difficultyTimer = 0;
            ///当前已经增加到最大难度时，仅增加分数不判断和播放动画
            if (difficulty < 20)
            {
                GameObject.Find("DongaText").GetComponent<Text>().text = "难度增加   " + difficulty + "/20";
                BlinkGameObject("DifficultyDonga");
            }
            else if (difficulty == 20)
            {
                GameObject.Find("DongaText").GetComponent<Text>().text = "难度满载！开启持续挑战模式！";
                BlinkGameObject("DifficultyDonga");
            }
            else
            {
                RandomMaxPoxX = (RandomMaxPoxX + 0.1f) > 5.49f ? 5.49f : RandomMaxPoxX + 0.2f;
            }
            ///持续增加随机范围和下落速度至最大
            RandomMinPoxX = (RandomMinPoxX - 0.1f) < -4.67f ? -4.67f : RandomMinPoxX - 0.2f;
            DropSpace = (DropSpace - 0.5f) < -10 ? -10 : DropSpace - 0.5f;
            MaxCreateSpace = (MaxCreateSpace - 0.5f) < 0.2f ? 0.2f : MaxCreateSpace - 0.5f;
        }

        ///物体刷新间隔
        intervalTime += Time.deltaTime;
        if (intervalTime >= Random.Range(MinCreateSpace,MaxCreateSpace))
        {
            ///只刷新一个物体
            GameObject go = Instantiate(random);
            go.transform.SetParent(GameObject.Find("Randoms").transform);
            ///添加到列表
            list.Add(go);
            intervalTime = 0;
        }

        ///平滑显示分数
        if (addScore!=0)
        {
            curScore += (addScore * 0.153f);
            if (curScore>=maxScore)
            {
                curScore = maxScore;
                addScore = 0;
            }
            Score.text = "Score:" + (int)curScore;
        }
	}


    /// <summary>
    /// 按钮点击的开始事件
    /// </summary>
    public void OnGameStart()
    {
        Time.timeScale = 1;
        IsGameStart = true;
    }

    /// <summary>
    /// 游戏重置事件
    /// </summary>
    public void OnReGame()
    {
        MinCreateSpace = 0.1f;
        MaxCreateSpace = 5;
        DropSpace = -5;
        maxScore = 0;
        addScore = 0;
        curScore = 0;
        difficultyTimer = 0;
        difficulty = 1;
        combo = 1;
        count = 0;
        ShiftPower = 1;
        RandomMinPoxX = -2;
        RandomMaxPoxX = 2;
        intervalTime = 0;

        Hp.value = 1;
        LockPower.fillAmount = 1;
        LenssenRandom.fillAmount = 1;
        IsGameOver = false;
        GameWindows.SetActive(false);

        Score.text = "Score:0";
        ClearGameObjectsOnTag("Fruit");
        ClearGameObjectsOnTag("Phantom");
        ClearGameObjectsOnTag("IsContact");
        ClearGameObjectsOnTag("Donga");

        Time.timeScale = 1;
    }

    /// <summary>
    /// 退出游戏事件
    /// </summary>
    public void OnQuitGame()
    {
        Application.Quit();
    }


    /// <summary>
    /// 删除所有tag为_tag的物体
    /// </summary>
    /// <param name="_tag"></param>
    public void ClearGameObjectsOnTag(string _tag)
    {
        foreach (GameObject item in GameObject.FindGameObjectsWithTag(_tag))
        {
            Destroy(item);
        }
    }

    /// <summary>
    /// 指定一个名为_str带有RectTransform和Imag组件的物件在背景中心处缩放动画处理
    /// </summary>
    /// <param name="_str"></param>
    public void BlinkGameObject(string _str) 
    {
                ///闪烁背景动画
        GameObject go = Instantiate<GameObject>(GameObject.Find(_str));

        if (go==null)
        {
            return;
        }

        go.tag = "Donga";
        go.transform.SetParent(GameObject.Find("Canvas").transform);
        go.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, 0);
        go.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
        go.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, 0);
        go.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
        go.GetComponent<RectTransform>().anchorMin = Vector2.zero;
        go.transform.localScale = Vector3.one;
        ///闪烁
        Sequence seq = DOTween.Sequence();
        seq.Append(go.GetComponent<Transform>().DOScale(1.2f, 1f));
        seq.Join(go.GetComponent<Image>().DOFade(0, 1f));
        Destroy(go, 1);
    }
}