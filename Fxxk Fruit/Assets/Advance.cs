using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Advance : MonoBehaviour {
    /// 水果样式
    public Sprite[] spr;
    /// 血量条
    Image ima;
    /// 水果下标
    public int type;
    /// 下落向量
    public Vector2 ver;

    Rigidbody2D rigidbody2d;

    GameObject player;
    FxxkFruit gameController;

    GameObject NextAdvance;
    public Vector3 NextAdvanceVer;

	// Use this for initialization
	void Start ()
    {
        gameController = GameObject.Find("GameController").GetComponent<FxxkFruit>();
        player = GameObject.Find("ColumnShortSprite");
        type = Random.Range(0, spr.Length);
        GetComponent<SpriteRenderer>().sprite = spr[type];
        GetComponent<SpriteRenderer>().color = gameController.color;
        Vector3 trm = new Vector3(Random.Range(gameController.RandomMinPoxX, gameController.RandomMaxPoxX), 5.66f, 0);
        transform.position = trm;
        ver = new Vector2(0, gameController.DropSpace);
        ///创建物体时的添加多边形碰撞
        gameObject.AddComponent<PolygonCollider2D>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        ///将上一个物体的"下一个(NextAdvance)"设为自己(判断列表内是否有内容，为空跳过(跳过第一次无物体))
        GameObject LastGo = gameController.list.Count > 1 ? gameController.list[gameController.list.IndexOf(gameObject) - 1] as GameObject : null;
        if (LastGo != null)
        {
            LastGo.GetComponent<Advance>().NextAdvance = gameObject;
        }
        clo = GetComponent<SpriteRenderer>().color;
	}

    public void Inis()
    {

        //print();
    }
    Color32 clo;
	// Update is called once per frame
	void Update ()
    {
        GetComponent<LineRenderer>().SetPosition(0, transform.position);
        GetComponent<LineRenderer>().SetPosition(1, transform.position);

        ///"下一个"不为空时，获得平行坐标
        if (NextAdvance != null)
        {
            NextAdvanceVer = new Vector2(NextAdvance.transform.position.x, transform.position.y);
        }
        if (gameController.IsOpenLine)
        {        //调试用划线
            if (NextAdvance != null && NextAdvance.tag != "IsContact")
            {
                GetComponent<LineRenderer>().SetPosition(0, transform.position);
                GetComponent<LineRenderer>().SetPosition(1, NextAdvanceVer);
            }
        }

        ///透明度渐变
        //clo.a = clo.a >= 6 ? clo.a -= 6 : (byte)0;
        //GetComponent<SpriteRenderer>().color = clo;

        ///是否是被接住
        bool IsContact = gameObject.tag == "IsContact" ? true : false;
        ///是否是越界
        bool IsThrough = transform.position.y <= player.transform.position.y ? true : false;
        ///接住的水果与没有接住的水果下落速度控制
        rigidbody2d.velocity = IsContact ? rigidbody2d.velocity : ver;

        ///越界
        if (IsThrough)
        {
            ///没有接住
            if (!IsContact)
            {
                ///确认未接住
                player.GetComponent<RoleMove>().IsContact = true;
                ///生命减少
                gameController.Hp.value -= 0.02f + (gameController.difficulty / 500);
            }
            gameController.list.Remove(gameObject);
            ///删除物体
            Destroy(gameObject);
        }
     }
}