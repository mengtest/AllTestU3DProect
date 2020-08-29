using UnityEngine;
using System.Collections;
using Assets.Scripts.Main;

public class Poker : MonoBehaviour
{
    //扑克信息
    public PokerInfo info;
    //是否选中
    public bool isSelect = false;
    public bool isOk = false;
    public bool isVisible = false;
    //初始位置记录
    Vector3 ver;
    SpriteRenderer spr;
	// Use this for initialization
	void Start ()
    {
        spr = GetComponent<SpriteRenderer>();
        spr.sprite = ResourcesManage.CreateSprite("BeiMian");
	}

	// Update is called once per frame
	void Update ()
    {
        if (isVisible)
        {
            spr.sprite = info.spr;
        }
	}

    public void OnMouseDown()
    {
        AudioSound.CreateSoundPlay("xuanpai");
        ChangeSelect();
    }

    public void ChangeSelect()
    {
        if (isOk)
        {
            isSelect = !isSelect;
            ver = isSelect ? transform.position : ver;
            transform.position = isSelect ? new Vector3(ver.x, ver.y + 0.2f, ver.z) : ver;
        }
    }
}
