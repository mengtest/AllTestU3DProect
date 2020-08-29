using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts;

public class GridsMenusController : MonoBehaviour 
{

    public GameObject go;

    GameController gameController;
	// Use this for initialization
	void Start () 
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    public void AddRoad()
    {
        gameController.roadList.Add(go);
        go.GetComponent<Grid>().type = GridType.Road;
    }
    public void ClearRoad()
    {
        foreach (GameObject item in gameController.roadList)
        {
            item.GetComponent<Grid>().type = GridType.NullGrid;
        }
        gameController.roadList.Clear();
        gameController.isGameStart = false;
    }
    public void GameStart()
    {
        gameController.isGameStart = true;
    }

    public void CreateDoubleShot()
    {
        CreateTurret(TurretType.doubleShot);
    }
    public void CreateSingleShot()
    {
        CreateTurret(TurretType.singleShot);
    }

    public void CreateTurret(TurretType _type)
    {
        go.GetComponent<Grid>().type = GridType.Turret;
        GameObject turret = Instantiate(gameController.turret);
        Turret t = turret.GetComponent<Turret>();
        //t.type = _type;
        t.turretInfo = gameController.turretList[(int)_type];
        t.turretInfo.type = _type;
        gameController.money -= t.turretInfo.money;
        turret.transform.SetParent(go.transform);
        turret.transform.localScale = new Vector3(78, 79, 0);
        turret.transform.localPosition = Vector3.zero;

        switch (_type)
        {
            case TurretType.singleShot:
                turret.GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
            case TurretType.doubleShot:
                turret.GetComponent<SpriteRenderer>().color = Color.white;
                break;
        }
    }
}
