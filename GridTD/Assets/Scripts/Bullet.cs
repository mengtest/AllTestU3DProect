using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class Bullet : MonoBehaviour {

    public GameObject target;
    public TurretInfo father;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (target!=null)
        {
            transform.position += (target.transform.position - transform.position).normalized * Time.deltaTime*5;
            if ((target.transform.position - transform.position).magnitude <= 0.05f)
            {
                target.GetComponent<Monster>().curHp -= father.attack;
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
	}
}
