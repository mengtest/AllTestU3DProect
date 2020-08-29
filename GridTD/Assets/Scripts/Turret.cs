using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine.UI;


public class Turret : MonoBehaviour
{
    public TurretInfo turretInfo;
    public List<GameObject> list;
    public List<string> textList;
    public GameObject bullet;

	// Use this for initialzation
	void Start () 
    {
        
	}

    float time;
	// Update is called once per frame
	void Update ()
    {
        if (list.Count >= 1)
        {
            if (list[0] == null)
            {
                list.RemoveAt(0);
                textList.RemoveAt(0);
            }
            time += Time.deltaTime;
            if (time >= turretInfo.attackSpeed)
            {
                time = 0;
                Free();
            }
        }
	}

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag=="Monster")
        {
            list.Remove(collision.gameObject);
            textList.Remove(collision.name);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag=="Monster")
        {
            list.Add(collision.gameObject);
            textList.Add(collision.name);
        }
    }

    //public void OnMouseUp()
    //{
    //    Canvas.SetActive(true);
    //}

    void Free()
    {
        switch (turretInfo.type)
        {
            case TurretType.singleShot:
                {
                    GameObject go = Instantiate(bullet);
                    go.transform.position = transform.position;
                    go.GetComponent<Bullet>().target = list[0];
                    go.GetComponent<Bullet>().father = turretInfo;
                    break;
                }
            case TurretType.doubleShot:
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        GameObject go = Instantiate(bullet);
                        go.transform.position = transform.position;
                        go.GetComponent<Bullet>().target = list[i];
                        go.GetComponent<Bullet>().father = turretInfo;
                    }
                    break;
                }
            default:
                break;
        }
    }
}