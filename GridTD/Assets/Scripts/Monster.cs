using UnityEngine;
using System.Collections;
using Assets.Scripts;
using UnityEngine.UI;
using UnityEditor;

[ExecuteInEditMode]
public class Monster : MonoBehaviour 
{

    public MonsterInfo monsterInfo;
    public Slider showHp;
    public int curHp;
    GameController gameController;
	// Use this for initialization
	void Start ()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        monsterInfo = gameController.monsterList[0];
        listCount = gameController.roadList.Count;
        curHp = monsterInfo.hp;
	}

    public int count = 1;

    public int listCount;
	// Update is called once per frame
	void Update ()
    {
        showHp.value = (float)curHp / (float)monsterInfo.hp;
        if (gameController.roadList.Count > 1)
        {
            transform.position += (gameController.roadList[count].transform.position - transform.position).normalized * Time.deltaTime;
            if ((gameController.roadList[count].transform.position - transform.position).magnitude <= 0.01f)
            {
                count++;
                if (count == gameController.roadList.Count)
                {
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }

        if (curHp <= 0)
        {
            Destroy(gameObject);
        }
	}


}