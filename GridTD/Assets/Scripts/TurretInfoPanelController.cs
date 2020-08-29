using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts;

public class TurretInfoPanelController : MonoBehaviour {

    public GameObject clickGO;
    TurretInfo info;
    public Text name;
    public Text lv;
    public Text attck;
    public Text money;
    public Text desc;

    GameController gameController;
	// Use this for initialization
	void Start () 
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (clickGO != null)
        {
            info = clickGO.GetComponent<Turret>().turretInfo;
            name.text = info.name;
            lv.text = info.lv.ToString();
            attck.text = info.attack.ToString();
            money.text = info.money.ToString();
            desc.text = info.desc;
        }
	}

    public void UpLv()
    {
        info.UpLv();
    }

    public void Destroy()
    {
        gameController.money += info.money * 0.75f;
        clickGO.transform.parent.gameObject.GetComponent<Grid>().type = GridType.NullGrid;
        Destroy(clickGO);
    }
}
