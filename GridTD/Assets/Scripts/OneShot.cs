using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OneShot : MonoBehaviour
{

    //public List<GameObject> list;
    //public List<string> textList;
    //public GameObject bullet;

    //public float AttSpeed = 1;
	// Use this for initialzation
	void Start () 
    {
	
	}
    //float time;
	// Update is called once per frame
	void Update () 
    {
        //if (list.Count >= 1)
        //{
        //    time += Time.deltaTime;
        //    if (time >= AttSpeed)
        //    {
        //        GameObject go = Instantiate<GameObject>(bullet);
        //        go.transform.position = transform.position;
        //        go.GetComponent<Bullet>().target = list[0];
        //        time = 0;
        //    }
        //}
	}

    //public void OnTriggerExit2D(Collider2D collision)
    //{
        //if (collision.tag=="Monster")
        //{
        //    list.Remove(collision.gameObject);
        //    textList.Remove(collision.name);
        //}
    //}

    //public void OnTriggerEnter2D(Collider2D collision)
    //{
        //if (collision.tag=="Monster")
        //{
        //    list.Add(collision.gameObject);
        //    textList.Add(collision.name);
        //}
    //}
}
