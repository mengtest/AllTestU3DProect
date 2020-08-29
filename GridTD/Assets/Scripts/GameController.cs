using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Assets.Scripts;
using System.Xml;
using UnityEditor;

public class GameController : MonoBehaviour {

    public GameObject text;
    public Text time;
    public GameObject monster;

    public GameObject GirdFa;
    public GameObject gridsRightMenus;
    public GameObject gridsLeftMenus;


    public GameObject turretInfoPanel;

    public GameObject turret;

    public List<GameObject> roadList;
    public List<TurretInfo> turretList;
    public List<MonsterInfo> monsterList;

    public float money = 100;
    public Text moneyShow;

    public TextAsset xml;
	// Use this for initialization
	void Start ()
    {
        LoadXml();
        roadList = LoadMapPass(1, 1);
        turretList = LoadTurret();
        monsterList = LoadMonster();
	}

    IEnumerator DoAssault() 
    {
        //游戏开启后5秒钟
        //yield return new WaitForSeconds(5);
        int count = 0;
        //20波
        while (count < 50)
        {
            //发射一波敌人，并在屏幕上给出提示（一大波敌人正在袭来）。
            text.SetActive(true);
            //一秒后提示消失
            yield return new WaitForSeconds(1);
            text.SetActive(false);
            //创建五个敌人
            for (int i = 0; i < 10; i++)
            {
                GameObject go = Instantiate(monster);
                go.transform.SetParent(GameObject.Find("Monsters").transform);
                go.name = "第" + (count+1) + "波第" + (i+1) + "个";
                go.transform.position = roadList[0].transform.position;
                yield return new WaitForSeconds(1);
                if (isGameStart)
                {
                    StopCoroutine("DoAssault");
                }
            }
            //每波敌人发射的间隔时间为5秒。
            yield return new WaitForSeconds(5);
            count++;
        }
    }
    public bool isGameStart;
	// Update is called once per frame
	void Update ()
    {
        moneyShow.text = money.ToString();
        if (roadList.Count>0)
        {
            if (isGameStart)
            {
                isGameStart = false;
                StartCoroutine("DoAssault");
            }
        }
        time.text = Time.time.ToString();

	}

    GameObject GetRoadCoordInfoAtPosXY(int _x, int _y, GridType _type = GridType.NullGrid)
    {
        GameObject go = GirdFa.transform.GetChild(_x).GetChild(_y).gameObject;
        go.GetComponent<Grid>().type = _type;
        return go;
    }

    XmlDocument xmlDoc = new XmlDocument();
    void LoadXml()
    {
        xmlDoc.LoadXml(xml.text);

    }
    List<MonsterInfo> LoadMonster()
    {
        XmlNodeList list = xmlDoc.SelectSingleNode("/game/monster").ChildNodes;
        List<MonsterInfo> tempList = new List<MonsterInfo>();

        foreach (XmlElement item in list)
        {
            MonsterInfo mon = new MonsterInfo();
            mon.name = item.Attributes["name"].Value;
            mon.hp = int.Parse(item.Attributes["hp"].Value);
            mon.speed = float.Parse(item.Attributes["speed"].Value);
            tempList.Add(mon);
        }
        return tempList;
    }
    MonsterInfo LoadMonster(int _index)
    {
        XmlNodeList list = xmlDoc.SelectSingleNode("/game/monster").ChildNodes;
        if (_index >= list.Count)
        {
            return null;
        }
        MonsterInfo mon = new MonsterInfo();
        mon.name = list[_index].Attributes["name"].Value;
        mon.hp = int.Parse(list[_index].Attributes["hp"].Value);
        mon.speed = float.Parse(list[_index].Attributes["speed"].Value);
        return mon;
    }

    List<GameObject> LoadMapPass(int _mapID, int _passID)
    {
        List<GameObject> list = new List<GameObject>();

        XmlNodeList xmlList = xmlDoc.SelectSingleNode("/game/pass").ChildNodes;

        foreach (XmlElement item in xmlList[_mapID].ChildNodes[_passID])
        {
            list.Add(GetRoadCoordInfoAtPosXY(int.Parse(item.Attributes["posX"].Value), int.Parse(item.Attributes["posY"].Value), GridType.Road));
        }
        return list;
    }

    List<TurretInfo> LoadTurret()
    {
        List<TurretInfo> turret = new List<TurretInfo>();
        XmlNodeList xmlList = xmlDoc.SelectSingleNode("/game/turret").ChildNodes;


        foreach (XmlElement item in xmlList)
        {
            TurretInfo tur = new TurretInfo();
            tur.name = item.Attributes["name"].Value;
            tur.attack = int.Parse(item.Attributes["attack"].Value);
            tur.attackSpeed = float.Parse(item.Attributes["attackSpeed"].Value);
            tur.money = int.Parse(item.Attributes["money"].Value);
            tur.desc = item.Attributes["desc"].Value;
            turret.Add(tur);
        }
        //mon.money = int.Parse(xmlList[_index].Attributes["money"].Value);
        return turret;
    }
}