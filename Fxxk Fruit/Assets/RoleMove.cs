using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class RoleMove : MonoBehaviour {
    FxxkFruit gameController;
    float acceleration = 0;

    // Use this for initialization
	void Start ()
    {
        gameController = GameObject.Find("GameController").GetComponent<FxxkFruit>();
    }

	// Update is called once per frame
	void Update ()
    {
        //如果游戏结束 && 如果游戏没有开始
        if (gameController.IsGameOver || !gameController.IsGameStart)
        {
            return;
        }

        gameController.Power.fillAmount = gameController.ShiftPower;
        gameController.Hp.value = gameController.Hp.value - 0.001f;
        Vector3 trm = transform.position;
        ///加速度
        if (Input.GetKey(KeyCode.LeftShift) && gameController.ShiftPower > 0)
        {
            //Power.SetActive(true);
            ///加速度移动
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                ///移动时的能量减小时判断，是否小于最小值，小于置为零
                gameController.ShiftPower = gameController.ShiftPower <= 0 ? 0 : gameController.ShiftPower - 0.02f;
                ///制造人物残影
                GameObject go = Instantiate<GameObject>(GameObject.Find("Phantom"));
                go.GetComponent<SpriteRenderer>().enabled = true;
                go.transform.SetParent(GameObject.Find("Phantoms").transform);
                go.tag = "Phantom";
                go.transform.position = trm;
                ///残影朝向参照主体
                go.GetComponent<SpriteRenderer>().flipX = gameObject.GetComponent<SpriteRenderer>().flipX;
                //go.transform.DOScale(0, 0.5f);
                ///动画渐隐删除物体
                go.GetComponent<SpriteRenderer>().DOFade(0f, 0.5f);
                Destroy(go, 0.5f);
            }
            acceleration = gameController.accelerationValue;
        }
        else
        {
            acceleration = 0;
        }
        gameController.ShiftPower = gameController.ShiftPower >= 1 ? 1 : gameController.ShiftPower + 0.001f;
        if (Input.GetKey(KeyCode.A))
        {
            trm.x += -0.1f - acceleration;
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        if (Input.GetKey(KeyCode.D))
        {
            trm.x += 0.1f + acceleration;
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        ///限制空间
        trm.x = Mathf.Clamp(trm.x, -5.13f, 5.25f);
        transform.position = trm;
        if (IsContact)
        {
            gameController.Combo.text = "";
            gameController.count = 0;
            IsContact = false;
        }
	}
    public bool IsContact = false;
    public void OnCollisionEnter2D(Collision2D collision)
    {
        ///如果接到水果
        if (collision.gameObject.tag == "Fruit")
        {
            //if (Input.GetKey(KeyCode.LeftShift))
            //{
            //    ///当前物体与下一个物体距离过远时
            //    if ((collision.gameObject.GetComponent<Advance>().NextAdvanceVer - collision.gameObject.transform.position).magnitude > 7)
            //    {
            //        if (Input.GetKey(KeyCode.A))
            //        {
                        
            //        }
            //    }
            //}
            ///计数
            gameController.count++;

            ///放在盘子里
            collision.gameObject.transform.SetParent(gameObject.transform);
            ///不可移动
            collision.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            ///改变tag
            collision.gameObject.tag = "IsContact";
            ///删除碰撞体
            Destroy(collision.gameObject.GetComponent<PolygonCollider2D>());
            ///计数及显示
            GameObject.Find("Combo").GetComponent<Text>().text = "x " + gameController.count.ToString();
            ///Combo跳动动画
            GameObject.Find("Combo").transform.DOScale(5f, 0.25f);
            GameObject.Find("Combo").transform.DOScale(1f, 0.25f);


            ///接到水果时的能量增加(加成为基础0.1+难度/150)，以及增加时的超出限制
            gameController.ShiftPower = gameController.ShiftPower >= 1 ? 1 : gameController.ShiftPower + gameController.difficulty / 150;
            ///加成的分数 (计数*基础分*当前难度)
            gameController.addScore = gameController.count * gameController.addScorePlus * gameController.difficulty;

            ///Combo数
            gameController.combo++;
            ///当接满Combo数
            if (gameController.combo >= gameController.maxCombo)
            {
                ///分数加成1.5倍
                gameController.addScore *= 1.5f;
                ///加满水果的动画
                FullCombo();
            }
            ///每个水果恢复生命值(0.01倍的计数)
            gameController.Hp.value = gameController.Hp.value + (0.01f * gameController.count);

            ///当前分变化后的最大值
            gameController.maxScore += gameController.addScore;


        }
    }

    /// <summary>
    /// 接满 Combo 时的水果动作
    /// </summary>
    public void FullCombo()
    {
        ///获取所有接取到的水果
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("IsContact"))
        {
            ///改变物体为可以移动
            item.GetComponent<Rigidbody2D>().isKinematic = false;
            ///向上扇形随机位置加力，散落开下来
            item.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-5, 5), Random.Range(1, 5)) * 50);
            item.transform.SetParent(GameObject.Find("IsContact").transform);

            item.GetComponent<SpriteRenderer>().DOFade(0, 2.5f);
        }
        gameController.combo = 1;

        gameController.BlinkGameObject("BackGround");
    }
}